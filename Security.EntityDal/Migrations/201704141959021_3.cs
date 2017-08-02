namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER procedure [sec].[DeleteAppByName]
	@appName nvarchar(200) 
as
declare @idApplication int
select @idApplication = idApplication from sec.Applications where appName = @appName
exec sec.DeleteApp @idApplication");
        }
        
        public override void Down()
        {
            
        }
    }
}