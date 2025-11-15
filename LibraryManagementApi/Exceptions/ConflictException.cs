namespace LibraryManagementApi.Exceptions
{

    public class ConflictException : BaseException
    {
        public ConflictException(string message)
            : base(message, 409, "CONFLICT")
        {
        }

        public ConflictException(string entityName, string field, object value)
            : base($"{entityName} with {field} '{value}' already exists.", 409, "CONFLICT")
        {
        }
    }
}
