namespace ComLib.Exceptions
{
    public class UnpriviledgedException : System.Exception
    {
        private readonly string _message="Unpriviledged access.";

        public override string Message
        {
            get { return _message; }
        }

        public UnpriviledgedException()
        {}
        
        public UnpriviledgedException(string message)
        {
            _message = message;
        }
    }
}
