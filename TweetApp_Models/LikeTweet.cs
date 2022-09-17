using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TweetApp_Models
{
    public class LikeTweet
    {
        [Key]
        public int id { get; set; }

        public int userId { get; set; }

        public User user { get; set; }

        public int tweetId { get; set; }
        
        public bool IsLikeOrDislike { get; set; }
    }
}
