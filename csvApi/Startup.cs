using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using csvApi.Models;
using Microsoft.Extensions.Configuration;

namespace csvApi
{
	public class Startup
	{
       
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=localhost,1433;Database=csv.daily;User=sa;Password=reallyStrongPwd123;"));
		}


    }
	
}

