namespace E_commerce.API.Helper
{
    public class ApiException : ResponseAPI
    {
        // we make this class to show status code and message and details of exption 

        public ApiException(int statuscode, string? message = null, string? Details=null) : base(statuscode, message)
        {
            _Details = Details;
        }

        public string _Details { get; set; }
    }
}
