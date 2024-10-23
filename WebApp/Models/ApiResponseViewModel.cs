namespace WebApp.Models
{
    public class ApiResponseViewModel<T> where T : class
    {
        public bool Succes { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
}
