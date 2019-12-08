using club.Resources;

namespace club.Responses
{
    public class MemberListResponse : BaseListResponse<MemberListResource>
    {        
        public MemberListResponse(MemberListResource resource) : base(resource)
        { }
                
        public MemberListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}