namespace MimCrm.Api.Contracts;

public sealed record CreateCustomerRequest(string Name, string Email, string Phone);
