namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            Sql(@"
/****** Object:  Schema [sec]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE SCHEMA [sec]
GO
/****** Object:  UserDefinedFunction [sec].[GetIdentificationMode]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [sec].[GetIdentificationMode]()
returns nvarchar(max)
as
begin
	return sec.GetSettings('identification_mode')
end

GO
/****** Object:  UserDefinedFunction [sec].[GetSettings]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [sec].[GetSettings](@name nvarchar(100))
returns nvarchar(max)
as
begin
	declare @value nvarchar(max)

	select @value = value from sec.Settings where name = @name

	return @value
end


GO
/****** Object:  UserDefinedFunction [sec].[IsAllowById]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [sec].[IsAllowById](@idSecObject int, @idMember int, @idAccessType int)
returns bit
as
begin
    return sec.IsAllowById1(@idSecObject, @idMember, @idAccessType);
end


GO
/****** Object:  UserDefinedFunction [sec].[IsAllowById1]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [sec].[IsAllowById1](@idSecObject int, @idMember int, @idAccessType int)
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
end

GO
/****** Object:  UserDefinedFunction [sec].[IsAllowByName]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [sec].[IsAllowByName](@secObject nvarchar(200), @member nvarchar(200), @accessType nvarchar(300))
returns bit
as
begin
declare @idSecObject int = (select idSecObject from sec.SecObjects where ObjectName = @secObject)
declare @idMember int = (select idMember from sec.Members where name = @member)
declare @idAccessType int = (select idAccessType from sec.AccessTypes where name = @accessType)

return sec.IsAllowById(@idSecObject, @idMember, @idAccessType)
end


GO
/****** Object:  Table [sec].[AccessTypes]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[AccessTypes](
	[idAccessType] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[idAccessType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [sec].[Grants]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Grants](
	[idSecObject] [int] NOT NULL,
	[idRole] [int] NOT NULL,
	[idAccessType] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idSecObject] ASC,
	[idRole] ASC,
	[idAccessType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [sec].[Groups]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Groups](
	[idMember] [int] NOT NULL,
	[description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [sec].[Logs]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Logs](
	[idLog] [int] IDENTITY(1,1) NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[dateCreated] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idLog] ASC
)WITH (PAD_INDEX = ON, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 98) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [sec].[MemberRoles]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[MemberRoles](
	[idMember] [int] NOT NULL,
	[idRole] [int] NOT NULL,
 CONSTRAINT [PK_MemberRole] PRIMARY KEY CLUSTERED 
(
	[idMember] ASC,
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [sec].[Members]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Members](
	[idMember] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [sec].[Roles]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Roles](
	[idRole] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[description] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [sec].[SecObjects]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[SecObjects](
	[idSecObject] [int] IDENTITY(1,1) NOT NULL,
	[ObjectName] [nvarchar](200) NOT NULL,
	[Type] [nvarchar](100) NULL,
	[Discriminator] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[idSecObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [sec].[Settings]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Settings](
	[idSettings] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[value] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[idSettings] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [sec].[UserGroups]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[UserGroups](
	[idUser] [int] NOT NULL,
	[idGroup] [int] NOT NULL,
 CONSTRAINT [PK_UserGroups] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC,
	[idGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [sec].[Users]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Users](
	[idMember] [int] NOT NULL,
	[password] [varbinary](max) NULL,
	[firstName] [nvarchar](200) NOT NULL,
	[lastName] [nvarchar](200) NOT NULL,
	[middleName] [nvarchar](200) NULL,
	[email] [nvarchar](450) NOT NULL,
	[status] [bit] NOT NULL,
	[dateCreated] [datetime2](7) NOT NULL,
	[lastActivityDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  View [sec].[GroupsView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [sec].[GroupsView]
as
select
    m.*,
    g.description
from 
    sec.Groups g
        inner join sec.Members m on g.idMember = m.idMember


GO
/****** Object:  View [sec].[RoleOfMembers]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [sec].[RoleOfMembers]
as
select
	r.idRole,
	r.name roleName,
	r.description roleDescription,
	m.idMember,
	m.name memberName
from
	sec.Roles r inner join sec.MemberRoles mr 
on 
	r.idRole = mr.idRole inner join sec.Members m
on
	mr.idMember = m.idMember


GO
/****** Object:  View [sec].[UsersView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [sec].[UsersView]
as
select
    m.idMember,
    m.name as login,
    u.firstName,
    u.lastName,
    u.middleName,
    u.email,
    u.status,
    u.dateCreated,
    u.lastActivityDate
from 
    sec.Users u 
        inner join sec.Members m on u.idMember = m.idMember


GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_AccessType_AccessName]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_AccessType_AccessName] ON [sec].[AccessTypes]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Group_idMember]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Group_idMember] ON [sec].[Groups]
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Member_Name]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Member_Name] ON [sec].[Members]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Role_Name]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Role_Name] ON [sec].[Roles]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_SecObject_ObjectName]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_SecObject_ObjectName] ON [sec].[SecObjects]
(
	[ObjectName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Settings_Name]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Settings_Name] ON [sec].[Settings]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ_User_idMember]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_User_idMember] ON [sec].[Users]
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Users_Email]    Script Date: 3/14/2017 9:42:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Users_Email] ON [sec].[Users]
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [sec].[Logs] ADD  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [sec].[Users] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [sec].[Users] ADD  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [sec].[Grants]  WITH CHECK ADD  CONSTRAINT [FK_Grants_AccessTypes] FOREIGN KEY([idAccessType])
REFERENCES [sec].[AccessTypes] ([idAccessType])
GO
ALTER TABLE [sec].[Grants] CHECK CONSTRAINT [FK_Grants_AccessTypes]
GO
ALTER TABLE [sec].[Grants]  WITH CHECK ADD  CONSTRAINT [FK_Grants_Roles] FOREIGN KEY([idRole])
REFERENCES [sec].[Roles] ([idRole])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[Grants] CHECK CONSTRAINT [FK_Grants_Roles]
GO
ALTER TABLE [sec].[Grants]  WITH CHECK ADD  CONSTRAINT [FK_Grants_SecObjects] FOREIGN KEY([idSecObject])
REFERENCES [sec].[SecObjects] ([idSecObject])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[Grants] CHECK CONSTRAINT [FK_Grants_SecObjects]
GO
ALTER TABLE [sec].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Groups_Members] FOREIGN KEY([idMember])
REFERENCES [sec].[Members] ([idMember])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[Groups] CHECK CONSTRAINT [FK_Groups_Members]
GO
ALTER TABLE [sec].[MemberRoles]  WITH CHECK ADD  CONSTRAINT [FK_MemberRoles_Members] FOREIGN KEY([idMember])
REFERENCES [sec].[Members] ([idMember])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[MemberRoles] CHECK CONSTRAINT [FK_MemberRoles_Members]
GO
ALTER TABLE [sec].[MemberRoles]  WITH CHECK ADD  CONSTRAINT [FK_MemberRoles_Roles] FOREIGN KEY([idRole])
REFERENCES [sec].[Roles] ([idRole])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[MemberRoles] CHECK CONSTRAINT [FK_MemberRoles_Roles]
GO
ALTER TABLE [sec].[UserGroups]  WITH CHECK ADD  CONSTRAINT [FK_UserGroups_Groups] FOREIGN KEY([idGroup])
REFERENCES [sec].[Groups] ([idMember])
GO
ALTER TABLE [sec].[UserGroups] CHECK CONSTRAINT [FK_UserGroups_Groups]
GO
ALTER TABLE [sec].[UserGroups]  WITH CHECK ADD  CONSTRAINT [FK_UserGroups_Users] FOREIGN KEY([idUser])
REFERENCES [sec].[Users] ([idMember])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[UserGroups] CHECK CONSTRAINT [FK_UserGroups_Users]
GO
ALTER TABLE [sec].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Members] FOREIGN KEY([idMember])
REFERENCES [sec].[Members] ([idMember])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[Users] CHECK CONSTRAINT [FK_Users_Members]
GO
/****** Object:  StoredProcedure [sec].[AddGrant]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [dbo].[Grant_Insert] @IdSecObject=6,@IdRole=1,@IdAccessType=11,@ObjectName=NULL,@RoleName=NULL,@AccessName=NULL
create procedure [sec].[AddGrant]
	@IdSecObject int,
	@IdRole int,
	@IdAccessType int
as
set nocount on
insert into sec.Grants(idSecObject, idRole, idAccessType) values(@IdSecObject, @IdRole, @IdAccessType)


GO
/****** Object:  StoredProcedure [sec].[AddGroup]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[AddGroup]
	@name nvarchar(200),
	@description nvarchar(max)
as
set nocount on

declare @idMember int

insert into Members(name) values(@name)
select @idMember = SCOPE_IDENTITY()
insert into Groups(idMember, description) values(@idMember, @description)
select @idMember as idMember


GO
/****** Object:  StoredProcedure [sec].[AddUser]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[AddUser]
	@login nvarchar(200),
    @firstName nvarchar(200),
    @lastName nvarchar(200),
    @middleName nvarchar(200),
    @email nvarchar(450),
    @status bit,
    @dateCreated datetime2,
    @lastActivityDate datetime2
as
set nocount on

declare @idMember int

insert into Members(name) values(@login)
select @idMember = SCOPE_IDENTITY()

if @dateCreated is null
    set @dateCreated = GETDATE()

insert into Users(idMember, firstName, lastName, middleName, email, status, dateCreated, lastActivityDate) 
    values(@idMember, @firstName, @lastName, @middleName, @email, @status, @dateCreated, @lastActivityDate)

select @idMember as idMember


GO
/****** Object:  StoredProcedure [sec].[DeleteGrant]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[DeleteGrant]
	@IdSecObject int,
	@IdRole int,
	@IdAccessType int
as
set nocount on
delete from sec.Grants where idSecObject = @IdSecObject and idRole = @IdRole and idAccessType = @IdAccessType

GO
/****** Object:  StoredProcedure [sec].[DeleteGroup]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[DeleteGroup]
	@idMember int
as
set nocount on
delete from sec.UserGroups where idGroup = @idMember
delete from sec.Members where idMember = @idMember

GO
/****** Object:  StoredProcedure [sec].[DeleteUser]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[DeleteUser]
	@idMember int
as
set nocount on
delete from sec.Members where idMember = @idMember

GO
/****** Object:  StoredProcedure [sec].[GrantToPublic]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [sec].[GrantToPublic]
    @toPrint BIT = 1
AS
SET NOCOUNT ON
BEGIN

DECLARE @str VARCHAR(4000)
DECLARE @stmt NVARCHAR(MAX)
DECLARE @crlf VARCHAR(50)

SET @stmt = ''
SET @crlf = /*CHAR(13) + 'GO' +*/ CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)

DECLARE grantcursor CURSOR
FOR 
SELECT 'GRANT EXEC ON [' + schm.name + '].[' + p.name + '] TO PUBLIC;' FROM sys.procedures p inner join sys.schemas schm on p.schema_id = schm.schema_id WHERE LOWER(p.name) <> 'granttopublic'
UNION ALL
SELECT 'GRANT SELECT, UPDATE, INSERT, DELETE ON [' + schm.name + '].[' + p.name + '] TO PUBLIC;' FROM sys.tables p inner join sys.schemas schm on p.schema_id = schm.schema_id
UNION ALL
SELECT 'GRANT SELECT ON [' + schm.name + '].[' + p.name + '] TO PUBLIC;' FROM sys.views p inner join sys.schemas schm on p.schema_id = schm.schema_id
UNION ALL
select 'GRANT EXEC ON [' + schm.name + '].[' + p.name + '] TO PUBLIC;' from sys.objects p inner join sys.schemas schm on p.schema_id = schm.schema_id where type_desc = 'SQL_SCALAR_FUNCTION'

    OPEN grantcursor
    FETCH NEXT FROM grantcursor INTO @str
    WHILE @@FETCH_STATUS = 0
    BEGIN
		IF @toPrint = 1
		BEGIN
			IF LEN(@stmt + @str) > 4000
			BEGIN
				PRINT @stmt
				SET @stmt = ''
			END
		END

        SET @stmt += @str
        IF @toPrint = 1
			SET @stmt += @crlf
        FETCH NEXT FROM grantcursor INTO @str
    END

    CLOSE grantcursor
    DEALLOCATE grantcursor

IF @toPrint = 0 
    EXEC sp_executesql @stmt
ELSE
    PRINT @stmt

END

/****** Object:  UserDefinedFunction [sec].[GetIdentificationMode]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON

GO
/****** Object:  StoredProcedure [sec].[SetIdentificationMode]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[SetIdentificationMode]
	@mode nvarchar(100)
as
set nocount on
if not exists(select 1 from sec.Settings where name = 'identification_mode')
	insert into sec.Settings(name, value) values('identification_mode', @mode)
else
	update sec.Settings set value = @mode where name = 'identification_mode'

GO
/****** Object:  StoredProcedure [sec].[SetPassword]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[SetPassword]
    @login nvarchar(200),
    @password varbinary(max)
as
update sec.Users set password = @password where idMember = (select idMember from sec.Members where name = @login)

GO
/****** Object:  StoredProcedure [sec].[UpdateGrant]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateGrant]
	@IdSecObject int,
	@IdRole int,
	@IdAccessType int
as
set nocount on
raiserror('not_modified', 16, 1)


GO
/****** Object:  StoredProcedure [sec].[UpdateGroup]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateGroup]
	@idMember int,
	@name nvarchar(200),
	@description nvarchar(max)
as
set nocount on
update sec.Members set name = @name where idMember = @idMember
update sec.Groups set description = @description where idMember = @idMember


GO
/****** Object:  StoredProcedure [sec].[UpdateUser]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateUser]
	@idMember int,
	@login nvarchar(200),
    @firstName nvarchar(200),
    @lastName nvarchar(200),
    @middleName nvarchar(200),
    @email nvarchar(450),
    @status bit,
    @dateCreated datetime2,
    @lastActivityDate datetime2
as
set nocount on
update sec.Members set name = @login where idMember = @idMember
update 
    sec.Users 
set 
    firstName = @firstName, 
    lastName = @lastName, 
    middleName = @middleName, 
    email = @email, 
    status = @status, 
    dateCreated = @dateCreated, 
    lastActivityDate = @lastActivityDate 
where 
    idMember = @idMember


GO
/****** Object:  Trigger [sec].[OnDeleteGroup]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE trigger [sec].[OnDeleteGroup] on [sec].[Groups]
after delete
as
set nocount on

--Ограничение на удаление записи в sec.Groups, если не удалена запись в sec.Member
if exists(select 1 from sec.Members where idMember in (select idMember from deleted))
	begin
		raiserror('fk_member_error', 16, 10)
		rollback
		return
	end

delete from sec.UserGroups where idGroup in (select idGroup from deleted)


GO
ALTER TABLE [sec].[Groups] ENABLE TRIGGER [OnDeleteGroup]
GO
/****** Object:  Trigger [sec].[OnDeleteUser]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE trigger [sec].[OnDeleteUser] on [sec].[Users]
after delete
as
set nocount on

--Ограничение на удаление записи в sec.Users, если не удалена запись в sec.Member
if exists(select 1 from sec.Members where idMember in (select idMember from deleted))
	begin
		raiserror('fk_member_error', 16, 10)
		rollback
		return
	end


GO
ALTER TABLE [sec].[Users] ENABLE TRIGGER [OnDeleteUser]
GO
/****** Object:  Trigger [sec].[OnDeleteGroupsView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [sec].[OnDeleteGroupsView] on [sec].[GroupsView]
instead of delete
as
delete from sec.UserGroups where idGroup in (select idMember from deleted)
delete from sec.Members where idMember in (select idMember from deleted)

GO
/****** Object:  Trigger [sec].[OnInsertGroupsView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [sec].[OnInsertGroupsView] on [sec].[GroupsView]
instead of insert
as

insert into sec.Members(name) select name from inserted
insert into sec.Groups(idMember, description) select m.idMember, ins.description from Members m inner join inserted ins on m.name = ins.name

GO
/****** Object:  Trigger [sec].[OnUpdateGroupsView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [sec].[OnUpdateGroupsView] on [sec].[GroupsView]
instead of update
as

update sec.Members set name = inserted.name from inserted where inserted.idMember = sec.Members.idMember 
update sec.Groups set description = inserted.description from inserted where inserted.idMember = sec.Groups.idMember

GO
/****** Object:  Trigger [sec].[OnDeleteUsersView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [sec].[OnDeleteUsersView] on [sec].[UsersView]
instead of delete
as

delete from sec.Members where idMember in (select idMember from deleted)

GO
/****** Object:  Trigger [sec].[OnInsertUsersView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [sec].[OnInsertUsersView] on [sec].[UsersView]
instead of insert
as

insert into sec.Members(name) select login from inserted
insert into sec.Users(idMember, firstName, lastName, middleName, email, status, dateCreated, lastActivityDate) 
    select 
        m.idMember, 
        ins.firstName, 
        ins.lastName, 
        ins.middleName, 
        ins.email, 
        ins.status, 
        ins.dateCreated, 
        ins.lastActivityDate 
    from 
        Members m inner join inserted ins 
    on 
        m.name = ins.login

select SCOPE_IDENTITY()

GO
/****** Object:  Trigger [sec].[OnUpdateUsersView]    Script Date: 3/14/2017 9:42:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create trigger [sec].[OnUpdateUsersView] on [sec].[UsersView]
instead of update
as

update sec.Members set name = inserted.login from inserted where inserted.idMember = sec.Members.idMember 
update sec.Users set 
    firstName =  ins.firstName, 
    lastName =  ins.lastName, 
    middleName =  ins.middleName, 
    email =  ins.email, 
    status =  ins.status, 
    dateCreated =  ins.dateCreated, 
    lastActivityDate = ins.lastActivityDate
from 
    inserted ins 
where ins.idMember = sec.Users.idMember

");
        }
        
        public override void Down()
        {
            DropStoredProcedure("sec.DeleteUser");
            DropStoredProcedure("sec.UpdateUser");
            DropStoredProcedure("sec.AddUser");
            DropStoredProcedure("sec.DeleteGroup");
            DropStoredProcedure("sec.UpdateGroup");
            DropStoredProcedure("sec.AddGroup");
            DropStoredProcedure("sec.DeleteGrant");
            DropStoredProcedure("sec.UpdateGrant");
            DropStoredProcedure("sec.AddGrant");
//            DropStoredProcedure("sec.DeleteApp");
//            DropStoredProcedure("sec.UpdateApp");
//            DropStoredProcedure("sec.AddApp");
            DropForeignKey("sec.UserGroups", "idGroup", "sec.GroupsView");
            DropForeignKey("sec.UserGroups", "idUser", "sec.UsersView");
            DropForeignKey("sec.Grants", "IdAccessType", "sec.AccessTypes");
//            DropForeignKey("sec.SecObjects", "IdApplication", "sec.Applications");
//            DropForeignKey("sec.Roles", "IdApplication", "sec.Applications");
            DropForeignKey("sec.MemberRoles", "idRole", "sec.Roles");
            DropForeignKey("sec.MemberRoles", "idMember", "sec.Members");
            DropForeignKey("sec.Grants", "IdRole", "sec.Roles");
            DropForeignKey("sec.Grants", "IdSecObject", "sec.SecObjects");
//            DropForeignKey("sec.AccessTypes", "IdApplication", "sec.Applications");
            DropIndex("sec.UserGroups", new[] { "idGroup" });
            DropIndex("sec.UserGroups", new[] { "idUser" });
            DropIndex("sec.MemberRoles", new[] { "idRole" });
            DropIndex("sec.MemberRoles", new[] { "idMember" });
            DropIndex("sec.Settings", "UQ_Settings_Name");
            DropIndex("sec.UsersView", "UQ_UserProfile_Email");
            DropIndex("sec.Members", "UQ_Member_Name");
//            DropIndex("sec.SecObjects", new[] { "IdApplication" });
            DropIndex("sec.SecObjects", "UQ_SecObject_ObjectName");
            DropIndex("sec.Grants", new[] { "IdAccessType" });
            DropIndex("sec.Grants", new[] { "IdRole" });
            DropIndex("sec.Grants", new[] { "IdSecObject" });
//            DropIndex("sec.Roles", new[] { "IdApplication" });
            DropIndex("sec.Roles", "UQ_Role_Name");
//            DropIndex("sec.Applications", "UQ_Applications_Name");
//            DropIndex("sec.AccessTypes", new[] { "IdApplication" });
            DropIndex("sec.AccessTypes", "UQ_AccessType_AccessName");
            DropTable("sec.UserGroups");
            DropTable("sec.MemberRoles");
            DropTable("sec.Settings");
            DropTable("sec.Logs");
            DropView("sec.UsersView");
            DropView("sec.GroupsView");
            DropTable("sec.Members");
            DropTable("sec.SecObjects");
            DropTable("sec.Grants");
            DropTable("sec.Roles");
//            DropTable("sec.Applications");
            DropTable("sec.AccessTypes");
        }

        public void DropView(string viewName)
        {
            Sql($"Drop view {viewName}");
        }
    }
}
