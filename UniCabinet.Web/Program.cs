using Microsoft.EntityFrameworkCore;
using UniCabinet.Infrastructure;
using UniCabinet.Infrastructure.Data;
using UniCabinet.Web.DataSending;
using UniCabinet.Web.Extension;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� ������������ ������ ����������� ��� Entity Framework Core
builder.Logging.AddConsole();

// ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // ����������� � ���� ������
    options.UseSqlServer(connectionString);
    // �������� ����������� �������������� ������ ������ � dev-�����
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

�ookiesExtensions.AddCookies(builder.Services);
SessionsExtensions.AddSession(builder.Services);
IdentityExtensions.AddIdentity(builder.Services);


InfrastructureServicesExtensions.AddInfrastructureLayer(builder.Services);
ApplicationsServicesExtensions.AddApplicationLayer(builder.Services);

// ����������� ������������ API
builder.Services.AddControllers();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
await DatabaseSeeder.SeedDatabaseAsync(app);


// Middleware ������������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// ������������� Razor Pages � MVC ������������
app.MapRazorPages();
app.MapControllers(); // ��������� ����� MapRazorPages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
