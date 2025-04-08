var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add the HTTP client factory
builder.Services.AddHttpClient();

// Add the in memory cache
builder.Services.AddSession( options => {
    options.IdleTimeout = TimeSpan.FromMinutes( 1 );//You can set Time   
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
} );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}" );
app.MapRazorPages()
   .WithStaticAssets();

// generate an api-key on startup that we can use to validate callbacks
System.Environment.SetEnvironmentVariable( "API-KEY", Guid.NewGuid().ToString() );

app.Run();
