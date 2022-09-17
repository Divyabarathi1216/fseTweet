using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp_API.Dtos
{
    public class UserPostDto
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailId { get; set; }

        public long contactNo { get; set; }

        public string loginId { get; set; }

        public string password { get; set; }

        public string Picture { get; set; }
    }
}
