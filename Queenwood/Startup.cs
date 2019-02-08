using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using NetEscapades.AspNetCore.SecurityHeaders;
using Queenwood.Core.Client;
using Queenwood.Core.Client.Ebay;
using Queenwood.Core.Client.Etsy;
using Queenwood.Core.Client.Instagram;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.ContentfulService;
using Queenwood.Core.Services.EmailService;
using Queenwood.Models.Config;

namespace Queenwood
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get config settings
            services.Configure<EmailConfig>(Configuration.GetSection("Emails"));
            services.Configure<EtsyConfig>(Configuration.GetSection("Etsy"));
            services.Configure<EbayConfig>(Configuration.GetSection("Ebay"));
            services.Configure<InstagramConfig>(Configuration.GetSection("Instagram"));
            services.Configure<ContentfulConfig>(Configuration.GetSection("ContentfulOptions"));


            // Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });


            // Cache on server based on client cache rules
            services.AddResponseCaching();

            // Client side caching profiles, keep small because no way to invalidate
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        VaryByHeader = "Accept-Encoding",
                        Location = ResponseCacheLocation.Any,
                        Duration = 60
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddSession();

            // Add application services
            services.AddSingleton<IBaseClient, BaseClient>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IContentfulService, ContentfulService>();
            services.AddTransient<IEtsyClient, EtsyClient>();
            services.AddTransient<IEbayClient, EbayClient>();
            services.AddTransient<IInstagramClient, InstagramClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add default security headers
            var policyCollection = new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders();

            app.UseSecurityHeaders(policyCollection);

            if (env.IsDevelopment())
            {
                // detailed error pages
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // friendly error pages
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                // output caching in production
                app.UseResponseCaching();
            }

            // Gzip
            app.UseResponseCompression();

            // add cache control header to static resources
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(365),
                        Public = true
                    };
                }
            });

            // Enable sessions
            //app.UseSession();            

            // don't pass routes for Attribute routing
            app.UseMvc();
        }
    }
}
