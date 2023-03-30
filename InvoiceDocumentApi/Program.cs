using InvoiceDocumentApi.Infrastructure;
using InvoiceDocumentApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace InvoiceDocumentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // configure sql server
            builder.Services.AddDbContext<InvoiceDocumentContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // services
            builder.Services.AddScoped<IInvoiceDocumentService, InvoiceDocumentService>();


            // setup token auth against identity server
            builder.Services.AddAuthentication("Bearer")
              .AddJwtBearer("Bearer", options =>
              {
                  options.Authority = builder.Configuration.GetValue<string>("Authority");

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateAudience = false,

                  };

                  options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                  options.MapInboundClaims = true;
              });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}