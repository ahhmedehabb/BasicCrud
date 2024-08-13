using AspNetCore;
using AspNetCore.Authentication;
using AspNetCore.Data;
using AspNetCore.Filters;
using AspNetCore.MiddleWare;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("config.json");

builder.Services.AddLogging(cfg => cfg.AddDebug());

builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));



//var attachmentOptions=builder.Configuration.GetSection("Attachments").Get<AttachmentOptions>();

//builder.Services.AddSingleton(attachmentOptions);

//var attachmentsOptions = new AttachmentOptions();
//builder.Configuration.GetSection("Attachments").Bind(attachmentsOptions);
//builder.Services.AddSingleton(attachmentsOptions);


builder.Services.AddControllers(options =>
{
	//Global Filter
	options.Filters.Add<logActivityFilter>(); 
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(cfg => cfg.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("Basic",null);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseMiddleware<RateLimitingMiddleWare>();
app.UseMiddleware<ProfilingMiddleWare>();



app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();


