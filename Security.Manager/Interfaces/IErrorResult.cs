namespace Security.Manager.Interfaces
{
    public interface IErrorResult
    {
        string Message { get; set; }
        int StatusCode { get; set; }
        string FullText { get; set; }
    }
}