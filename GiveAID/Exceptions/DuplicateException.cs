namespace GiveAID.Exceptions;

public class DuplicateException(string fieldName) : Exception($"Duplicate value on field {fieldName}")
{
    public string FieldName { get; } = fieldName;
}
