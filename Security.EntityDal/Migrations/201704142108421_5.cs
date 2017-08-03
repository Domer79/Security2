namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            Sql(@"DROP TRIGGER [sec].[OnInsertApplicaton]");
        }
        
        public override void Down()
        {
            Sql(@"CREATE trigger [sec].[OnInsertApplicaton] on [sec].[Applications]
after insert
as
begin

insert into sec.AccessTypes select at.name, a.idApplication from inserted a right join sec.AccessTypes at on a.idApplication <> at.idApplication group by a.idApplication, at.name 

end

GO

ALTER TABLE [sec].[Applications] ENABLE TRIGGER [OnInsertApplicaton]");
        }
    }
}
