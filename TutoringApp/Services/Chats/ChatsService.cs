using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly ITimeService _timeService;
        private readonly IHubsService _hubsService;

        public ChatsService(
            IRepository<ChatMessage> chatMessagesRepository,
            ICurrentUserService currentUserService,
            UserManager<AppUser> userManager,
            ITimeService timeService,
            IHubsService hubsService)
        {
            _chatMessagesRepository = chatMessagesRepository;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _timeService = timeService;
            _hubsService = hubsService;
        }

        public async Task PostChatMessage(string receiverId, ChatMessageNewDto chatMessageNew)
        {
            await ValidateMessagePosting(receiverId, chatMessageNew);

            var senderId = _currentUserService.GetUserId();

            var chatMessage = new ChatMessage
            {
                ReceiverId = receiverId,
                SenderId = senderId,
                Timestamp = _timeService.GetCurrentTime(),
                Content = chatMessageNew.Content,
                ModuleId = chatMessageNew.ModuleId
            };

            await _chatMessagesRepository.Create(chatMessage);
            var sender = await _userManager.FindByIdAsync(senderId);
            var chatMessageDto = new ChatMessageDto
            {
                SenderId = chatMessage.SenderId,
                Timestamp = chatMessage.Timestamp,
                SenderName = $"{sender.FirstName} {sender.LastName}",
                Content = chatMessage.Content
            };

            await _hubsService.SendChatNotificationToUser(senderId, chatMessageDto);
            await _hubsService.SendChatNotificationToUser(receiverId, chatMessageDto);
        }

        private async Task ValidateMessagePosting(string receiverId, ChatMessageNewDto chatMessageNew)
        {
            var receiver = await _userManager.Users
                .Include(u => u.StudentTutors)
                .Include(u => u.TutorStudents)
                .FirstOrDefaultAsync(u => u.Id == receiverId);

            if (receiver is null)
            {
                throw new InvalidOperationException($"Could not send message to user (id = '{receiverId}'): user does not exist");
            }

            var senderId = _currentUserService.GetUserId();
            var isStudent = receiver.StudentTutors.Any(st => st.TutorId == senderId && st.ModuleId == chatMessageNew.ModuleId);
            var isTutor = receiver.TutorStudents.Any(st => st.StudentId == senderId && st.ModuleId == chatMessageNew.ModuleId);

            if (!isStudent && !isTutor)
            {
                throw new InvalidOperationException($"Could not send message to user (id = '{receiverId}'): user does not exist");
            }
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatMessages(string receiverId, int moduleId)
        {
            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver is null)
            {
                throw new InvalidOperationException($"Could not get messages from user (id = '{receiverId}'): he does not exist.");
            }

            var senderId = _currentUserService.GetUserId();
            var chatMessages = await _chatMessagesRepository
                .GetFiltered(cm =>
                    cm.ModuleId == moduleId &&
                    (cm.SenderId == senderId && cm.ReceiverId == receiverId
                     || cm.ReceiverId == senderId && cm.SenderId == receiverId));
            chatMessages = chatMessages.OrderBy(cm => cm.Timestamp);

            var sender = await _userManager.FindByIdAsync(senderId);

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
