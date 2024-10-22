namespace WebAPI.ViewModels
{
    public class StatusVM<T> where T : class
    {
        public bool Succes { get; private set; } = false;
        public string Message { get; private set; } = "Ok";
        public T? Data { get; private set; } = null;

        public StatusVM() { }

        public StatusVM(T data)
        {
            Succes = true;
            Data = data;
        }

        public StatusVM(string message)
        {
            Message = message;
        }
    }
}
