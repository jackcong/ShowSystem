namespace ComLib.HTTPResultHelpers
{
    public class HttpResponseResult
    {
        public string Status { get; set; }

        public int RowEffected { get; set; }

        public string Message { get; set; }

        public object EffectedObject { get; set; }

        public bool Effected { get; set; }
    }

    public enum HttpStatus
    { 
        Success = 0,
        Fail = 1
    }
}
