namespace ReserveHub.Application.DTOs;

public class BaseResponseDto
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}
