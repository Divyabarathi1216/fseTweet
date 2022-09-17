using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp_DataAccess;
using TweetApp_Models;
using TweetApp_Services.IRepository;

namespace TweetApp_Services.Repository
{
    public class ReplyTweetRepo : IReplyTweetRepo
    {
        private readonly DataContext _dbContext;

        public ReplyTweetRepo(DataContext dataContext)
        {
            _dbContext = dataContext;
        }

        /// <summary>
        /// Add reply tweet
        /// </summary>
        /// <param name="replyTweet"></param>
        /// <returns></returns>
        public async Task<ReplyTweet> AddReplyTweet(ReplyTweet replyTweet)
        {
           
            try
            {
                replyTweet.replyTweetCreatedDate = DateTime.Now;
                _dbContext.replyTweets.Add(replyTweet);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                replyTweet = new ReplyTweet();
            }

            return replyTweet;
        }

        /// <summary>
        /// View Reply Tweets
        /// </summary>
        /// <param name="tweetId"></param>
        /// <returns></returns>
        public async Task<List<ReplyTweet>> ViewReplyTweets(int tweetId)
        {
            List<ReplyTweet> replyTweets = new List<ReplyTweet>();
            try
            {

                replyTweets =await _dbContext.replyTweets.Where(x => x.tweetId == tweetId).ToListAsync();
                foreach(var obj in replyTweets)
                {
                    //obj.tweet =await _dbContext.tweets.FindAsync(obj.tweetId);
                    obj.user = await _dbContext.users.FindAsync(obj.userId);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return replyTweets;
        }
    }
}
