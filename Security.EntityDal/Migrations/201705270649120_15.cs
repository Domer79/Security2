namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _15 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER function [sec].[IsAllowByName](@secObject nvarchar(200), @member nvarchar(200), @accessType nvarchar(300), @appName nvarchar(200))
returns bit
as
begin
declare @idApplication int = (select idApplication from sec.Applications where appName = @appName)
declare @idAccessType int = (select idAccessType from sec.AccessTypes where name = @accessType and idApplication = @idApplication)
declare @idSecObject int = (select idSecObject from sec.SecObjects where ObjectName = @secObject and idApplication = @idApplication and IdAccessType = @idAccessType)
declare @idMember int = (select idMember from sec.Members where name = @member)

return sec.IsAllowById(@idSecObject, @idMember, @idAccessType)
end");

            Sql(@"ALTER function [sec].[IsAllowById1](@idSecObject int, @idMember int, @idAccessType int)
returns bit
as
begin
	declare @result bit = 0

	--Сначала проверяем доступ, если участником безопасности является группа
	if not exists(select 1 from sec.Users where idMember = @idMember)
	begin
		if exists
				(
					select
						1
					from
						sec.Grants gr inner join sec.SecObjects so 
					on
						gr.idSecObject = so.idSecObject inner join sec.RoleOfMembers rof 
					on
						gr.idRole = rof.idRole 
					where 
						so.idSecObject = @idSecObject
						and rof.idMember = @idMember
						and gr.idAccessType = @idAccessType
				)
		begin
			set @result = 1
		end
		
		return @result
	end

	--Проверяем доступ по списку всех групп, в которых состоит пользователь, не забывая при этом вклювить самого пользователя в этот список
    if exists(select 1 from sec.Users where idMember = @idMember and status = 0)
        return cast(0 as bit)

	if exists
			(
				select
					1
				from
					sec.Grants gr inner join sec.SecObjects so 
				on
					gr.idSecObject = so.idSecObject inner join sec.RoleOfMembers rof 
				on
					gr.idRole = rof.idRole 
				where 
					so.idSecObject = @idSecObject
					and rof.idMember in (select idGroup from sec.UserGroups where idUser = @idMember union all select idMember from sec.Users where idMember = @idMember)
					and gr.idAccessType = @idAccessType
			)
	begin
		set @result = 1
	end

	return @result
end");
        }
        
        public override void Down()
        {
            Sql(@"ALTER function [sec].[IsAllowByName](@secObject nvarchar(200), @member nvarchar(200), @accessType nvarchar(300), @appName nvarchar(200))
returns bit
as
begin
declare @idApplication int = (select idApplication from sec.Applications where appName = @appName)
declare @idAccessType int = (select idAccessType from sec.AccessTypes where name = @accessType and idApplication = @idApplication)
declare @idSecObject int = (select idSecObject from sec.SecObjects where ObjectName = @secObject and idApplication = @idApplication and IdAccessType = @idAccessType)
declare @idMember int = (select idMember from sec.Members where name = @member)

return sec.IsAllowById(@idSecObject, @idMember, @idAccessType)
end");

            Sql(@"ALTER function [sec].[IsAllowById1](@idSecObject int, @idMember int, @idAccessType int)
returns bit
as
begin
	declare @result bit = 0

	--Сначала проверяем доступ, если участником безопасности является группа
	if not exists(select 1 from sec.Users where idMember = @idMember)
	begin
		if exists
				(
					select
						1
					from
						sec.Grants gr inner join sec.SecObjects so 
					on
						gr.idSecObject = so.idSecObject inner join sec.RoleOfMembers rof 
					on
						gr.idRole = rof.idRole 
					where 
						so.idSecObject = @idSecObject
						and rof.idMember = @idMember
						and idAccessType = @idAccessType
				)
		begin
			set @result = 1
		end
		
		return @result
	end

	--Проверяем доступ по списку всех групп, в которых состоит пользователь, не забывая при этом вклювить самого пользователя в этот список
    if exists(select 1 from sec.Users where idMember = @idMember and status = 0)
        return cast(0 as bit)

	if exists
			(
				select
					1
				from
					sec.Grants gr inner join sec.SecObjects so 
				on
					gr.idSecObject = so.idSecObject inner join sec.RoleOfMembers rof 
				on
					gr.idRole = rof.idRole 
				where 
					so.idSecObject = @idSecObject
					and rof.idMember in (select idGroup from sec.UserGroups where idUser = @idMember union all select idMember from sec.Users where idMember = @idMember)
					and idAccessType = @idAccessType
			)
	begin
		set @result = 1
	end

	return @result
end");
        }
    }
}
