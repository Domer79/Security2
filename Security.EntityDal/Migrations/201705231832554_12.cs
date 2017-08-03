namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("sec.SecObjects", "IdAccessType", c => c.Int());
            CreateIndex("sec.SecObjects", "IdAccessType");
            AddForeignKey("sec.SecObjects", "IdAccessType", "sec.AccessTypes", "IdAccessType");
        }
        
        public override void Down()
        {
            DropForeignKey("sec.SecObjects", "IdAccessType", "sec.AccessTypes");
            DropIndex("sec.SecObjects", new[] { "IdAccessType" });
            DropColumn("sec.SecObjects", "IdAccessType");
        }
    }
}
