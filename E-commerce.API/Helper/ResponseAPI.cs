namespace E_commerce.API.Helper
{
    public class ResponseAPI
    {
        public int statuscode { get; set; }
        public string? message { get; set; }

        public ResponseAPI(int statuscode, string? message=null)
        {
            this.statuscode = statuscode;
            //if msg null>> get msg from statuscode
            this.message = message ?? GetMessageFromStatusCode(statuscode);
        }


        private string ? GetMessageFromStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403=> "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
