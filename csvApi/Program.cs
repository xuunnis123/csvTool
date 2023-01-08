using Microsoft.EntityFrameworkCore;
using csvApi;
using System.Data.SqlClient;
using System.Xml.Linq;
using csvApi.Models;
using Microsoft.EntityFrameworkCore.SqlServer;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer("Server=localhost,1433;Database=csv;User=sa;Password=reallyStrongPwd123;TrustServerCertificate=True"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

app.MapGet("/getAllVisitors", async (ApplicationDbContext db) =>
    
    await db.Visitors.FromSqlRaw("dbo.SelectAllVisitor").ToListAsync());

app.MapPost("/refreshVisitor", async (HttpRequest request,Visitor visitor, ApplicationDbContext db) =>
{


    var id = request.Query["id"];

    var visitor_phone = request.Query["visitor_phone"];

    var visitor_name = request.Query["visitor_name"];

    Console.WriteLine(id + visitor_phone + visitor_name);
    visitor = new Visitor { Id = 4, visitor_phone = 1, visitor_name = "test1" };
    try
    {
        await db.Visitors.FromSqlRaw($"dbo.AddVisitor {id},{visitor_phone},{visitor_name}").ToListAsync();
        
        await db.SaveChangesAsync();
        Console.WriteLine("success");
    }
    catch (InvalidOperationException e) {
        Console.WriteLine("e="+e);
    }
    //return "Successfully add :" + visit_name;
    return visitor_phone;
});

app.MapDelete("/deleteAllVisitors", async (ApplicationDbContext db) =>
    {
        try {
            await db.Visitors.FromSqlRaw("dbo.DeleteAllVisitor2").ToListAsync();
           
        }

        catch (InvalidOperationException e) {
            
        }
        return "Successfully delete!";
    });
    

app.Run();