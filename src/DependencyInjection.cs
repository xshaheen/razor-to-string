using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sharp.RazorToString {
    public static class DependencyInjection {
        public static IServiceCollection AddRazorToString(
            this IServiceCollection services,
            Action<RazorToStringOptions> configureOptions
        ) {
            services.Configure(configureOptions);
            services.AddTransient<IRazorToStringRenderer, RazorToStringRenderer>();

            return services;
        }
    }
}