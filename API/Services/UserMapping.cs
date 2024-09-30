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
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                City = user.City,
                Country = user.Country,
                Zip = user.Zip,
                IsAdmin = user.IsAdmin,
            };
            return userGetDTO;
        }
    }
}
