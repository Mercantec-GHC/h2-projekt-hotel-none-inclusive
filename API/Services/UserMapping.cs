using API.Models;

namespace API.Services
{
    public class UserMapping
    {
        public UserGetDTO MapUserToUserGetDTO(User user)
        {
            UserGetDTO userGetDTO = new UserGetDTO()
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName
            };


            return userGetDTO;
        }
    }
}
