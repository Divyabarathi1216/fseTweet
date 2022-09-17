using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp_API.Dtos
{
    public class TweetPostDto
    {
        public int userId { get; set; }

        public string tweet { get; set; }

        public string tag { get; set; }
    }
}
