namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("sec.SecObjects", "IdAccessType", "sec.AccessTypes");
            DropIndex("sec.SecObjects", new[] { "IdAccessType" });
            Sql(@"delete sec.SecObjects where idSecObject in (select idSecObject from sec.SecObjects except select idSecObject from sec.Grants)
update sec.SecObjects set idAccessType = grants.idAccessType from (select idSecObject, idAccessType from sec.Grants) grants where sec.SecObjects.idSecObject = grants.idSecObject");
            AlterColumn("sec.SecObjects", "IdAccessType", c => c.Int(nullable: false));
            CreateIndex("sec.SecObjects", "IdAccessType");
            AddForeignKey("sec.SecObjects", "IdAccessType", "sec.AccessTypes", "IdAccessType", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("sec.SecObjects", "IdAccessType", "sec.AccessTypes");
            DropIndex("sec.SecObjects", new[] { "IdAccessType" });
            AlterColumn("sec.SecObjects", "IdAccessType", c => c.Int());
            CreateIndex("sec.SecObjects", "IdAccessType");
            AddForeignKey("sec.SecObjects", "IdAccessType", "sec.AccessTypes", "IdAccessType");
        }
    }
}
