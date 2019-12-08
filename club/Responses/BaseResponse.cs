using club.Resources;

namespace club.Responses
{
    public abstract class BaseResponse<T>
    {
        public bool Success { get; private set; }
        public ErrorResource Error { get; private set; }
        public T Result { get; private set; }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="resource">Item.</param>
        /// <returns>Response.</returns>
        protected BaseResponse(T resource)
        {
            Success = true;
            Error = default;
            Result = resource;
        }


        /// <summary>
        /// Creates an error response.
        /// </summary>
        /// <param name="statusCode">Error status.</param>
        /// <param name="errorCode">Error title.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>Response.</returns>
        protected BaseResponse(int statusCode, string errorTitle, string errorKey, string errorMessage)
        {
            Success = false;
            Error = new ErrorResource(statusCode, errorTitle, errorKey, errorMessage);
            Result = default;

        }
    }
}