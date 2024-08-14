using AspNetCore.Authentication;
using AspNetCore.Data;
using AspNetCore.Filters;
using AspNetCore.MiddleWare;
using BasicCrudOperation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthentication()
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtOptions.Issuer,
			ValidateAudience = true,
			ValidAudience = jwtOptions.Audience,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
		};
	});
	//.AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("Basic",null);

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


