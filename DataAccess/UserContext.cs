using Microsoft.EntityFrameworkCore;
using Models.Data;

namespace DataAccess
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        { }
        public DbSet<User> tblUsers { get; set; }
        public DbSet<County> lkpCounties { get; set; }
    }
}
