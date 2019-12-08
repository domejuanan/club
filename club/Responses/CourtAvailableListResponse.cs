using club.Resources;

namespace club.Responses
{
    public class CourtAvailableListResponse : BaseResponse<CourtAvailableListResource>
    {      
        public CourtAvailableListResponse(CourtAvailableListResource resource) : base(resource)
        { }

        public CourtAvailableListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}