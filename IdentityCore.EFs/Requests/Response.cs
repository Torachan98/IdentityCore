namespace IdentityCore.EFs.Requests
{
    public class ObjectResponse<T> : Response where T : class
    {
        public T? Item { get; set; }
    }

    public class ArrayResponse<T> : Response where T : class
    {
        public IList<T>? Items { get; set; }
    }

    public class Response
    {
        public string? Message { get; set; } = string.Empty;
    }
}
