using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TweetApp_Models
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; }

        public int userId { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }

        [MaxLength(144)]
        public string tweet { get; set; }

        [MaxLength(50)]
        public string tag { get; set; }

        public DateTime tweetCreatedDate { get; set; }

        public List<ReplyTweet> replyTweets { get; set; }

        public int likeCnt { get; set; } = 0;

        public int dislikeCnt { get; set; } = 0; 
    }
}
