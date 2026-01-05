using CoursesOnline.Application.Interfaces;
using CoursesOnline.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesOnline.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILessonService, LessonService>();

            return services;
        }
    }
}