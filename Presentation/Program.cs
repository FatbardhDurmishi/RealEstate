using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.App.Implementations;
using RealEstate.App.Interfaces;
using RealEstate.Data.Context;
using RealEstate.Data.Identity;
using Microsoft.AspNetCore.Mvc.Paging;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Identity.UI.Services;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<DBRealEstateContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IRolesRepository, RolesRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPropertyRepository, PropertyRepository>();
builder.Services.AddTransient<IPropertyTypeRepository, PropertyTypeRepository>();
builder.Services.AddTransient<ITransactionTypeRepository, TransactionTypeRepository>();
builder.Services.AddTransient<IPropertyImagesRepository, PropertyImagesRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IEmailSender, EmailSenderExtensions>();
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);



//builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var en = new CultureInfo("en-US");
    en.NumberFormat.NumberDecimalSeparator = ".";
    en.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
    en.DateTimeFormat.LongTimePattern = "dd/MM/yyyy";
    en.DateTimeFormat.ShortTimePattern = "HH:mm";
    en.DateTimeFormat.LongTimePattern = "HH:mm";
    var al = new CultureInfo("sq-AL");
    al.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
    al.DateTimeFormat.LongTimePattern = "dd.MM.yyyy";
    al.DateTimeFormat.ShortTimePattern = "HH:mm";
    al.DateTimeFormat.LongTimePattern = "HH:mm";
    al.NumberFormat.NumberDecimalSeparator = ".";

    var supportedCultures = new[]
    {
        en,
        al
    };

    options.DefaultRequestCulture = new RequestCulture(en, en);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resource"; });

builder.Services.AddMvc()
    .AddViewLocalization(
        LanguageViewLocationExpanderFormat.Suffix,
        opts => opts.ResourcesPath = "Resource")
    .AddDataAnnotationsLocalization();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Individual/Account/Login";
    options.AccessDeniedPath = "/Individual/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => false; // was true
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.Configure<CookieTempDataProviderOptions>(options =>
{
    options.Cookie.IsEssential = true;
});

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

var supportedCultures = new[] { "en-US", "sq-AL" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "deafultArea",
        pattern: "{area:exists}/{controller}/{action}/{id?}"
        );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area=Individual}/{controller=Home}/{action=Index}/{id?}"
        );
});


CheckTransactionDate();



app.MapRazorPages();

app.Run();

//void SeedDatabase()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//        dbInitializer.Initialize();
//    }
//}
void CheckTransactionDate()
{
    using (var scope = app.Services.CreateScope())
    {
        var method = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
        method.CheckTransactionDate();
    }
}
