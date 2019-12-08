using club.Resources;

namespace club.Responses
{
    public abstract class BaseListResponse<T>
    {
        public bool Success { get; private set; }
        public ErrorResource Error { get; private set; }
        public T Result { get; set; }

        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="results">QueryResultResource.</param>
        /// <returns>Response.</returns>
        protected BaseListResponse(T result)
        {
            Success = true;
            Error = default;            
            Result = result;
        }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="statusCode">Error status.</param>
        /// <param name="errorCode">Error title.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>Response.</returns>
        protected BaseListResponse(int statusCode, string errorTitle, string errorKey, string errorMessage)
        {
            Success = false;
            Error = new ErrorResource(statusCode, errorTitle, errorKey, errorMessage);
            Result = default;

        }
    }
}