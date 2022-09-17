using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TweetApp_Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailId { get; set; }

        public long contactNo { get; set; }

        public string loginId { get; set; }

        public string password { get; set; }

        public string Picture { get; set; }

        public DateTime userCreatedDate { get; set; }

        public DateTime userModifiedDate { get; set; }

        public DateTime userLastSeen { get; set; }

        public bool isActive { get; set; }

        [NotMapped]
        public string confirmPwd { get; set; }
    }
}
