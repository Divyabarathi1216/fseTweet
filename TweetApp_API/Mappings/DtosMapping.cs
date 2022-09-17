using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TweetApp_API.Dtos;
using TweetApp_Models;

namespace TweetApp_API
{
    public class DtosMapping : Profile
    {
        public DtosMapping()
        {
            CreateMap<User, UserDisplayDto>().ReverseMap();
            CreateMap<User, UserPostDto>().ReverseMap();
            CreateMap<Tweet, TweetDisplayDto>().ReverseMap();
            CreateMap<Tweet, TweetPostDto>().ReverseMap();
            CreateMap<ReplyTweet, ReplyTweetDisplayDto>().ReverseMap();
            CreateMap<ReplyTweet, ReplyTweetPostDto>().ReverseMap();
        }
    }
}
