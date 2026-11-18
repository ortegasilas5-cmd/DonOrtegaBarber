
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalaoBeleza.Data;
using SalaoBeleza.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco MySQL
var conexao = builder.Configuration.GetConnectionString("ConexaoMySQL");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        conexao,
        ServerVersion.AutoDetect(conexao)
    )
);


// Configuração de autenticação e usuários
builder.Services.AddIdentity<Usuario, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configuração MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração das requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
