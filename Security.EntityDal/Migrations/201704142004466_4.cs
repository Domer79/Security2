namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            Sql("exec sec.DeleteAppByName N'Default'");
        }
        
        public override void Down()
        {
            Sql("exec sec.AddApp N'Default', N'Приложение по умолчанию'");
        }
    }
}