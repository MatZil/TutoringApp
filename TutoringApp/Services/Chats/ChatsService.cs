using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringApp.Data.Dtos.Chats;
using TutoringApp.Data.Models;
using TutoringApp.Infrastructure.Repositories;
using TutoringApp.Services.Interfaces;

namespace TutoringApp.Services.Chats
{
    public class ChatsService : IChatsService
    {
        private readonly IRepository<ChatMessage> _chatMessagesRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<IChatsService> _logger;
        private readonly ITimeService _timeService;

        public ChatsService(
            IRepository<ChatMessage> chatMessagesRepository,
            ICurrentUserService currentUserService,
            UserManager<AppUser> userManager,
            ILogger<IChatsService> logger,
            ITimeService timeService)
        {
            _chatMessagesRepository = chatMessagesRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
            _timeService = timeService;
        }

        public async Task<ChatMessageDto> PostChatMessage(string receiverId, ChatMessageNewDto chatMessageNew)
        {
            await ValidateMessagePosting(receiverId);

            var senderId = _currentUserService.GetUserId();

            var chatMessage = new ChatMessage
            {
                ReceiverId = receiverId,
                SenderId = senderId,
                Timestamp = _timeService.GetCurrentTime(),
                Content = chatMessageNew.Content
            };

            await _chatMessagesRepository.Create(chatMessage);

            var sender = await _userManager.FindByIdAsync(senderId);

            return new ChatMessageDto
            {
                SenderId = chatMessage.SenderId,
                Timestamp = chatMessage.Timestamp,
                SenderName = $"{sender.FirstName} {sender.LastName}",
                Content = chatMessage.Content
            };
        }

        private async Task ValidateMessagePosting(string receiverId)
        {
            var receiver = await _userManager.Users
                .Include(u => u.StudentTutors)
                .Include(u => u.TutorStudents)
                .FirstOrDefaultAsync(u => u.Id == receiverId);

            if (receiver is null)
            {
                var errorMessage = $"Could not send message to user (id = '{receiverId}'): user does not exist";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            var senderId = _currentUserService.GetUserId();
            var isStudent = receiver.StudentTutors.Any(st => st.TutorId == senderId);
            var isTutor = receiver.TutorStudents.Any(st => st.StudentId == senderId);

            if (!isStudent && !isTutor)
            {
                var errorMessage = $"Could not send message to user (id = '{receiverId}'): he is neither your student nor tutor.";
                _logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatMessages(string receiverId)
        {
            var senderId = _currentUserService.GetUserId();
            var chatMessages = await _chatMessagesRepository.GetFiltered(cm => cm.SenderId == senderId && cm.ReceiverId == receiverId);

            var sender = await _userManager.FindByIdAsync(senderId);
            var receiver = await _userManager.FindByIdAsync(receiverId);

            return chatMessages.Select(chatMessage => new ChatMessageDto
            {
                SenderId = chatMessage.SenderId,
                Timestamp = chatMessage.Timestamp,
                Content = chatMessage.Content,
                SenderName = chatMessage.SenderId == senderId
                    ? $"{sender.FirstName} {sender.LastName}"
                    : $"{receiver.FirstName} {receiver.LastName}"
            });
        }
    }
}
