
using AutoMapper;
using BooksApiApp.Configuration;
using BooksApiApp.Data;
using BooksApiApp.Helpers;
using BooksApiApp.Repositories;
using BooksApiApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BooksApiApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });

            // Configure DbContext
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<BooksWebApiContext>(options => options.UseSqlServer(connString));

            // Configure services
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddRepositories();



            // Configure AutoMapper as a singleton
            builder.Services.AddAutoMapper(typeof(MapperConfig));

            ////Per request scope
            //builder.Services.AddScoped(provider =>
            //new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new MapperConfig());
            //})
            //.CreateMapper());


            ///Add Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    SignatureValidator = (token, validator) => { return new JsonWebToken(token); }
                };
            });


            /// System.Text.JSON
            /*builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                // Adding a converter for string enums
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });*/

            // If NewtonSoft would be used for json serialization / deserialization
            // We have to add the NuGet dependencies an the following config


            // Configure JSON serialization
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });


            //builder.Services.AddControllers()
            //    .AddNewtonsoftJson(options =>
            //    {
            //        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            //        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            //        options.SerializerSettings.Converters.Add(new StringEnumConverter());
            //    });


            
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Books API", Version = "v1" });
                // Non-nullable reference are properly documented
                options.SupportNonNullableReferenceTypes();
                options.OperationFilter<AuthorizeOperationFilter>();
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT"
                    });
            });

            // Configure CORS
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    b => b.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
            });

            builder.Services.AddCors(options => {
                options.AddPolicy("AngularClient",
                    b => b.WithOrigins("http://localhost:4200") // Assuming Angular runs on localhost:4200
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AngularClient");

            app.UseAuthentication();
            app.UseAuthorization();

            /// Global Error Handler
            app.UseMiddleware<ErrorHandlerMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}
