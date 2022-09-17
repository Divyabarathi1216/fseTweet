using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TweetApp_Models
{
    public class ReplyTweet
    {
        [Key]
        public int Id { get; set; }

        public string replyTweet { get; set; }

        public string tag { get; set; }

        public int tweetId { get; set; }

        public int userId { get; set; }

        public DateTime replyTweetCreatedDate { get; set; }

        [NotMapped]
        public User user { get; set; }
    }
}
