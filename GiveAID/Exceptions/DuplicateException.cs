namespace GiveAID.Exceptions;

public class DuplicateException(string fieldName) : Exception
{
    public string FieldName { get; } = fieldName;
}
