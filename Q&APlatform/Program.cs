using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Q_APlatform;
using Q_APlatform.IServices;
using Q_APlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection(nameof(CosmosDbSettings)));

builder.Services.AddDbContext<QuestionAnswerDbContext>
(
    (IServiceProvider sp, DbContextOptionsBuilder options) =>
    {
        var cosmosDbSettings = sp.GetRequiredService<IOptions<CosmosDbSettings>>().Value;

        options.UseCosmos(
            cosmosDbSettings.AccountEndpoint,
            cosmosDbSettings.AccountKey,
            cosmosDbSettings.DatabaseName);
    }
);
builder.Services.AddTransient<IApplicationFormService,ApplicationFormService>();
builder.Services.AddTransient<IQuestionService,QuestionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.GetService<IServiceScopeFactory>()!.CreateScope();
    var dbcontext = scope.ServiceProvider.GetRequiredService<QuestionAnswerDbContext>();
    dbcontext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
