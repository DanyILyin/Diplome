using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProgulkaProject.Domain;
using ProgulkaProject.Domain.Rpositories.Abstract;
using ProgulkaProject.Domain.Rpositories.EntityFramework;
using ProgulkaProject.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.Bind("Project", new Config());


// ���������� ������ ���������� ���������� � �������� ��������
builder.Services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository > ();
builder.Services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository> ();
builder.Services.AddTransient<DataManager>();

//���������� �������� � ��
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

//����������� identity �������
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//����������� authentification cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "MyProjectAuth";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
});
//����������� �������� ����������� ��� Admin area
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
});

//��������� ������� ��� ������������ � ������������� MVC 
builder.Services.AddControllersWithViews(x =>
{
    x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();

//���������� ������� �������������
app.UseRouting();

//���������� �������������� � �����������
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//������������ ������ ��������
app.MapControllerRoute(name: "admin",  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
