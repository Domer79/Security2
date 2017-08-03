using Security.Manager.Interfaces;

namespace Security.Manager.Models
{
    public class ErrorResult : IErrorResult
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public string FullText { get; set; }
    }
}