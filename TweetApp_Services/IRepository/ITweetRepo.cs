using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TweetApp_Models;

namespace TweetApp_Services.IRepository
{
    public interface ITweetRepo
    {
        Task<bool> PostTweet(Tweet tweet);
        Task<bool> RemoveTweet(int id);
        Task<Tweet> UpdateTweet(Tweet tweet);
        Task<List<Tweet>> ViewTweetByUser(string userId);
        Task<List<Tweet>> ViewAllTweets();
        Task<bool> LikeTweet(int tweetId,bool isLike);
    }
}
