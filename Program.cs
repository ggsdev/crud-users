using ANPCentral.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<UserDataContext>();
var app = builder.Build();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
