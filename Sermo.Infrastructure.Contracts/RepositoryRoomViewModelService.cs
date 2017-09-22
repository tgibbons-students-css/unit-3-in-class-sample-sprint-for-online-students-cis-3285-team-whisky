using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sermo.Data.Contracts;
using Sermo.Infrastructure.Contracts;
using Sermo.UI.Contracts;

namespace Sermo.UI.Controllers
{
    public class RepositoryRoomViewModelService : IRoomViewModelReader, IRoomViewModelWriter
    {
        public RepositoryRoomViewModelService(IRoomRepository roomRepository, IMessageRepository messageRepository, IViewModelMapper mapper)
        {
            Contract.Requires<ArgumentNullException>(roomRepository != null);
            Contract.Requires<ArgumentNullException>(messageRepository != null);
            Contract.Requires<ArgumentNullException>(mapper != null);

            this.roomRepository = roomRepository;
            this.messageRepository = messageRepository;
            this.mapper = mapper;
        }

        public IEnumerable<RoomViewModel> GetAllRooms()
        // Changes for Sprint 1 -- I want to view a list of rooms that represent conversations -- Andre Burgoyne
        {
            var allRooms = new List<RoomViewModel>();
            var allRoomRecords = roomRepository.GetAllRooms();
            foreach(var roomRecord in allRoomRecords)
            {
                allRooms.Add(mapper.MapRoomRecordToRoomViewModel(roomRecord));
            }
            return allRooms;
        }

        public IEnumerable<MessageViewModel> GetRoomMessages(int roomID)
        // Changes for Sprint 1 -- I want to view the messages that have been sent to a room -- Andre Burgoyne
        {
            var roomMessages = new List<MessageViewModel>();
            var roomMessageRecords = messageRepository.GetMessagesForRoomID(roomID);
            foreach(var messageRecord in roomMessageRecords)
            {
                roomMessages.Add(mapper.MapMessageRecordToMessageViewModel(messageRecord));
            }
            return roomMessages;
        }

        public void CreateRoom(RoomViewModel roomViewModel)
        {
            var roomRecord = mapper.MapRoomViewModelToRoomRecord(roomViewModel);
            roomRepository.CreateRoom(roomRecord.Name);
        }

        public void AddMessage(MessageViewModel messageViewModel)
        {
            var messageRecord = mapper.MapMessageViewModelToMessageRecord(messageViewModel);
            messageRepository.AddMessageToRoom(messageRecord.RoomID, messageRecord.AuthorName, messageRecord.Text);
        }

        private readonly IRoomRepository roomRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IViewModelMapper mapper;
    }
}
