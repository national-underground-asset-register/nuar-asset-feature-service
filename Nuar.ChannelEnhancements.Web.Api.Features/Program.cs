
namespace Nuar.ChannelEnhancements.Web.Api.Features;

using Microsoft.OpenApi;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Helpers;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Nuar.ChannelEnhancements.Core.Data.MapConfiguration.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Use Controllers rather than minimal APIs
        builder.Services.AddControllers();

        builder.Services.AddMvc(options =>
        {
            options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
            options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
            
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        }).AddXmlSerializerFormatters();

        // TODO: Implement any authentication and authorization required by your use case here. This implementation does not include any authentication or authorization mechanisms.

        // Create the database configuration to enable the data access layer to connect to the database
        DatabaseConfigurationSettings dbConfigurationSettings = new()
        {
            ConnectionString = builder.Configuration.GetValue<string>("DatabaseConfiguration:ConnectionString") ?? throw new InvalidOperationException("Connection string for the database not found."),
            MapConfigurationSchemaName = builder.Configuration.GetValue<string>("DatabaseConfiguration:MapConfigurationSchemaName") ?? throw new InvalidOperationException("Map configuration database schema name was not provided."),
            MapConfigurationFunctionMap = builder.Configuration.GetSection("DatabaseConfiguration:MapConfigurationFunctionMap")
                .Get<Dictionary<string, string>>() ?? throw new InvalidOperationException("Map configuration function map was not provided.")
        };

        // Create the data access layer services and register them with the dependency injection container
        IDataAccessLayer dataAccessLayer = new DataAccessLayer(dbConfigurationSettings);
        builder.Services.AddScoped<IDataAccessLayer>(_ => dataAccessLayer);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi("v1", options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Version = "1.0.0";
                document.Info.Title = "Nuar Asset Feature Service";
                document.Info.Description =
                    "An implementation of an OGC API - Features compliant service that serves features from a database conforming to the NUAR Harmonised Data Model.";
                document.Info.TermsOfService = new Uri("https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/", UriKind.Absolute);
                document.Info.Contact = new OpenApiContact()
                {
                    Name = "Government Digital Service, Geospatial",
                    Email = "",
                    Url = new Uri(
                        "https://www.gov.uk/government/organisations/department-for-science-innovation-and-technology",
                        UriKind.Absolute)
                };

                return Task.CompletedTask;
            });
        });

        // Enable CORS for all origins, methods, and headers
        // TODO: Limit this as needed based on your application's requirements
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "AllowAllHosts", policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });
        });

        // Build the configured application
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Things to run only in development mode
        }

        // Expose the OpenAPI documentation on an endpoint
        app.MapOpenApi().CacheOutput();

        // Expose the Scalar interactive API documentation if configured
        if (builder.Configuration.GetValue<bool>("Scalar:ExposeUi"))
        {
            // Get the theme from settings and parse to a ScalarTheme enum, theme will be None (default theme will be applied) if this fails
            Enum.TryParse<ScalarTheme>(builder.Configuration.GetValue<string>("Scalar:Theme"), ignoreCase: true, out var scalarTheme);
            app.MapScalarApiReference(opt =>
            {
                opt.Theme = scalarTheme;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http2);
                opt.WithDarkModeToggle(builder.Configuration.GetValue<bool>("Scalar:DarkModeToggle"));
                opt.WithClientButton(builder.Configuration.GetValue<bool>("Scalar:ClientButton"));
                // TODO: Add preferred security schemes and authentication methods as needed by your use case
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseReDoc(c =>
        {
            c.SpecUrl = "/openapi/v1.json";
        });

        app.MapControllers();

        app.Run();
    }
}
