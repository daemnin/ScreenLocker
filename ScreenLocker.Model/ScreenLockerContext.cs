using MySql.Data.Entity;
using ScreenLocker.Model.Contracts;
using System.Data.Entity;

namespace ScreenLocker.Model
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ScreenLockerContext : DbContext, IContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<UsageLog> UsageLogs { get; set; }

        public ScreenLockerContext() : base("ScreenLockerContext") { }
    }
}