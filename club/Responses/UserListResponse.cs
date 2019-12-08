using club.Resources;

namespace club.Responses
{
    public class UserListResponse : BaseListResponse<UserListResource>
    {
        public UserListResponse(UserListResource resource) : base(resource)
        { }

        public UserListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}