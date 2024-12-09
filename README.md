## Web API with custom auth
Code of this repo was initiated partially from a command like:
```bash
dotnet new webapi -n web-api -controllers
```

## Concise cookbook
[Mastering .NET 8 Web API: From Setup to Security - 50 Tips Guide for Developers](https://dev.to/ssukhpinder/mastering-net-8-web-api-from-setup-to-security-50-tips-guide-for-developers-n40)


## Extra info

[Middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-9.0)

[Video: ASP.NET Monsters #91: Middleware vs Filters](https://learn.microsoft.com/en-us/shows/aspnetmonsters/aspnet-monsters-91-middleware-vs-filters)

[Authz 1](https://learn.microsoft.com/en-us/entra/identity-platform/scenario-protected-web-api-verification-scope-app-roles?tabs=aspnetcore
)

[Authz 2](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-9.0)

Try different authentication methods behind `-au` parameter:

```bash
dotnet new webapi -h
# like
dotnet new webapi -n web-api -controllers -au IndividualB2C ...
```

### Look inside JWT tokens:
[JWT.io](https://jwt.io/)