using GuilianServiceApp.DAL;
using GuilianServiceApp.DAL.IDAL;


var builder = WebApplication.CreateBuilder(args);

// The required variable to add the database connection in appsettings.json
string connectionString = builder.Configuration.GetConnectionString("default");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IUserDAL>(ud => new UserDAL(connectionString));
builder.Services.AddTransient<IServiceProvidedDAL>(sp => new ServiceProvidedDAL(connectionString));
builder.Services.AddTransient<IActiveDutyDAL>(ad => new ActiveDutyDAL(connectionString));
builder.Services.AddTransient<IFeedbackDAL>(f => new FeedbackDAL(connectionString));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

builder.Services.AddDistributedMemoryCache();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
