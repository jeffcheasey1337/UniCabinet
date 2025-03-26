﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UniCabinet.Application.BackgroundServices;

namespace UniCabinet.Infrastructure
{
    public class ApplicationsServicesExtensions
    {
        public static void AddApplicationLayer(IServiceCollection services)
        {
            services.AddHostedService<SemesterBackgroundService>();
            services.AddHostedService<CourseBackgroundService>();
            services.AddHostedService<LectureProgressUpdateService>();

            var useCaseTypes = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.Name.EndsWith("UseCase") && t.IsClass && !t.IsAbstract);

            foreach (var type in useCaseTypes)
            {
                services.AddTransient(type);
            }

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            try
            {
                var mapper = services.BuildServiceProvider().GetRequiredService<IMapper>();
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
            }
            catch (AutoMapperConfigurationException ex)
            {
                Console.WriteLine(ex.Message); // Посмотреть подробности ошибки
                throw;
            }

        }
    }
}
