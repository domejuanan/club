using club.Resources;

namespace club.Responses
{
    public class SportListResponse : BaseResponse<SportListResource>
    {      
        public SportListResponse(SportListResource resource) : base(resource)
        { }

        public SportListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}