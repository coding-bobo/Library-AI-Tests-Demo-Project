namespace LibrarySystem.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public object? Data { get; }

        private Result(bool isSuccess, string message, object? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static Result Success(string message = "Operation completed successfully", object? data = null)
        {
            return new Result(true, message, data);
        }

        public static Result Failure(string message)
        {
            return new Result(false, message);
        }
    }
}
