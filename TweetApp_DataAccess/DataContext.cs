using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TweetApp_Models;

namespace TweetApp_DataAccess
{
    public class DataContext:DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Tweet> tweets { get; set; }
        public DbSet<ReplyTweet> replyTweets { get; set; }
        public DbSet<LikeTweet> likes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
