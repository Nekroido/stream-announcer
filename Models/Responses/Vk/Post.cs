using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Vk
{
    public class Post
    {
        [JsonProperty("id")]
        private int? id { get; set; }

        [JsonProperty("post_id")]
        private int? postId { get; set; }

        public int PostId
        {
            get { return (id ?? postId).Value; }
        }

        [JsonProperty("copy_history")]
        public List<Post> CopyHistory { get; set; }

        [JsonProperty("source_id")]
        private int? sourceId { get; set; }

        [JsonProperty("from_id")]
        public int? fromId { get; set; }

        public int SourceId
        {
            get { return (sourceId ?? fromId).Value; }
        }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("reply_owner_id")]
        public int ReplyOwnerId { get; set; }

        [JsonProperty("reply_post_id ")]
        public int ReplyPostId { get; set; }

        [JsonProperty("friends_only  ")]
        public bool IsFriendsOnly { get; set; }

        [JsonProperty("is_pinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("can_pin")]
        public bool CanPin { get; set; }

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }

        [JsonProperty("created_by", NullValueHandling = NullValueHandling.Ignore)]
        public int? CreatedBy { get; set; }

        [JsonProperty("can_delete")]
        public bool CanDelete { get; set; }

        [JsonProperty("friends_only")]
        public bool FriendsOnly { get; set; }

        [JsonProperty("comments")]
        public Comment Comments { get; set; }

    }
}
