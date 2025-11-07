using ReserveHub.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure application configuration
builder.Configuration.ConfigureApplicationConfiguration();

// Configure services
builder.Services.ConfigureControllers();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigurePostgresSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureApplicationCookieForApi(); // Must be after ConfigureIdentity
builder.Services.ConfigureCurrentUserService();
builder.Services.AddGoogleConfiguration(builder.Configuration);
builder.Services.AddEmailConfiguration(builder.Configuration);
builder.Services.AddFrontendConfiguration(builder.Configuration);
builder.Services.ConfigureEmailService();
builder.Services.ConfigureSwagger();

var app = builder.Build();

// Add request logging middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");
    Console.WriteLine($"[Request] QueryString: {context.Request.QueryString}");
    Console.WriteLine($"[Request] Content-Type: {context.Request.ContentType ?? "NOT SET"}");
    Console.WriteLine($"[Request] Content-Length: {context.Request.ContentLength ?? 0}");
    if (context.Request.Headers.ContainsKey("Authorization"))
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        Console.WriteLine($"[Request] Authorization header: {authHeader?.Substring(0, Math.Min(50, authHeader?.Length ?? 0))}...");
        if (authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
        {
            Console.WriteLine("[Request] Bearer token detected in Authorization header");
        }
    }
    else
    {
        Console.WriteLine("[Request] No Authorization header present");
    }
    await next();
    Console.WriteLine($"[Request] After processing - IsAuthenticated: {context.User?.Identity?.IsAuthenticated}, AuthenticationType: {context.User?.Identity?.AuthenticationType}");
    
    // Log all claims including roles
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        Console.WriteLine("[Request] User claims:");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"  - {claim.Type}: {claim.Value}");
        }
        
        // Specifically check for role claims
        var roleClaims = context.User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role").ToList();
        Console.WriteLine($"[Request] Role claims found: {roleClaims.Count}");
        foreach (var roleClaim in roleClaims)
        {
            Console.WriteLine($"  - Role: {roleClaim.Value}");
        }
        
        // Check if user is in Owner role
        var isInOwnerRole = context.User.IsInRole("Owner");
        Console.WriteLine($"[Request] IsInRole('Owner'): {isInOwnerRole}");
    }
});

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map controllers - must be after UseRouting()
app.MapControllers();

// Add middleware to catch redirects and convert to 401 for API routes
app.Use(async (context, next) =>
{
    Console.WriteLine($"[Middleware] Before Request - Path: {context.Request.Path}, Method: {context.Request.Method}");
    await next();
    Console.WriteLine($"[Middleware] After Request - StatusCode: {context.Response.StatusCode}");
    
    // If we get a redirect response for API routes, convert it to 401
    if (context.Response.StatusCode == StatusCodes.Status302Found && 
        context.Request.Path.StartsWithSegments("/api"))
    {
        Console.WriteLine("[Middleware] Converting redirect to 401 for API route");
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.Remove("Location");
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized" }));
    }
    
    // Also check for 404 on authorized endpoints - might be authorization issue
    if (context.Response.StatusCode == StatusCodes.Status404NotFound && 
        context.Request.Path.StartsWithSegments("/api") &&
        context.User?.Identity?.IsAuthenticated == false)
    {
        Console.WriteLine("[Middleware] Got 404 on API route without auth - might be authorization issue");
    }
});

app.Run();

