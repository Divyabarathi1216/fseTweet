using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp_API.Dtos
{
    public class AuthDto
    {
        public bool isValidUser { get; set; }
        public string token { get; set; }
    }
}
