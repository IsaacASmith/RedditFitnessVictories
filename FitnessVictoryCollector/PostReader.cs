using FitnessVictoryCollector.ApiModels;
using FitnessVictoryCollector.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FitnessVictoryCollector
{
    public class PostReader
    {
        public async Task<IEnumerable<VictoryPost>> GetVictoryPosts(DateTime startDate)
        {
            const string searchTerm = "Victory Sunday";

            var oldestPostFoundSoFarDate = DateTime.Now;
            var after = "";

            var httpClient = new HttpClient();

            var apiPostResults = new List<PostListing>();

            var postSearchUrl = $"https://www.reddit.com/r/Fitness/search.json" +
                                                     $"?q={searchTerm}" +
                                                     $"&restrict_sr=1" +
                                                     $"&sort=new" +
                                                     $"&limit=100";

            while (oldestPostFoundSoFarDate > startDate)
            {
                var nextSearchUrl = $"{postSearchUrl}&after={after}";

                var response = await httpClient.GetAsync(nextSearchUrl);
                var responseContent = await response.Content.ReadAsStringAsync();

                var responseModel = JsonConvert.DeserializeObject<PostQueryResult>(responseContent);
                apiPostResults.AddRange(responseModel.Data.Listings.Where(e => e.Data.Author == "AutoModerator"));

                var nextOldestPostDate = DateTimeOffset.FromUnixTimeSeconds(responseModel.Data.Listings.Min(e => e.Data.CreatedUtc)).Date;

                if(nextOldestPostDate > oldestPostFoundSoFarDate)
                {
                    Console.WriteLine($"Retrieved {responseModel.Data.Listings.Count()} posts. Oldest post so far: {nextOldestPostDate.ToShortDateString()}");
                    break;
                }

                oldestPostFoundSoFarDate = nextOldestPostDate;
                after = responseModel.Data.After;

                Console.WriteLine($"Retrieved {responseModel.Data.Listings.Count()} posts. Oldest post so far: {nextOldestPostDate.ToShortDateString()}");
            }

            Console.WriteLine("- - - - ");

            var allPosts = apiPostResults.GroupBy(e => e.Data.Id).Select(e => e.First()).Select(e => new VictoryPost
            {
                Id = e.Data.Id,
                PostDate = DateTimeOffset.FromUnixTimeSeconds(e.Data.CreatedUtc).UtcDateTime
            }).ToList();

            allPosts = allPosts.GroupBy(e => e.Id).Select(e => e.First()).ToList();
            
            var commentLookups = allPosts.Select(e => GetVictoryCommentCount(e.Id, httpClient));
            var results = await Task.WhenAll(commentLookups);

            foreach(var result in results)
            {
                allPosts.FirstOrDefault(e => e.Id == result.Item1).VictoryCommentCount = result.Item2;
            }

            Console.WriteLine($"Oldest post: {allPosts.OrderBy(e => e.PostDate).FirstOrDefault().PostDate.ToShortDateString()}");

            return allPosts;
        }


        private async Task<(string, int)> GetVictoryCommentCount(string id, HttpClient client)
        {
            var commentsLookupUrl = $"https://www.reddit.com/comments/{id}/.json";

            var response = await client.GetAsync(commentsLookupUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            var responseModel = JsonConvert.DeserializeObject<IEnumerable<CommentListing>>(responseContent);

            Console.WriteLine($"Retrieved data for id {id}...");

            return (id, responseModel.FirstOrDefault(e => e.Data.Children.FirstOrDefault().Kind == "t1")?.Data.Children.Count() ?? 0);
        }
    }
}

//var searchUrl = $"https://www.reddit.com/comments/hps1ri/.json";