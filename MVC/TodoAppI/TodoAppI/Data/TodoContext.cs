using Microsoft.EntityFrameworkCore;

namespace TodoAppI.Data;

public class TodoContext(DbContextOptions options) : DbContext(options)
{
    
}