using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data;
using TutoringApp.Data.Dtos.Chats;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories.ModelRepositories;
using TutoringApp.Services.Chats;
using TutoringApp.Services.Interfaces;
using TutoringApp.Services.Shared;
using TutoringAppTests.Setup;
using Xunit;

namespace TutoringAppTests.UnitTests.Chats
{
    public class ChatsServiceTests
    {
        private readonly IChatsService _chatsService;
        private readonly UserManager<AppUser> _userManager;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ApplicationDbContext _context;

        public ChatsServiceTests()
        {
            var setup = new UnitTestSetup();

            _userManager = setup.UserManager;
            _context = setup.Context;

            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(setup.UserManager.Users.First(u => u.Email == "matas.zilinskas@ktu.edu").Id);

            _chatsService = new ChatsService(
                new ChatMessagesRepository(setup.Context),
                _currentUserServiceMock.Object,
                setup.UserManager,
                new TimeService(),
                new Mock<IHubsService>().Object
                );
        }

        [Fact]
        public async Task When_GettingChatMessages_Expect_CorrectMessages()
        {
            var receiver = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");

            var actualChatMessages = await _chatsService.GetChatMessages(receiver.Id, 1);

            Assert.Collection(actualChatMessages,
                chatMessage =>
                {
                    Assert.Equal("Hello", chatMessage.Content);
                    Assert.Equal(receiver.Id, chatMessage.SenderId);
                    Assert.Equal("Matas FirstTutor", chatMessage.SenderName);
                },
                cm => Assert.Equal("Timing is everything :)", cm.Content),
                cm => Assert.Equal("World", cm.Content)
                );
        }

        [Theory]
        [InlineData("Doesn't exist")]
        public async Task When_GettingChatMessagesFromNonExistingUser_Expect_Exception(string userId)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _chatsService.GetChatMessages(userId, 1)
            );
        }

        [Theory]
        [InlineData("Doesn't exist")]
        public async Task When_GettingChatMessagesFromNonExistingModule_Expect_Exception(string userId)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _chatsService.GetChatMessages(userId, 99)
            );
        }

        [Fact]
        public async Task When_PostingChatMessageAsStudent_Expect_MessagePosted()
        {
            var receiver = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");
            var sender = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");

            var chatMessageNewDto = new ChatMessageNewDto { Content = "Testing...", ModuleId = 1};
            await _chatsService.PostChatMessage(receiver.Id, chatMessageNewDto);

            var chatMessagePosted = await _context.ChatMessages.FirstAsync(cm => cm.Content == "Testing...");
            Assert.Equal(sender.Id, chatMessagePosted.SenderId);
            Assert.Equal(receiver.Id, chatMessagePosted.ReceiverId);
        }

        [Fact]
        public async Task When_PostingChatMessageAsTutor_Expect_MessagePosted()
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius1@ktu.edu").Id);

            var sender = await _userManager.FindByEmailAsync("matas.tutorius1@ktu.edu");
            var receiver = await _userManager.FindByEmailAsync("matas.zilinskas@ktu.edu");

            var chatMessageNewDto = new ChatMessageNewDto { Content = "Testing...", ModuleId = 1};
            await _chatsService.PostChatMessage(receiver.Id, chatMessageNewDto);

            var chatMessagePosted = await _context.ChatMessages.FirstAsync(cm => cm.Content == "Testing...");
            Assert.Equal(sender.Id, chatMessagePosted.SenderId);
            Assert.Equal(receiver.Id, chatMessagePosted.ReceiverId);
        }

        [Theory]
        [InlineData("matas.zilinskas@ktu.edu")]
        [InlineData("any")]
        public async Task When_PostingChatMessageToRandomUser_Expect_Exception(string receiverEmail)
        {
            _currentUserServiceMock
                .Setup(s => s.GetUserId())
                .Returns(_userManager.Users.First(u => u.Email == "matas.tutorius2@ktu.edu").Id);

            var receiver = await _userManager.FindByEmailAsync(receiverEmail);
            var chatMessageNewDto = new ChatMessageNewDto { Content = "Testing..." };

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                    await _chatsService.PostChatMessage(receiver?.Id, chatMessageNewDto)
            );
        }
    }
}
