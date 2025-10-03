using Newtonsoft.Json;

namespace UserStoryReader.Models
{
    public class UserStory
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("acceptanceCriteria")]
        public List<string> AcceptanceCriteria { get; set; } = new List<string>();

        [JsonProperty("priority")]
        public string Priority { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("assignee")]
        public string Assignee { get; set; } = string.Empty;

        [JsonProperty("estimatedHours")]
        public int EstimatedHours { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonProperty("epic")]
        public string Epic { get; set; } = string.Empty;

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Title} - {Status} ({Priority})";
        }
    }
}
