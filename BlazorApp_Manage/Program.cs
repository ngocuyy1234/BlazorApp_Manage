

using BlazorApp_Manage.Components;
using BlazorApp_Manage.Data; // ?? s? d?ng WebAppManageContext
using BlazorApp_Manage.Services; // ?? s? d?ng AuthService
using BlazorApp_Manage.Auth; // ?? s? d?ng CustomAuthStateProvider
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp_Manage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. C?u hình k?t n?i Database (L?y t? appsettings.json)
            builder.Services.AddDbContextFactory<WebAppManageContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication();

            // 3.dang ky Business Service
            builder.Services.AddScoped<AuthService>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // ??ng ký Authentication
            builder.Services.AddCascadingAuthenticationState();
            // QUAN TR?NG: Ph?i ??ng ký c? 2 dòng này cho cùng m?t ??i t??ng
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

            // ??ng ký l?p g?c
            builder.Services.AddScoped<CustomAuthStateProvider>();

            // Câu dây: Khi ai ?ó h?i AuthenticationStateProvider, hãy ??a ?úng cái CustomAuthStateProvider ? trên
            builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
                sp.GetRequiredService<CustomAuthStateProvider>());

            // Kích ho?t tr?ng thái Cascading
            builder.Services.AddCascadingAuthenticationState();


            var app = builder.Build();

            // Configure the HTTP request pipeline. //quan trong khong duoc xoa
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            // 4. Kích ho?t Middleware xác th?c và phân quy?n
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}