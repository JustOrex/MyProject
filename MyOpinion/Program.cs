using Microsoft.EntityFrameworkCore;
using MyOpinion.Domain.Repositories.Abstract;
using MyOpinion.Domain.Repositories.EntityFramework;
using MyOpinion.Domain;
using Microsoft.AspNetCore.Identity;
using MyOpinion.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Bind("Project", new Config());

builder.Services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
builder.Services.AddTransient<IArticlesRepository, EFArticlesRepository>();
builder.Services.AddTransient<IUsersRepository, EFUsersRepository > ();
builder.Services.AddTransient<IUsersRolesRepository, EFUsersRolesRepository>();
builder.Services.AddTransient<IRolesRepository, EFRolesRepository>();
builder.Services.AddTransient<ISubjectsRepository, EFSubjectsRepository>();



builder.Services.AddTransient<DataManager>();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyOpinion.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.IsEssential = true;
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "MyOpinionAuth";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
    x.AddPolicy("UserArea", policy => { policy.RequireRole("user"); });
});

builder.Services.AddControllersWithViews(x =>
{
    x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
    x.Conventions.Add(new UserAreaAuthorization("User", "UserArea"));
});


builder.Services.AddRazorPages();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseStaticFiles();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

//app.Map("/ckfinder/connector",)


app.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseHttpsRedirection();




app.MapRazorPages();


app.Run();
