using club.Resources;

namespace club.Responses
{
    public class SportResponse : BaseResponse<SportResource>
    {        
        public SportResponse(SportResource sportRes) : base(sportRes)
        { }
                
        public SportResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}