var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddIdentity<IdentityUser, IdentityRole>( 
    opt => { opt.SignIn.RequireConfirmedEmail = true; 
            opt.SignIn.RequireConfirmedPhoneNumber = true;
    } )
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"] ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( configuration["JWT:key"] ))
        };
    });


builder.Services.Configure<SendgridSettings>(configuration.GetSection("Sendgrid"));
builder.Services.AddTransient<SendGridService, SendGridEmailSender>();

builder.Services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
builder.Services.AddTransient<TwilioService, TwilioSMSSender>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>  {
    // adding the authorize input modal, used for endpoints with token
swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });

                // makes the swagger ui get rid of the padlock icons for endpoints that does not need authorization
                swagger.OperationFilter<AuthorizationOperationFilter>();                

                // for enabling the triple-slash documentation tags on your methods 
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // hides the schema at the bottom of the swagger page
    app.UseSwaggerUI(c => {
    c.DefaultModelsExpandDepth(-1);});
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
