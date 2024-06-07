Short Answer:
- be sure to preserve case exactly as in database and force override of files:
C:\_DayStar\dev\ExampleGPA\Services\Dbio\Daystar.Services.Dbio.ASM>dotnet ef dbcontext scaffold --use-database-names --force "Data Source=(local)\SQLEXPRESS;Initial Catalog=ASM;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --project C:\_DayStar\dev\ExampleGPA\Services\Dbio\Daystar.Services.Dbio.ASM


Entity Framework for Core has very wet paint.  For now, 

1. Open a command prompt
2. CD to the Core MVC folder, e.g. CoreEF in this case.
3. Run this command:
   dotnet ef dbcontext scaffold --use-database-names --force "Data Source=(local)\SQLEXPRESS;Initial Catalog=ASM;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --project C:\_DayStar\dev\ExampleGPA\Services\Dbio\Daystar.Services.Dbio.ASM
   
4. Be sure you have Microsoft.EntityFrameworkCore.SqlServer referenced in that Dbio module
   e.g. UseSqlServer will not resolve without that referenced
   