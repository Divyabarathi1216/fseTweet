using System;
using System.Collections.Generic;
using System.Text;

namespace TweetApp_API
{
    public class TweetDisplayDto
    {
        public int Id { get; set; }

        public int userId { get; set; }

        public UserDisplayDto user { get; set; }

        public string tweet { get; set; }

        public string tag { get; set; }

        public List<ReplyTweetDisplayDto> replyTweets { get; set; }

        public int likeCnt { get; set; } = 0;

        public int dislikeCnt { get; set; } = 0;

        public DateTime tweetCreatedDate { get; set; }
    }
}
