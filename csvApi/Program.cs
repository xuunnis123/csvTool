using Microsoft.EntityFrameworkCore;
using csvApi;
using System.Data.SqlClient;
using System.Xml.Linq;
using csvApi.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer("Server=csv-server.database.windows.net,1433;Database=csv_collection;User=xuunnis123;Password=!Xuunnis456;TrustServerCertificate=True"));
//builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer("Server=localhost,1433;Database=csv;User=sa;Password=reallyStrongPwd123;TrustServerCertificate=True"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

app.MapGet("/getAllVisitors", async (ApplicationDbContext db) =>
    
    await db.Visitors.FromSqlRaw("dbo.SelectAllVisitor").ToListAsync());

app.MapPost("/refreshVisitor", async (HttpRequest request, ApplicationDbContext db) =>
{

    var visitor = await request.ReadFromJsonAsync<Visitor>();

    Console.WriteLine("在 API 上取得：" + visitor.Id + visitor.visitor_phone + visitor.visitor_name);
    try
    {
        await db.Visitors.FromSqlRaw($"dbo.AddVisitor {visitor.Id},{visitor.visitor_phone},{visitor.visitor_name}").ToListAsync();
        
        await db.SaveChangesAsync();
        Console.WriteLine("success");
    }
    catch (InvalidOperationException e) {
        Console.WriteLine("e="+e);
    }
    
    return visitor.Id+"|"+visitor.visitor_name+"|"+ visitor.visitor_phone;
});

app.MapDelete("/deleteAllVisitors", async (ApplicationDbContext db) =>
    {
        try {
            await db.Visitors.FromSqlRaw("dbo.DeleteAllVisitor").ToListAsync();
           
        }

        catch (InvalidOperationException e) {
            
        }
        return "Successfully delete!";
    });
    

app.Run();