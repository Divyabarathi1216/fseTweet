using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweetApp_API.Dtos;
using TweetApp_Models;
using TweetApp_Services.IRepository;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace TweetApp_API.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IUserRepo userRepo;
        private readonly ITweetRepo tweetRepo;
        private readonly IReplyTweetRepo replyTweetRepo;
        private readonly IMapper mapper;
        private ILogger<TweetsController> logger;
        ResponseDto responseDto;
        private readonly AppSettings _appSettings;

       
        public TweetsController(IUserRepo userRepo, ITweetRepo tweetRepo, IMapper mapper, ILogger<TweetsController> _logger,IReplyTweetRepo _replyTweetRepo, IOptions<AppSettings> appsettings)
        {
            this.userRepo = userRepo;
            this.tweetRepo = tweetRepo;
            replyTweetRepo = _replyTweetRepo;
            this.mapper = mapper;
            logger = _logger;
            responseDto = new ResponseDto();
            _appSettings = appsettings.Value;
        }

        ProducerConfig config = new ProducerConfig { BootstrapServers = "localhost:9092" };

        //************************************* User functions *************************************//

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>returns all users</returns>
        [Authorize]
        [HttpGet]
        [Route("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
          //  var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var userList= await userRepo.ViewAllUsers();
            var userDto = new List<UserDisplayDto>();
            foreach(var obj in userList)
            {
                userDto.Add(mapper.Map<UserDisplayDto>(obj));
            }
             
            if (userList != null && userList.Count>0)
            {
                responseDto.result = userDto;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Users retrieved successfully.";
                logger.LogInformation("View all users successfully executed.");
                var response = await producer.ProduceAsync("tweetmessages",
                    new Message<Null, string> { Value = "Return all users" });
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
                logger.LogError("Error occurred while registering user");
            }
            return Ok(responseDto);
        }



        /// <summary>
        /// Get user by login id
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns>response</returns>
        [Authorize]
        [HttpGet]
        [Route("users/search/{loginId}")]
        public async Task<IActionResult> GetUserByLoginID(string loginId)
        {
            var user = await userRepo.ViewUser(loginId);
            var userObj = mapper.Map<UserDisplayDto>(user);
            if (userObj != null && userObj.Id>0)
            {
                responseDto.result = userObj;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "User retrieved successfully.";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }



        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <returns>response</returns>
        [HttpGet]
        [Route("users/login/{loginId},{password}")]
        public async Task<IActionResult> AuthenticateUser(string loginId, string password)
        {
            bool IsValidUser = await userRepo.Authenticate(loginId, password);
            AuthDto authDto = new AuthDto();
            if (IsValidUser)
            {
                authDto.token= GenerateToken(loginId);
                authDto.isValidUser = true;
                responseDto.result = authDto;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Valid user";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Invalid user";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// Forgot password
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="newPassword"></param>
        /// <returns>response</returns>
        [HttpPatch]
        [Route("users/forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPwdDto forgotPwdDto)
        {
            bool isSuccess = await userRepo.ForgotPassword(forgotPwdDto.userId, forgotPwdDto.password);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "New password created";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }



        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>response</returns>
       
        [HttpPost]
        [Route("users/register")]
        public async Task<IActionResult> CreateUser([FromBody] UserPostDto userDto)
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var newUser = mapper.Map<User>(userDto);
            bool isSuccess = await userRepo.CreateNewUser(newUser);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "New user registered successfully";
                var response = await producer.ProduceAsync("TestTopic",
                    new Message<Null, string> { Value = "New user registered" });
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("users/update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDisplayDto userDto)
        {
            var newUser = mapper.Map<User>(userDto);
            var updatedUser = await userRepo.EditUser(newUser);
            if (updatedUser!=null && updatedUser.Id>0)
            {
                responseDto.result = updatedUser;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "User updated successfully";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }



        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("users/delete/{loginId}")]
        public async Task<IActionResult> DeleteUser(string loginId)
        {
            
            bool isSuccess = await userRepo.DeleteUser(loginId);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "User deleted successfully";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }

        /// <summary>
        /// Is unique user by login Id
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/Isuniqueid/{loginId}")]
        public async Task<IActionResult> IsUniqueUserByLoginId(string loginId)
        {

            bool isSuccess = await userRepo.IsUniquesUserByLoginId(loginId);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Login Id is unique";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }

        /// <summary>
        /// Is unique user by email
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/Isuniqueemail/{emailId}")]
        public async Task<IActionResult> IsUniqueUserByEmail(string emailId)
        {

            bool isSuccess = await userRepo.IsUniqueUserByEmail(emailId);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Email Id is unique";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }


        //************************************* Tweet functions *************************************//

        /// <summary>
        /// Get all tweets
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllTweets()
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var tweetList = await tweetRepo.ViewAllTweets();
            var tweetListDto = new List<TweetDisplayDto>();
            if (tweetList != null && tweetList.Count()>0)
            {
                foreach(var tweet in tweetList)
                {
                    tweetListDto.Add(mapper.Map<TweetDisplayDto>(tweet));
                }
                
                responseDto.result = tweetListDto;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Tweets retrieved successfully.";
                var response = await producer.ProduceAsync("TestTopic",
                    new Message<Null, string> { Value = "Return all tweets" });
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// Get tweets by user
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("{loginId}")]
        public async Task<IActionResult> GetTweetsByUser(string loginId)
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var tweetsByUser = await tweetRepo.ViewTweetByUser(loginId);
            var tweetListDto = new List<TweetDisplayDto>();
            if (tweetsByUser != null && tweetsByUser.Count()>0)
            {
                foreach (var tweet in tweetsByUser)
                {
                    tweetListDto.Add(mapper.Map<TweetDisplayDto>(tweet));
                }        
                responseDto.result = tweetListDto;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Tweets retrieved successfully.";
                var response = await producer.ProduceAsync("TestTopic",
                    new Message<Null, string> { Value = "Return tweets by user" });
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// Create tweet
        /// </summary>
        /// <param name="tweetDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> CreateTweet([FromBody] TweetPostDto tweetDto)
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var tweet = mapper.Map<Tweet>(tweetDto);
            bool isSuccess = await tweetRepo.PostTweet(tweet);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Tweet created";
                var response = await producer.ProduceAsync("TestTopic",
                    new Message<Null, string> { Value = "New tweet created" });
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// Update tweet
        /// </summary>
        /// <param name="tweetDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateTweet([FromBody] TweetDisplayDto tweetDto)
        {
            var tweet = mapper.Map<Tweet>(tweetDto);
            var updateTweet = await tweetRepo.UpdateTweet(tweet);
            if (updateTweet!=null && updateTweet.Id>0)
            {
                var updateTweetDto = mapper.Map<TweetDisplayDto>(updateTweet);
                responseDto.result = updateTweetDto;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Tweet updated.";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// Remove tweet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTweet(int id)
        {
            bool isSuccess = await tweetRepo.RemoveTweet(id);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Tweet removed.";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }

        /// <summary>
        /// Like a tweet
        /// </summary>
        /// <param name="tweetId"></param>
        /// <param name="isLike"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("like")]
        public async Task<IActionResult> LikeTweet([FromBody] LikeTweetDto likeTweetDto)
        {
            bool isSuccess = await tweetRepo.LikeTweet(likeTweetDto.tweetId, likeTweetDto.isLike);
            if (isSuccess)
            {
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Tweet liked";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }

        //************************************* Reply Tweet functions *************************************//

        /// <summary>
        /// Post a reply tweet
        /// </summary>
        /// <param name="replyTweetPostDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("reply")]
        public async Task<IActionResult> AddReplyTweet(ReplyTweetPostDto replyTweetPostDto)
        {
            var replyTweet = mapper.Map<ReplyTweet>(replyTweetPostDto);
            var replyTweetRes = await replyTweetRepo.AddReplyTweet(replyTweet);
            if (replyTweetRes!=null)
            {
                responseDto.result = replyTweetRes;
                responseDto.isSuccess = true;
                responseDto.displayMessage = "Reply tweet created";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }


        /// <summary>
        /// View reply tweets
        /// </summary>
        /// <param name="replyTweetPost"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewReplyTweets(int tweetId)
        {
            var replyTweets = await replyTweetRepo.ViewReplyTweets(tweetId);
            var replyTweetsDisplyDto = new List<ReplyTweetDisplayDto>();

            if (replyTweets != null)
            {
                foreach(var obj in replyTweets)
                {
                    replyTweetsDisplyDto.Add(mapper.Map<ReplyTweetDisplayDto>(obj));
                }
                responseDto.isSuccess = true;
                responseDto.result = replyTweetsDisplyDto;
                responseDto.displayMessage = "Reply tweets retrieved successfully.";
            }
            else
            {
                responseDto.isSuccess = false;
                responseDto.errorMessage = "Something went wrong.";
            }
            return Ok(responseDto);
        }


        //Token generator
        private string GenerateToken(string loginId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, loginId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            
        }
    }
}
