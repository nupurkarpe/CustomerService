namespace CustomerService.Helper
{
    public static class ApiResponse
    {
        public static object Success(string message, object data)
        {
            return new
            {
                success = true,
                message,
                data,
                error = (object)null
            };
        }
        public static object Error(string message, string code, string details)
        {
            return new
            {
                success = false,
                message,
                data = (object)null,
                error = new
                {
                    code,
                    details
                }
            };
        }

    }
}
