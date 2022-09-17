using System;
using System.Collections.Generic;
using System.Text;

namespace TweetApp_API
{
    public class ResponseDto
    {
        public bool isSuccess { get; set; }
        public string displayMessage { get; set; }
        public string errorMessage { get; set; }
        public object result { get; set; }
    }
}
