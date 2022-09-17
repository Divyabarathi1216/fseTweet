using System;
using System.Collections.Generic;
using System.Text;

namespace TweetApp_API
{
    public class ReplyTweetDisplayDto
    {
        public int Id { get; set; }

        public string replyTweet { get; set; }

        public string tag { get; set; }

        public int tweetId { get; set; }

        public int userId { get; set; }

        public DateTime replyTweetCreatedDate { get; set; }

        public UserDisplayDto user { get; set; }
    }
}
