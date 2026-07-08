namespace GiveAID.Exceptions;

public class MissingForeignEntityException(string referenceField): Exception
{
    public string ReferenceField { get; } = referenceField;
}
