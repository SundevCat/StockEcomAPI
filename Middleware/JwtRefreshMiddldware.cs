namespace StockAPI.Moddleware;
public class JwtRefreshMiddleware
{
    private readonly RequestDelegate _next;
    public JwtRefreshMiddleware(RequestDelegate next)
    {
        _next = next;
    }
}