using SoftOne.Exceptions;

namespace SoftOne.Common;

 
public static class RowVersionHelper
{
    public static byte[] Parse(string? rowVersionBase64)
    {
        if (string.IsNullOrWhiteSpace(rowVersionBase64))
        {
            throw new BusinessException("RowVersion is required.");
        }

        try
        {
            return Convert.FromBase64String(rowVersionBase64);
        }
        catch (FormatException)
        {
            throw new BusinessException("Invalid RowVersion format.");
        }
    }
}
