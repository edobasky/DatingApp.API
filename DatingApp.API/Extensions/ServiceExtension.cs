using DatingApp.API.Data;
using DatingApp.API.Service.Interface;
using DatingApp.API.Service;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Repository;

namespace DatingApp.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConn"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenServiceImp>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
