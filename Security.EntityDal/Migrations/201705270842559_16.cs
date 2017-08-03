namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16 : DbMigration
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
declare @idMember int = (select idMember from sec.Members where name = @login)
if @allowAll = 1
begin
	select 
		s.ObjectName
	from 
		sec.Grants g inner join sec.SecObjects s on g.idSecObject = s.idSecObject
	where 
		idRole in (select idRole from sec.RoleOfMembers where idApplication = @idApplication)
		and g.idAccessType = (select idAccessType from sec.AccessTypes where idApplication = @idApplication and name = @accessName)
end
else
begin
	select 
		s.ObjectName
	from 
		sec.Grants g inner join sec.SecObjects s on g.idSecObject = s.idSecObject
	where 
		idRole in (select idRole from sec.RoleOfMembers where idMember in (select idGroup from sec.UserGroups where idUser = @idMember union all select idMember from sec.Users where idMember = @idMember) and idApplication = @idApplication)
		and g.idAccessType = (select idAccessType from sec.AccessTypes where idApplication = @idApplication and name = @accessName)
end");
        }
        
        public override void Down()
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
    }
}
