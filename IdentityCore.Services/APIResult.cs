namespace IdentityCore.Services
{
    public class ObjectResult<T> where T : class
    {
        public string Message { get; set; } = string.Empty;
        public T Item { get; set; } = null;
    }

    public class ArrayResult<T> where T : class
    {
        public string Message { get; set; } = string.Empty;
        public List<T> Items { get; set; } = [];
    }
}
