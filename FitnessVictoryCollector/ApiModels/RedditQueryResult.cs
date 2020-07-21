using Newtonsoft.Json;
using System.Collections.Generic;

namespace FitnessVictoryCollector.ApiModels
{
    public class PostQueryResult
    {
        [JsonProperty("data")]
        public QueryData Data { get; set; }
    }

    public class QueryData
    {
        [JsonProperty("children")]
        public IEnumerable<PostListing> Listings { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }

    public class PostListing
    {
        [JsonProperty("data")]
        public PostListingData Data { get; set; }
    }

    public class PostListingData
    {
        [JsonProperty("created_utc")]
        public long CreatedUtc { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    //---


    public class CommentListing
    {
        [JsonProperty("data")]
        public CommentListingData Data { get; set; }
    }

    public class CommentListingData
    {
        [JsonProperty("children")]
        public IEnumerable<CommentListingChild> Children { get; set; }
    }

    public class CommentListingChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
    }
}
