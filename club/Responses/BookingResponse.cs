using club.Resources;

namespace club.Responses
{
    public class BookingResponse : BaseResponse<BookingResource>
    {        
        public BookingResponse(BookingResource bookingResource) : base(bookingResource)
        { }
                
        public BookingResponse(int statusCode, string errorTitle, string errorKey, string errorMessage) : base(statusCode, errorTitle, errorKey, errorMessage)
        { }
    }
}