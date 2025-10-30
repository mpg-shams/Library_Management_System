namespace LibraryManagementApi.Models
{
    public enum BookStatus
    {
        Available,
        Borrowed,
        Lost
    }

    public enum BorrowStatus
    {
        Active,
        Returned,
        Overdue
    }
}