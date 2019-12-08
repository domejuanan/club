using club.Resources;

namespace club.Responses
{
    public class MemberResponse : BaseResponse<MemberResource>
    {
        public MemberResponse(MemberResource resource) : base(resource)
        { }
                
        public MemberResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}