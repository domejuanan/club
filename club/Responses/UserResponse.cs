using club.Resources;

namespace club.Responses
{
    public class UserResponse : BaseResponse<UserResource>
    {
        public UserResponse(UserResource res) : base(res)
        { }

        public UserResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}
