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

        public StatusVM(T data, string message)
        {
            Succes = true;
            Message = message;
            Data = data;
        }

        public StatusVM(string message)
        {
            Succes = false;
            Message = message;
        }
    }
}
