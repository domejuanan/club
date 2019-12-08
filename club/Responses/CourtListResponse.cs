using club.Resources;

namespace club.Responses
{
    public class CourtListResponse : BaseResponse<CourtListResource>
    {      
        public CourtListResponse(CourtListResource resource) : base(resource)
        { }

        public CourtListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}