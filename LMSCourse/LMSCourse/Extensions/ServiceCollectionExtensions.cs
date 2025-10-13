using System.Reflection;
using System.Runtime.CompilerServices;

namespace LMSCourse.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesAndRepositorys(this IServiceCollection services)
        {
            // Lấy assembly hiện tại
            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null)
                .Where(t => t.Namespace.EndsWith("Services") || t.Namespace.EndsWith("Repositories"))
                .Where(t => !t.IsDefined(typeof(CompilerGeneratedAttribute), false) && !t.Name.Contains("<"));

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                foreach (var itf in interfaces)
                {
                    // Nếu cả interface và class đều là generic mở (<>)
                    if (itf.IsGenericType && itf.ContainsGenericParameters
                        && type.IsGenericType && type.ContainsGenericParameters)
                    {
                        services.AddScoped(itf.GetGenericTypeDefinition(), type.GetGenericTypeDefinition());
                        Console.WriteLine($"[DI] Registered open generic: {itf.Name} -> {type.Name}");
                    }
                    else
                    {
                        services.AddScoped(itf, type);
                        Console.WriteLine($"[DI] Registered: {itf.Name} -> {type.Name}");
                    }
                }
            }
        }
    }
}
