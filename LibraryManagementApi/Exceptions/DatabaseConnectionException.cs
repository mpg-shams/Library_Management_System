namespace LibraryManagementApi.Exceptions
{
    public class DatabaseConnectionException : BaseException
    {
        public DatabaseConnectionException(string message)
            : base(message, 503, "DATABASE_CONNECTION_ERROR")
        {
        }

        public DatabaseConnectionException(string message, Exception innerException)
            : base(message, 503, "DATABASE_CONNECTION_ERROR", innerException)
        {
        }

        public DatabaseConnectionException()
            : base("Unable to connect to the database. Please try again later.", 503, "DATABASE_CONNECTION_ERROR")
        {
        }
    }
}
