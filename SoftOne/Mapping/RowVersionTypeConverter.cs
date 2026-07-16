using AutoMapper;
using SoftOne.Common;

namespace SoftOne.Mapping;

/// <summary>
/// Globally converts BaseUpdateRequest.RowVersion (base64) into BaseEntity.RowVersion (byte[]).
/// </summary>
public class RowVersionTypeConverter : ITypeConverter<string, byte[]>
{
    public byte[] Convert(string source, byte[] destination, ResolutionContext context)
    {
        return RowVersionHelper.Parse(source);
    }
}
