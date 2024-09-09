using API.Models;

namespace API.Services
{
    public class RoomMapping
    {
        public GetRoomDTO MapRoomToGetRoomDTO(Room room)
        {
            GetRoomDTO getRoomDTO = new GetRoomDTO()
            {
                Id = room.Id,
                Price = room.PricePerNight,
                RoomType = room.RoomType,
                Description = room.Description,
                ImageURL = room.ImageURL
            };

            return getRoomDTO;
        }
    }
}
