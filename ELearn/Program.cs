using Elearn.Model;

namespace Elearn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Razor Pages services
            builder.Services.AddRazorPages();

            // Add session services
            builder.Services.AddDistributedMemoryCache(); // Session data will be stored in memory
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;  // Make the session cookie accessible only via HTTP
                options.Cookie.IsEssential = true;  // Mark the session cookie as essential for the application
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout (optional)
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            // Use session middleware
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Map Razor Pages
            app.MapRazorPages();


            app.Run();
        }
    }
}
