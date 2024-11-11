namespace Entities.Exceptions;

public sealed class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(Guid CustomerId): base($"Customer with id: {CustomerId} not found")
    {

    }
}