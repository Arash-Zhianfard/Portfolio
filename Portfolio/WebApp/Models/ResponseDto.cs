namespace WebApp.Models
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        public Error Error { get; set; }
    }
    public class Error 
    {
        public string  Message { get; set; }
        public string Code { get; set; }
    }
}
