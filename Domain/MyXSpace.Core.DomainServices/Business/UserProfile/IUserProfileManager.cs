using MyXSpace.Core.Entities;

namespace MyXSpace.Core.Services
{
    public interface IUserProfileManager //: IDomainService
    {
        /// <summary>
        /// Get UserProfile by id
        /// </summary>
        /// <param name="id">UserProfile id</param>
        /// <returns></returns>
        UserProfile GetUserProfile(string id);

        /// <summary>
        /// Get profile for User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserProfile GetUserProfileByUserId(string id);
    }
}
