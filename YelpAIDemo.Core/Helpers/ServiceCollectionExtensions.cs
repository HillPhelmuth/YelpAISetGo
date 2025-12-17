using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using YelpAIDemo.Core.Services;

namespace YelpAIDemo.Core.Helpers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYelpAIServices(this IServiceCollection services)
    {
        return services
            .AddScoped<AppState>()
            .AddScoped<YelpAiAgentOrchestration>()
            .AddScoped<YelpAIService>()
            .AddScoped<YelpReservationsService>()
            .AddScoped<ITravelItineraryStore, BrowserStorageService>()
            .AddScoped<IAgentStorageService, BrowserStorageService>().AddScoped<ReverseGeocodeService>();
    }
}