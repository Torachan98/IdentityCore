namespace IdentityCore.EFs.Requests
{
    public class FriendlyException: Exception
    {
        public string Code { get; private set; } = string.Empty;

        public FriendlyException() 
        {

        }

        public FriendlyException(int code, string message) : base(message)
        {
            Code = code.ToString();
        }

        public FriendlyException(int code, Exception innerException) : base(code.ToString(), innerException)
        {
            Code = code.ToString();
        }
    }
}
