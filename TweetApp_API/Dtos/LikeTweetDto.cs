using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp_API.Dtos
{
    public class LikeTweetDto
    {
        public int tweetId { get; set; }
        public bool isLike { get; set; }
    }
}
