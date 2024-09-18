
namespace Share
{

    public class ActionResponse
    {

        public ActionResponse()
        {
            IsSuccess = true;
            ResponseType = ResponseType.Success;
        }
        private ActionResponse(string errorMessage, ResponseType errorType = ResponseType.BadRequest)
        {
            ResponseType = errorType;
            Message = errorMessage;
        }

        private ActionResponse(Exception ex)
        {
            Message = $"Internal Serval Error: \n \t{ex.Message} ";
            ResponseType = ResponseType.ServerError;
            ServerException = ex;
        }
        public static ActionResponse SuccessResult() => new();

        public static ActionResponse NotFoundResult(string msg = "Record is not found.") => new(msg, ResponseType.NotFound);

        public static ActionResponse BadRequestResult(string msg = "Bad request.") => new(msg, ResponseType.BadRequest);

        public static ActionResponse ServerExceptionError(Exception ex) => new(ex);

        public static ActionResponse Unauthorized(string msg = "Unauthorize") => new(msg, ResponseType.Unauthorized);



        public ResponseType ResponseType { get; private set; }
        public Exception? ServerException { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public bool IsSuccess { get; private set; }
        public object? PayLoad { get; set; }

        public object? Information { get; set; }
    }
    public enum ResponseType
    {
        Success = 0,
        BadRequest = 1,
        NotFound = 2,
        ServerError = 3,
        Unauthorized = 4,
    }
}

