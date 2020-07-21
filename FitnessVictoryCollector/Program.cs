using System;
using System.Threading.Tasks;

namespace FitnessVictoryCollector
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var postReader = new PostReader();
                var postProcessor = new PostProcessor();
                var postWriter = new PostWriter("C:/tmp");

                var victoryPosts = await postReader.GetVictoryPosts(new DateTime(2015, 1, 1));

                var currYearScores = postProcessor.GetVictoryPostsForCurrentYear(victoryPosts);
                var pastYearScores = postProcessor.GetAverageVictoryCountForPreviousYears(victoryPosts);

                postWriter.SaveAggregatePosts(currYearScores, pastYearScores);
                postWriter.SaveAllPosts(victoryPosts);
            }
            catch(Exception ex)
            {
                Console.Write($"Error in Main: {ex.Message}");
                throw;
            }
        }
    }
}
