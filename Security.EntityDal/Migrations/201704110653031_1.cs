namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            Sql(@"/*Сущность приложения sec.Applications*/

create table sec.Applications
(
	idApplication int not null primary key identity,
	appName nvarchar(200) not null,
	description nvarchar(max)
)
go

create unique index UQ_Application_Name on sec.Applications (appName asc)
go

set identity_insert sec.Applications on
Insert into sec.Applications(idApplication, appName, description) values(1, N'Default', N'Приложение по умолчанию')
set identity_insert sec.Applications off
go

Alter table sec.Roles
add idApplication int not null
go

alter table sec.SecObjects
add idApplication int not null
go

Alter table sec.AccessTypes
add idApplication int not null
go

/****** Object:  Index [UQ_Role_Name]    Script Date: 14.04.2015 11:08:44 ******/
if exists(select 1 from sys.indexes where name = N'UQ_Role_Name' and OBJECT_NAME(object_id) = N'Roles')
begin
	DROP INDEX [UQ_Role_Name] ON [sec].[Roles];
end

CREATE UNIQUE NONCLUSTERED INDEX [UQ_Role_Name] ON [sec].[Roles]
(
	[name] ASC, [idApplication] asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  Index [UQ_SecObject_ObjectName]    Script Date: 14.04.2015 11:08:44 ******/
if exists(select 1 from sys.indexes where name = N'UQ_SecObject_ObjectName' and OBJECT_NAME(object_id) = N'SecObjects')
begin
	DROP INDEX [UQ_SecObject_ObjectName] ON [sec].[SecObjects];
end

CREATE UNIQUE NONCLUSTERED INDEX [UQ_SecObject_ObjectName] ON [sec].[SecObjects]
(
	[ObjectName] ASC, [idApplication] asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [UQ_AccessType_AccessName]    Script Date: 14.04.2015 11:08:44 ******/
if exists(select 1 from sys.indexes where name = N'UQ_AccessType_AccessName' and OBJECT_NAME(object_id) = N'AccessTypes')
begin
	DROP INDEX [UQ_AccessType_AccessName] ON [sec].[AccessTypes];
end

CREATE UNIQUE NONCLUSTERED INDEX [UQ_AccessType_AccessName] ON [sec].[AccessTypes]
(
	[Name] ASC, [idApplication] asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

alter table sec.Roles
add constraint FK_Roles_Applications foreign key(idApplication) references sec.Applications(idApplication) on delete cascade,
constraint DF_IdApplication_Roles default 1 for idApplication
go

alter table sec.AccessTypes
add constraint FK_AccessTypes_Applications foreign key(idApplication) references sec.Applications(idApplication) on delete no action,
constraint DF_IdApplication_AccessTypes default 1 for idApplication
go

alter table sec.SecObjects
add constraint FK_SecObjects_Applications foreign key(idApplication) references sec.Applications(idApplication) on delete no action,
constraint DF_IdApplication_SecObjects default 1 for idApplication
go

create procedure sec.AddApp
	@appName nvarchar(200),
	@description nvarchar(max)
as
insert into sec.Applications values(@appName, @description)
select SCOPE_IDENTITY() as idApplication
GO

create procedure sec.UpdateApp
	@idApplication int,
	@appName nvarchar(200),
	@description nvarchar(max)
as
Update sec.Applications set appName = @appName, description = @description where idApplication = @idApplication
go

create procedure [sec].[DeleteApp]
	@idApplication int
as
delete from sec.Roles where idApplication = @idApplication
delete from sec.AccessTypes where idApplication = @idApplication
delete from sec.SecObjects where idApplication = @idApplication
delete from sec.Applications where idApplication = @idApplication
go

ALTER function [sec].[IsAllowByName](@secObject nvarchar(200), @member nvarchar(200), @accessType nvarchar(300), @appName nvarchar(200))
returns bit
as
begin
declare @idApplication int = (select idApplication from sec.Applications where appName = @appName)
declare @idSecObject int = (select idSecObject from sec.SecObjects where ObjectName = @secObject and idApplication = @idApplication)
declare @idMember int = (select idMember from sec.Members where name = @member)
declare @idAccessType int = (select idAccessType from sec.AccessTypes where name = @accessType and idApplication = @idApplication)

return sec.IsAllowById(@idSecObject, @idMember, @idAccessType)
end
go

--DROP TRIGGER [sec].[OnInsertApplicaton]
--GO


CREATE trigger [sec].[OnInsertApplicaton] on [sec].[Applications]
after insert
as
begin

insert into sec.AccessTypes select at.name, a.idApplication from inserted a right join sec.AccessTypes at on a.idApplication <> at.idApplication group by a.idApplication, at.name 

end

GO

create trigger sec.OnInsertGrant on sec.Grants
after INSERT, UPDATE
as
if exists(
			select 
				1
			from
			(
				select 
					gr.idSecObject, gr.idRole, gr.idAccessType, so.idApplication secObjectIdApp, r.idApplication roleIdApp, a.idApplication accessTypeIdApp 
				from 
					inserted gr 
					left join sec.SecObjects so on gr.idSecObject = so.idSecObject
					left join sec.Roles r on gr.idRole = r.idRole
					left join sec.AccessTypes a on gr.idAccessType = a.idAccessType
			) as extendGrants
			where
				secObjectIdApp <> roleIdApp or secObjectIdApp <> accessTypeIdApp
		)
raiserror(N'appconflict', 16, 1)
");
        }

        public override void Down()
        {
            DropStoredProcedure("sec.DeleteApp");
            DropStoredProcedure("sec.UpdateApp");
            DropStoredProcedure("sec.AddApp");
            DropForeignKey("sec.SecObjects", "IdApplication", "sec.Applications");
            DropForeignKey("sec.Roles", "IdApplication", "sec.Applications");
            DropForeignKey("sec.AccessTypes", "IdApplication", "sec.Applications");
            DropIndex("sec.SecObjects", new[] {"IdApplication"});
            DropIndex("sec.Roles", new[] {"IdApplication"});
            DropIndex("sec.Applications", "UQ_Applications_Name");
            DropIndex("sec.AccessTypes", new[] {"IdApplication"});
            DropTable("sec.Applications");
        }
    }
}
