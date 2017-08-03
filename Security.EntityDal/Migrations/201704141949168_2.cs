namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"create procedure sec.DeleteAppByName
	@appName nvarchar(200) 
as
declare @idApplication int
select @idApplication = idApplication from sec.Applications where appName = @appName
exec DeleteApp @idApplication");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE [sec].[DeleteAppByName]");
        }
    }
}
