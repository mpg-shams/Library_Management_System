namespace LibraryManagementApi.Exceptions
{

    public class NotFoundException : BaseException
    {
        public NotFoundException(string resourceName, object key)
            : base($"{resourceName} with id '{key}' was not found.", 404, "NOT_FOUND")
        {
        }

        public NotFoundException(string message)
            : base(message, 404, "NOT_FOUND")
        {
        }
    }
}
