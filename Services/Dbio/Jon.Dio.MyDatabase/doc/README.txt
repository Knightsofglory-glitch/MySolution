Short Answer:
- be sure to preserve case exactly as in database and force override of files:
C:\Dev\MySolution\Services\Dbio\Jon.Dio.MyDatabase>dotnet ef dbcontext scaffold --use-database-names --force "Data Source=(local)\SQLEXPRESS;Initial Catalog=MyDatabase;Trusted_Connection=True; TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project C:\Dev\MySolution\Services\Dbio\Jon.Dio.MyDatabase



Entity Framework for Core has very wet paint.  For now, 

1. Open a command prompt
2. CD to the Core MVC folder, e.g. CoreEF in this case.
3. Run this command:
   dotnet ef dbcontext scaffold --use-database-names --force "Data Source=(local)\SQLEXPRESS;Initial Catalog=MyDatabase;Trusted_Connection=True; TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project C:\Dev\MySolution\Services\Dbio\Jon.Dio.MyDatabase

   
4. Be sure you have Microsoft.EntityFrameworkCore.SqlServer referenced in that Dbio module
   e.g. UseSqlServer will not resolve without that referenced
   