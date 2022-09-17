using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp_API.Dtos
{
    public class ForgotPwdDto
    {
        public string userId { get; set; }
        public string password { get; set; }
    }
}
