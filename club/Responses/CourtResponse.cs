using club.Resources;

namespace club.Responses
{
    public class CourtResponse : BaseResponse<CourtResource>
    {        
        public CourtResponse(CourtResource resource) : base(resource)
        { }
                
        public CourtResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}