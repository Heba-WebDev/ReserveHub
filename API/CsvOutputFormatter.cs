using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;
namespace API;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(CustomersDto).IsAssignableFrom(type) ||
        typeof(IEnumerable<CustomersDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
    Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<CustomersDto>)
        {
            foreach(var customer in (IEnumerable<CustomersDto>)context.Object)
            {
                FormatCsv(buffer, customer);
            }
        } else
        {
            FormatCsv(buffer, (CustomersDto)context.Object!);
        }
        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CustomersDto customer)
    {
        buffer.AppendLine($"{customer.Id},\"{customer.FirstName},\"{customer.LastName},\"{customer.Email},\"{customer.Address},\"{customer.PhoneNumber}\"");
    }
}