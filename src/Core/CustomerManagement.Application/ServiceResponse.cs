namespace CustomerManagement.Application
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ServiceResponse<T> Success(T data)
        {
            return new ServiceResponse<T> { IsSuccess = true, Data = data };
        }

        public static ServiceResponse<T> Fail(List<string> errors)
        {
            return new ServiceResponse<T> { IsSuccess = false, Errors = errors };
        }
    }
}
