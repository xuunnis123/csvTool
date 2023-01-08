using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace csvApi.Models;
[Table("visitor")]
public class Visitor
{
  
    public int Id { get; set; }
    public int visitor_phone { get; set; }
	public string ?visitor_name { get; set; }
	
}

