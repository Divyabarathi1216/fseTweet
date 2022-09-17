using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TweetApp_Models;

namespace TweetApp_Services.IRepository
{
    public interface IReplyTweetRepo
    {
        Task<List<ReplyTweet>> ViewReplyTweets(int tweetId);
        Task<ReplyTweet> AddReplyTweet(ReplyTweet replyTweet);
    }
}
