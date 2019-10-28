using MyXSpace.Core.Entities;
using MyXSpace.Core.Interfaces;
using System.Linq;

namespace MyXSpace.Core.Services
{
    public class UserProfileManager : DomainService, IUserProfileManager
    {
        private readonly IRepository<UserProfile, string> _userProfileRepository;

        public UserProfileManager(IRepository<UserProfile, string> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public UserProfile GetUserProfile(string id)
        {
            return _userProfileRepository.Get(id);
        }

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserProfile GetUserProfileByUserId(string id)
        {
            return _userProfileRepository.GetAll().FirstOrDefault(u => u.UserId == id);
        }

        /// <summary>
        /// Create or update user profile
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public void SaveUserProfile(UserProfile profile)
        {
            //TODO: check if exist user (IserId > 0) and UserProfile (UserProfile.Id >0)
            //if not - create _userProfileRepository.Add(profile)

            _userProfileRepository.Update(profile);
        }
    }
}
