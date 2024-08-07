namespace EndoplasmCleanArchitecture.Api.DTOs
{
    public class Response<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public int statusCode { get; set; }
        private Response(bool success, string message, int statusCode, T data)
        {
            this.success = success;
            this.message = message;
            this.data = data;
            this.statusCode = statusCode;
        }

        private Response(bool success, string message, int statusCode)
        {
            this.success = success;
            this.message = message;
            this.statusCode = statusCode;
        }

        public static Response<T> SuccessResponse(T data, int statusCode = StatusCodes.Status200OK, string message = "")
        {
            return new Response<T>(true, message, statusCode, data);
        }

        public static Response<T> ErrorResponse(string message, int statusCode)
        {
            return new Response<T>(false, message, statusCode);
        }
    }
}
