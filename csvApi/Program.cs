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
    //await db.Visitors.ToListAsync());
    await db.Visitors.FromSqlRaw("dbo.SelectAllVisitor").ToListAsync());

app.MapPost("/refreshVisitor", async (Visitor visitor, ApplicationDbContext db) =>
{


    visitor = new Visitor { Id = 4, visitor_phone = 1, visitor_name = "test1" };

    db.Visitors.Add(visitor);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{visitor.visitor_phone}", visitor);
});

 //app.MapDelete("/deleteAllVisitors", async (ApplicationDbContext db) =>

 //       await db.Visitors.de("dbo.DeleteAllVisitor"));
        


//app.MapDelete("/getAllVisitors/{id}", async (int id, ApplicationDbContext db) =>
//{
//    if (await db.Visitors.FindAsync(id) is Visitor visitor)
//    {

//        db.Visitors.Remove(visitor);
//        await db.SaveChangesAsync();
//        return Results.Ok(visitor);
//    }

//    return Results.NotFound();
//});

app.Run();