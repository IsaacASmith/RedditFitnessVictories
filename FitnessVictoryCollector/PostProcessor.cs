using FitnessVictoryCollector.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessVictoryCollector
{
    public class PostProcessor
    {
        public Dictionary<int, int> GetVictoryPostsForCurrentYear(IEnumerable<VictoryPost> allPosts)
        {
            var x = allPosts.Where(e => e.PostDate.Year == DateTime.Now.Year).GroupBy(e => e.PostWeekNumber);
            return allPosts.Where(e => e.PostDate.Year == DateTime.Now.Year).ToDictionary(e => e.PostWeekNumber, e => e.VictoryCommentCount);
        }

        public Dictionary<int, int> GetAverageVictoryCountForPreviousYears(IEnumerable<VictoryPost> allPosts)
        {
            var postsNotInCurrentYear = allPosts.Where(e => e.PostDate.Year != DateTime.Now.Year);
            var result = new Dictionary<int, int>();

            for(int i = 1; i < 53; i++)
            {
                var postsForWeek = postsNotInCurrentYear.Where(e => e.PostWeekNumber == i);

                double totalVictoryComments = postsForWeek.Sum(e => e.VictoryCommentCount);
                double totalPostsForWeek = postsForWeek.Count() == 0 ? 1.0 : (double)postsForWeek.Count();

                var averageComments = totalVictoryComments / totalPostsForWeek;

                result.Add(i, (int)Math.Round(averageComments));
            }

            return result;
        }
    }
}
