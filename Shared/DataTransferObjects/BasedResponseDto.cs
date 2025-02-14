namespace Shared.DataTransferObjects;

public class BasedResponseDto
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}
