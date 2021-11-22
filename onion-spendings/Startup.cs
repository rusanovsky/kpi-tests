using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Spendings.Orchrestrators.Users;
using Spendings.Core.Users;
using Spendings.Data.DB;
using Spendings.Data.Users;
using Spendings.Core.Records;
using Spendings.Orchrestrators.Records;
using Spendings.Data.Records;
using Spendings.Data.Categories;
using Spendings.Core.Categories;
using Spendings.Orchrestrators.Categories;

namespace onion_spendings
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {

            string connString = Configuration.GetConnectionString("SpendingsDB");
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Spendings Api",
                    Description = ""
                });
            });
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });
            services.AddControllers();
            services.AddAutoMapper(typeof(OrchUserProfile), typeof(UserOrchProfile), typeof(UserDaoProfile), typeof(DaoUserProfile),
                typeof(RecordDaoProfile), typeof(RecordContractProfile),typeof(CategoryDaoProfile),typeof(CategoryContactProfile),typeof(DaoCategoryProfile),typeof(CategoryProfile));
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRecordRepository, RecordRepository>();
            services.AddScoped<IRecordService, RecordService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //env.EnvironmentName = "Production";
           
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spendings API V1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
