namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _14 : DbMigration
    {
        public override void Up()
        {
            DropIndex("sec.SecObjects", "UQ_SecObject_ObjectName");
            DropIndex("sec.SecObjects", new[] { "IdApplication" });
            DropIndex("sec.SecObjects", new[] { "IdAccessType" });
            CreateIndex("sec.SecObjects", new[] { "ObjectName", "IdApplication", "IdAccessType" }, unique: true, name: "UQ_SecObject_ObjectName");
        }
        
        public override void Down()
        {
            DropIndex("sec.SecObjects", "UQ_SecObject_ObjectName");
            CreateIndex("sec.SecObjects", "IdAccessType");
            CreateIndex("sec.SecObjects", "IdApplication");
            CreateIndex("sec.SecObjects", "ObjectName", unique: true, name: "UQ_SecObject_ObjectName");
        }
    }
}
