using ANPCentral;
using ANPCentral.Data;
using ANPCentral.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

app.MapControllers();

app.Run();

ConfigureMiddlewares(app);

void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddDbContext<UserDataContext>();
    services.AddAutoMapper(typeof(Program));
}

void ConfigureMiddlewares(IApplicationBuilder app)
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<VerifyIfUserExistsMiddleware>();
    app.UseRouting();
}

