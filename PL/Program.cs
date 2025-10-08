using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ht1ejtthghXbl1KdOwyRzLUDDfiQhGsbZMiGrguet3BRBjz37RvGa")
            )
        };

        // Permitir leer token desde cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["JwtToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            },
        };
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




// Token

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

//    .AddJwtBearer(options =>
//    {
//        options.Events = new JwtBearerEvents

//        {

//            OnMessageReceived = context =>

//            {

//                var token = context.Request.Cookies["JwtToken"];

//                if (!string.IsNullOrEmpty(token))

//                {

//                    context.Token = token;

//                }

//                return Task.CompletedTask;

//            },

//            OnChallenge = context =>

//            {

//                context.HandleResponse();

//                context.Response.Redirect("/Login/Login");

//                return Task.CompletedTask;

//            }

//        };

//    });


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
