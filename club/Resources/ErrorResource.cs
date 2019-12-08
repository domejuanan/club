using System.Collections.Generic;

namespace club.Resources
{
    public class ErrorResource
    {
        public Dictionary<string, string> Errors { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        
        public ErrorResource(int statusCode, string errorTitle, string errorKey, string errorMessage)
        {
            Errors = new Dictionary<string, string>()
            {
                [errorKey] = errorMessage
            };            
            Title = errorTitle;
            Status = statusCode;
        }




    }
}