using MaximTechnologyTasks.Configs;
using MaximTechnologyTasks.Services;
using MaximTechnologyTasks.Middlewares;
namespace MaximTechnologyTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<StringFormatService>();
            var section = builder.Configuration.GetSection("Settings");
            builder.Services.Configure<StringFormatServiceSettings>(section);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<MaxConcurrentRequestsMiddleware>(builder.Configuration.Get<ConcurrentReqestsSetting>());


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}