namespace SoftOne.Exceptions;

/// <summary>
/// User-facing messages for unhandled / infrastructure errors.
/// </summary>
public static class ExceptionMessages
{
    public const string UnexpectedError =
        "An unexpected error occurred. Please try again or contact support if the problem continues.";

    public const string DatabaseError =
        "A database error occurred while processing your request. Please contact support.";

    public const string SaveError =
        "Unable to save your changes. Please verify your data and try again.";
}
