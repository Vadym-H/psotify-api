using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Psotify;
using Psotify.Data;
using Psotify.Models.PlaylistModels;
using Psotify.Models.SongModels;
using Psotify.Validators.PlaylistValidators;
using Psotify.Validators.SongValidators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IValidator<SongUpdateModel>, SongUpdateModelValidator>()
                .AddScoped<IValidator<SongCreateModel>, SongCreateModelValidator>()
                .AddScoped<IValidator<PlaylistCreateModel>, PlaylistCreateModelValidator>()
                .AddScoped<IValidator<PlaylistUpdateModel>, PlaylistUpdateModelValidator>();
builder.Services.AddAutoMapper(x => x.AddProfile(new PsotifyMappingProfile()));
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(context.ModelState);
    });
builder.Services.AddDbContext<PsotifyDbContext>(options => options.UseInMemoryDatabase("SongsDb"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey("SUPR-SECRET-KEY-LONGER-THAN-32-CHARACTERS"u8.ToArray()),
        ValidateIssuer = false,
        ValidateAudience = false,
    });
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty; // Makes Swagger the default page
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PsotifyDbContext>();
    PsotifyDbContext.SeedData(dbContext);
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
