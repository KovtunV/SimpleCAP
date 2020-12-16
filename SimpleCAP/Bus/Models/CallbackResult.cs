namespace SimpleCAP.Bus.Models
{
    /// <summary>
    /// Callback result
    /// </summary>
    public class CallbackResult<TResponse>
    {
        /// <summary>
        /// Response model
        /// </summary>
        public TResponse Response { get; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Ok if <see cref="ErrorMessage"/> doesn't exist
        /// </summary>
        public bool IsOk => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Callback result
        /// </summary>
        public CallbackResult(TResponse response)
        {
            Response = response;
        }

        private CallbackResult(TResponse response, string errorMessage) 
            : this(response)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Makes the error instance
        /// </summary>
        public static CallbackResult<TResponse> Error(string errorMessage)
        {
            return new CallbackResult<TResponse>(default, errorMessage);
        }
    }
}
