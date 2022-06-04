namespace XTI_Jobs.Abstractions;

public interface ITransformedSourceData
{
    Task<string> Value();
}
