
1. Correct connection strings in appsettings.config 
 
 MyXCatalog_DBConnection= "Server=<YourCatalogDBServer>; Database=<YourDBCatalogDB>;Trusted_Connection=True;MultipleActiveResultSets=true;""

 My****_DBConnection= "Server=<YourTenantDBServer>; Database=<YourTenantDB>;Trusted_Connection=True;MultipleActiveResultSets=true;""
--------------
2. To create migrations (then they will be applied in MyXSpace.DBMigrator)

 add-migration Migrname_TenantDB -project MyXSpace.EF -context TenantDbContext
or 
 add-migration Migrname_CatalogDB -project MyXSpace.EF -context CatalogDbContext
 ----------
3. To create DB and init Data start Migrator
or call in VS 2017\2019
PM> update-database -project MyXSpace.EF

(NOTE: Instead of your AzureDB Server can be "data source=adn-recette.database.windows.net;initial catalog=ADN_RECETTE_UKR;integrated security=false;persist security info=True;User ID=AdnAdminSql;Password=AdequatAZ2017SSQL")
TO generate Code-First DBContext (with models) from existing DB. For Example
  PM> Scaffold-DbContext "data source=adn-recette.database.windows.net;initial catalog=ADN_RECETTE_UKR;integrated security=false;persist security info=True;User ID=AdnAdminSql;Password=AdequatAZ2017SSQL" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx)