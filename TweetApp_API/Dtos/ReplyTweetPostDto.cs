using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp_API.Dtos
{
    public class ReplyTweetPostDto
    {
        public string replyTweet { get; set; }

        public string tag { get; set; }

        public int tweetId { get; set; }

        public int userId { get; set; }
    }
}
