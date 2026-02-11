namespace MimCrm.Api.Contracts;

public sealed record CreateProductRequest(string Name, string Description, decimal Price);
