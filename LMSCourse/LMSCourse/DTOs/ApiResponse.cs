namespace LMSCourse.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ApiResponse<T> Ok(T? data, string message = "")
            => new ApiResponse<T> { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message)
            => new ApiResponse<T> { Success = false, Message = message };
    }
    public class ApiResponse : ApiResponse<object>
    {
        public new static ApiResponse Ok(object? data = null, string message = "")
            => new ApiResponse { Success = true, Data = data, Message = message };

        public new static ApiResponse Fail(string message)
            => new ApiResponse { Success = false, Message = message };
    }
}
