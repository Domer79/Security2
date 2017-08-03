namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            Sql(@"create procedure sec.GetSecurityObjects
	@login nvarchar(max),
	@appName nvarchar(max),
	@accessName nvarchar(max)
as

select 
	s.ObjectName
from 
	sec.Grants g inner join sec.SecObjects s on g.idSecObject = s.idSecObject
where 
	idRole in (select idRole from sec.RoleOfMembers where idMember = (select idMember from sec.UsersView where login = @login) 
	and idApplication = (select idApplication from sec.Applications where appName = @appName))
	and idAccessType = (select idAccessType from sec.AccessTypes where idApplication = (select idApplication from sec.Applications where appName = @appName) and name = @accessName)");
        }
        
        public override void Down()
        {
            Sql(@"Drop procedure sec.GetSecurityObjects");
        }
    }
}
