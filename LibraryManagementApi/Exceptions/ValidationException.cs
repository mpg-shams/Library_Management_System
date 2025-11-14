namespace LibraryManagementApi.Exceptions
{

    public class ValidationException : BaseException
    {
        public Dictionary<string, string[]>? ValidationErrors { get; }

        public ValidationException(string message)
            : base(message, 400, "VALIDATION_ERROR")
        {
        }

        public ValidationException(string field, string error)
            : base($"Validation failed for {field}", 400, "VALIDATION_ERROR")
        {
            ValidationErrors = new Dictionary<string, string[]>
            {
                { field, new[] { error } }
            };
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("One or more validation errors occurred.", 400, "VALIDATION_ERROR")
        {
            ValidationErrors = errors;
        }
    }
}
