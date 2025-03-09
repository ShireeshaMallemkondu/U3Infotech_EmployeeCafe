using FullStackAssessment_U3Infotech.Data;
using FullStackAssessment_U3Infotech.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:50456")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
var client = new HttpClient(handler);

builder.Services.AddControllers();



builder.Services.AddDbContext<AssessmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly)); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowLocalhost");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AssessmentDbContext>();

    try
    {
        context.Database.Migrate();
        SeedData(context); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }

    try
    {
        context.Database.Migrate();
        SeedEmployeeCafeRelationsData(context);
    }
    catch (Exception ex)
    {

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the employeecaferelations database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedData(AssessmentDbContext context)
{
    if (!context.Cafes.Any())
    {
        context.Cafes.AddRange(
            new Cafe
            {
                Name = "StarBucks",
                Description = "A place to drink coffee and do work as well",
                Location = "Singapore",
                Logo = "logo1.jpeg"
            },
            new Cafe
            {
                Name = "Barista",
                Description = "A hangout place with friends",
                Location = "India",
                Logo = "logo2.jpeg"
            }
        );
        context.SaveChanges();
    }

    if (!context.Employees.Any())
    {
        context.Employees.AddRange(
            new Employee
            {
                ID = "UI1234567",
                Name = "Siri",
                Email_Address = "siriabc@gmail.com",
                Phonenumber = "98761234",
                Gender = "Female"
            },
            new Employee
            {
                ID = "UI3456789",
                Name = "Giri",
                Email_Address = "giriabc@gmail.com",
                Phonenumber = "98761234",
                Gender = "Male"
            }
        );
        context.SaveChanges();
    }
}
void SeedEmployeeCafeRelationsData(AssessmentDbContext context)
{
    if (!context.EmployeeCafeRelations.Any())
    {
        var employees = context.Employees.ToList();
        var cafes = context.Cafes.ToList();

        var employeeCafeRelations = new List<EmployeeCafeRelation>();

        foreach (var employee in employees)
        {
            var cafeId = cafes.FirstOrDefault()?.ID;
            if (cafeId != null)
            {
                employeeCafeRelations.Add(new EmployeeCafeRelation
                {
                    EmployeeID = employee.ID,
                    CafeID = cafeId.Value,
                    StartDate = DateTime.Now
                });
            }
        }
        context.EmployeeCafeRelations.AddRange(employeeCafeRelations);
        context.SaveChanges();
    }
}
