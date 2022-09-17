using System;
using System.Collections.Generic;
using System.Text;

namespace TweetApp_API
{
    public class UserDisplayDto
    {
        public int Id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailId { get; set; }

        public long contactNo { get; set; }

        public string loginId { get; set; }

        public string password { get; set; }

        public string Picture { get; set; }

        public DateTime userLastSeen { get; set; }

        public bool isActive { get; set; }

    }
}
