using Microsoft.EntityFrameworkCore;

namespace TrickyTrayAPI.Data
{
    public class TrickyTrayContextFactory
    {
        //private const string ConnectionString = "Server=srv2\\pupils;DataBase=TrickyTrayDB;Integrated Security=SSPI;" +
        //    "Persist Security Info=False;TrustServerCertificate=true";
        private const string ConnectionString = "Server=DESKTOP-3B90OSN;DataBase=TrickyTrayDB;Integrated Security=SSPI;" +
            "Persist Security Info=False;TrustServerCertificate=true";

        public static TrickyTrayDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TrickyTrayDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            return new TrickyTrayDbContext(optionsBuilder.Options);
        }
    }
}



//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace TrickyTrayAPI.Data
//{
//    public class TrickyTrayDbContextFactory
//        : IDesignTimeDbContextFactory<TrickyTrayDbContext>
//    {
//        public TrickyTrayDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<TrickyTrayDbContext>();

//            optionsBuilder.UseSqlServer(
//                "Server=DESKTOP-FRV6PSE;" +
//                "Database=TrickyTrayDB;" +
//                "Integrated Security=True;" +
//                "TrustServerCertificate=True"
//            );

//            return new TrickyTrayDbContext(optionsBuilder.Options);
//        }
//    }
//}
