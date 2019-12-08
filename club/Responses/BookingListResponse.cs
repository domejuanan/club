using club.Resources;

namespace club.Responses
{
    public class BookingListResponse : BaseResponse<BookingListResource>
    {      
        public BookingListResponse(BookingListResource resource) : base(resource)
        { }

        public BookingListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}