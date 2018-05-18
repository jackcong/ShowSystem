namespace ComLib.Exceptions
{
    public class Http404Exception : System.Exception
    {
        private readonly string _message="HTTP 404 exception.";

        public override string Message
        {
            get { return _message; }
        }

        public Http404Exception()
        {}

        public Http404Exception(string message)
        {
            _message = message;
        }
    }
}
