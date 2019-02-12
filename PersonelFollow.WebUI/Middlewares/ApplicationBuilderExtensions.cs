using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace PersonelFollow.WebUI.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            var path = Path.Combine(root, "bower_components");
            var provider = new PhysicalFileProvider(path);
            var options=new StaticFileOptions();
            options.RequestPath = "/bower_components";
            options.FileProvider = provider;
            app.UseStaticFiles(options);
            return app;
        }
    }
}
