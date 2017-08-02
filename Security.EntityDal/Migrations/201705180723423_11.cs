namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER procedure [sec].[GetSecurityObjects]
	@login nvarchar(max) null,
	@appName nvarchar(max),
	@accessName nvarchar(max),
	@allowAll bit
as
declare @idApplication int = (select idApplication from sec.Applications where appName = @appName)

if @allowAll = 1
begin
	select 
		s.ObjectName
	from 
		sec.Grants g inner join sec.SecObjects s on g.idSecObject = s.idSecObject
	where 
		idRole in (select idRole from sec.RoleOfMembers where idApplication = @idApplication)
		and idAccessType = (select idAccessType from sec.AccessTypes where idApplication = @idApplication and name = @accessName)
end
else
begin
	select 
		s.ObjectName
	from 
		sec.Grants g inner join sec.SecObjects s on g.idSecObject = s.idSecObject
	where 
		idRole in (select idRole from sec.RoleOfMembers where idMember = (select idMember from sec.UsersView where login = @login) and idApplication = @idApplication)
		and idAccessType = (select idAccessType from sec.AccessTypes where idApplication = @idApplication and name = @accessName)
end");
        }
        
        public override void Down()
        {
            Sql(@"alter procedure sec.GetSecurityObjects
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
    }
}
