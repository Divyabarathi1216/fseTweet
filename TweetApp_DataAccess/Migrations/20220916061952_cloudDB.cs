using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TweetApp_DataAccess.Migrations
{
    public partial class cloudDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    emailId = table.Column<string>(nullable: true),
                    contactNo = table.Column<long>(nullable: false),
                    loginId = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    userCreatedDate = table.Column<DateTime>(nullable: false),
                    userModifiedDate = table.Column<DateTime>(nullable: false),
                    userLastSeen = table.Column<DateTime>(nullable: false),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(nullable: false),
                    tweetId = table.Column<int>(nullable: false),
                    IsLikeOrDislike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.id);
                    table.ForeignKey(
                        name: "FK_likes_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tweets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(nullable: false),
                    tweet = table.Column<string>(maxLength: 144, nullable: true),
                    tag = table.Column<string>(maxLength: 50, nullable: true),
                    tweetCreatedDate = table.Column<DateTime>(nullable: false),
                    likeCnt = table.Column<int>(nullable: false),
                    dislikeCnt = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tweets_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "replyTweets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    replyTweet = table.Column<string>(nullable: true),
                    tag = table.Column<string>(nullable: true),
                    tweetId = table.Column<int>(nullable: false),
                    userId = table.Column<int>(nullable: false),
                    replyTweetCreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_replyTweets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_replyTweets_tweets_tweetId",
                        column: x => x.tweetId,
                        principalTable: "tweets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_likes_userId",
                table: "likes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_replyTweets_tweetId",
                table: "replyTweets",
                column: "tweetId");

            migrationBuilder.CreateIndex(
                name: "IX_tweets_userId",
                table: "tweets",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "replyTweets");

            migrationBuilder.DropTable(
                name: "tweets");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
