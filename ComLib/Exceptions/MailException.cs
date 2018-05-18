namespace ComLib.Exceptions
{
    public class MailSenderCreationException: System.Exception
    {
        private readonly string _message="Mail sender creation failed.";

        public override string Message
        {
            get { return _message; }
        }

        public MailSenderCreationException()
        {}
        
        public MailSenderCreationException(string message)
        {
            _message = message;
        }
    }
}
