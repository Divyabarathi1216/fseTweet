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
    public class TweetRepo : ITweetRepo
    {
        private readonly DataContext _dbContext;

        public TweetRepo(DataContext dataContext)
        {
            _dbContext = dataContext;
        }


        /// <summary>
        /// Like tweet
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LikeTweet(int tweetId,bool isLike)
        {
            bool res = false;
            try
            {
                Tweet tweet = new Tweet();
                tweet = _dbContext.tweets.Find(tweetId);
                if (isLike == true)
                {
                    tweet.likeCnt += 1;
                }
                else
                {
                    tweet.dislikeCnt += 1;
                }
                await _dbContext.SaveChangesAsync();

                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// Post a tweet
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        public async Task<bool> PostTweet(Tweet tweet)
        {
            bool res = false;
            try
            {
                tweet.tweetCreatedDate = DateTime.Now;
                _dbContext.tweets.Add(tweet);
                await _dbContext.SaveChangesAsync();

                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// Remove tweet
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        public async Task<bool> RemoveTweet(int id)
        {
            bool res = false;
            try
            {
                Tweet tweet = new Tweet();
                tweet = _dbContext.tweets.Find(id);
                _dbContext.tweets.Remove(tweet);
                await _dbContext.SaveChangesAsync();
                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// Update tweet
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        public async Task<Tweet> UpdateTweet(Tweet tweet)
        {
            Tweet tweetUpdate = new Tweet();
            try
            {
                _dbContext.tweets.Update(tweet);
                //tweetUpdate = _dbContext.tweets.Find(tweet.Id);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tweet;
        }

        /// <summary>
        /// View all the tweets
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tweet>> ViewAllTweets()
        {
            List<Tweet> tweetList = new List<Tweet>();
            try
            {
                tweetList = await _dbContext.tweets.ToListAsync();
                foreach(var obj in tweetList)
                {
                    obj.user=await _dbContext.users.FindAsync(obj.userId);
          
                    var replyTweets = await _dbContext.replyTweets.Where(x => x.tweetId == obj.Id).ToListAsync();
                    obj.replyTweets = replyTweets;
                    foreach (var item in obj.replyTweets)
                    {
                        var repliedUser = await _dbContext.users.FindAsync(item.userId);
                        item.user = repliedUser;
                    }
        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tweetList;
        }

        /// <summary>
        /// View tweet by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Tweet>> ViewTweetByUser(string userId)
        {
            List<Tweet> tweets = new List<Tweet>();
            try
            {
                var user = await _dbContext.users.FirstOrDefaultAsync(x => x.loginId == userId);
                tweets = await _dbContext.tweets.Where(x => x.userId == user.Id).ToListAsync();
                foreach(var Obj in tweets)
                {
                    Obj.user = user;
                    var replyTweets = await _dbContext.replyTweets.Where(x => x.tweetId == Obj.Id).ToListAsync();
                    Obj.replyTweets = replyTweets;
                    foreach(var item in Obj.replyTweets)
                    {
                        var repliedUser = await _dbContext.users.FindAsync(item.userId);
                        item.user = repliedUser;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tweets;
        }
    }
}
