using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DoorToDoorWeb.Controllers
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    /// <summary>
    /// This is a helper class for using Session and Temp data with .Net Core MVC Apps
    /// </summary>
    public class SessionController : Controller
    {
        private IHttpContextAccessor _httpContext = null;

        public SessionController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public void SetSessionData<T>(string key, T data)
        {
            _httpContext.HttpContext.Session.Set<T>(key, data);
        }

        public T GetSessionData<T>(string key)
        {
            return _httpContext.HttpContext.Session.Get<T>(key);
        }

        public void SetTempData<T>(string key, T data)
        {
            TempData[key] = JsonConvert.SerializeObject(data);
        }

        public T GetTempData<T>(string key)
        {
            var data = TempData[key] as string;
            return data == null ? default(T) : JsonConvert.DeserializeObject<T>(data);
        }
    }
}

// Below is the code that needs to exist in the 2 methods in the Startup.cs file for this class to work

//public void ConfigureServices(IServiceCollection services)
//{
//    services.Configure<CookiePolicyOptions>(options =>
//    {
//        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//        options.MinimumSameSitePolicy = SameSiteMode.None;
//    });

//    services.AddDistributedMemoryCache();
//    services.AddSession(options =>
//    {
//        // Sets session expiration to 20 minuates
//        options.IdleTimeout = TimeSpan.FromMinutes(20);
//        options.Cookie.HttpOnly = true;
//    });
//    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//}

//public void Configure(IApplicationBuilder app, IHostingEnvironment env)
//{
//    app.UseCookiePolicy();
//    app.UseSession();
//}

// ********* Important ***********
// public AuthController(IVendingService db, IHttpContextAccessor httpContext) : base(httpContext)
// You will need to pass the IHttpContextAccessor httpContext into the SessionController constructor from your child class