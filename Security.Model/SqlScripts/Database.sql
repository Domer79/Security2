USE [master]
GO

:setvar DatabaseName "SecurityDev"

/*
Проверьте режим SQLCMD и отключите выполнение скрипта, если режим SQLCMD не поддерживается.
Чтобы повторно включить скрипт после включения режима SQLCMD выполните следующую инструкцию:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'Для успешного выполнения этого скрипта должен быть включен режим SQLCMD.';
        SET NOEXEC ON;
    END
GO

if DB_ID(N'$(DatabaseName)') is not null
	BEGIN
		PRINT N'База данных $(DatabaseName) уже существует'
		SET NOEXEC ON;
		ROLLBACK;
	END

/****** Object:  Database [$(DatabaseName)]    Script Date: 14.04.2015 11:08:44 ******/
CREATE DATABASE [$(DatabaseName)]
GO
ALTER DATABASE [$(DatabaseName)] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [$(DatabaseName)].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [$(DatabaseName)] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET ARITHABORT OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [$(DatabaseName)] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [$(DatabaseName)] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [$(DatabaseName)] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [$(DatabaseName)] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET  DISABLE_BROKER 
GO
ALTER DATABASE [$(DatabaseName)] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [$(DatabaseName)] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET RECOVERY FULL 
GO
ALTER DATABASE [$(DatabaseName)] SET  MULTI_USER 
GO
ALTER DATABASE [$(DatabaseName)] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [$(DatabaseName)] SET DB_CHAINING OFF 
GO
ALTER DATABASE [$(DatabaseName)] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [$(DatabaseName)] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'$(DatabaseName)', N'ON'
GO
USE [$(DatabaseName)]
GO

/* Для внедрения в существующую базу данных копировать отсюда */

/****** Object:  Schema [sec]    Script Date: 14.04.2015 11:08:44 ******/
CREATE SCHEMA [sec]
GO
/****** Object:  StoredProcedure [sec].[AddGrant]    Script Date: 14.04.2015 11:08:44 ******/
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
/****** Object:  StoredProcedure [sec].[AddGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[AddGroup]
	@name varchar(200),
	@description varchar(max)
as
set nocount on

declare @idMember int

insert into Members(name) values(@name)
select @idMember = SCOPE_IDENTITY()
insert into Groups(idMember, description) values(@idMember, @description)
select @idMember

GO
/****** Object:  StoredProcedure [sec].[AddUser]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [sec].[AddUser]
	@login varchar(200),
	@password varbinary(16)
as
set nocount on

declare @idMember int

insert into Members(name, isUser) values(@login, 1)
select @idMember = SCOPE_IDENTITY()
insert into Users(idMember, password) values(@idMember, @password)
select @idMember

GO
/****** Object:  StoredProcedure [sec].[DeleteGrant]    Script Date: 14.04.2015 11:08:44 ******/
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
/****** Object:  StoredProcedure [sec].[DeleteGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[DeleteGroup]
	@idMember int
as
set nocount on
delete from sec.Members where idMember = @idMember
GO
/****** Object:  StoredProcedure [sec].[DeleteUser]    Script Date: 14.04.2015 11:08:44 ******/
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
/****** Object:  StoredProcedure [sec].[SetIdentificationMode]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[SetIdentificationMode]
	@mode varchar(100)
as
set nocount on
if not exists(select 1 from sec.Settings where name = 'identification_mode')
	insert into sec.Settings(name, value) values('identification_mode', @mode)
else
	update sec.Settings set value = @mode where name = 'identification_mode'
GO
/****** Object:  StoredProcedure [sec].[UpdateGrant]    Script Date: 14.04.2015 11:08:44 ******/
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
/****** Object:  StoredProcedure [sec].[UpdateGroup]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateGroup]
	@idMember int,
	@name varchar(200),
	@description varchar(max)
as
set nocount on
update sec.Members set name = @name where idMember = @idMember
update sec.Groups set description = @description where idMember = @idMember

GO
/****** Object:  StoredProcedure [sec].[UpdateUser]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [sec].[UpdateUser]
	@idMember int,
	@login varchar(200),
	@password varbinary(16)
as
set nocount on
update sec.Members set name = @login where idMember = @idMember
update sec.Users set password = @password where idMember = @idMember

GO
/****** Object:  UserDefinedFunction [sec].[GetIdentificationMode]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [sec].[GetIdentificationMode]()
returns varchar(max)
as
begin
	return sec.GetSettings('identification_mode')
end
GO
/****** Object:  UserDefinedFunction [sec].[GetSettings]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [sec].[GetSettings](@name varchar(100))
returns varchar(max)
as
begin
	declare @value varchar(max)

	select @value = value from sec.Settings where name = @name

	return @value
end

GO
/****** Object:  UserDefinedFunction [sec].[IsAllowById]    Script Date: 25.04.2015 2:54:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create function [sec].[IsAllowById](@idSecObject int, @idMember int, @idAccessType int)
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

	--Проверяем доступ по списку всех групп, в которых состоит пользователь, не забывая при вклювить самого пользователя в этот список
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
					and rof.idMember in (select idGroup from sec.UserGroups where idUser = @idMember union all select @idMember)
					and idAccessType = @idAccessType
			)
	begin
		set @result = 1
	end

	return @result
end

GO
/****** Object:  UserDefinedFunction [sec].[IsAllowByName]    Script Date: 25.04.2015 2:54:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE function [sec].[IsAllowByName](@secObject varchar(200), @member varchar(200), @accessType varchar(300))
returns bit
as
begin
declare @idSecObject int = (select idSecObject from sec.SecObjects where ObjectName = @secObject)
declare @idMember int = (select idMember from sec.Members where name = @member)
declare @idAccessType int = (select idAccessType from sec.AccessTypes where name = @accessType)

return sec.IsAllowById(@idSecObject, @idMember, @idAccessType)
end

GO
/****** Object:  Table [sec].[Grants]    Script Date: 14.04.2015 11:08:44 ******/
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
/****** Object:  Table [sec].[Groups]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[Groups](
	[idMember] [int] NOT NULL PRIMARY KEY,
	[description] [varchar](max) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sec].[Roles]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[Roles](
	[idRole] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NOT NULL,
	[description] [varchar](max) NULL
PRIMARY KEY CLUSTERED 
(
	[idRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sec].[Users]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[Users](
	[idMember] [int] NOT NULL PRIMARY KEY,
	--[displayName] [varchar](200) NULL,
	--[email] [varchar](300) NULL,
	[password] [varbinary](max) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sec].[AccessTypes]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[AccessTypes](
	[idAccessType] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[idAccessType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sec].[Members]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[Members](
	[idMember] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NOT NULL,
	[isUser] bit not null

PRIMARY KEY CLUSTERED 
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [sec].[MemberRoles]    Script Date: 14.04.2015 11:08:44 ******/
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
/****** Object:  Table [sec].[SecObjects]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[SecObjects](
	[idSecObject] [int] IDENTITY(1,1) NOT NULL,
	[ObjectName] [varchar](200) NOT NULL,
	[Type] [varchar](100) NULL,
	[Discriminator] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[idSecObject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sec].[Settings]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [sec].[Settings](
	[idSettings] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[value] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[idSettings] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [sec].[UserGroups]    Script Date: 14.04.2015 11:08:44 ******/
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

/****** Object:  View [sec].[RoleOfMember]    Script Date: 14.04.2015 11:08:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create table sec.Logs
(
	idLog int not null primary key identity,
	message nvarchar(max) not null
)
go

CREATE view [sec].[RoleOfMembers]
as
select
	r.idRole,
	r.name roleName,
	r.description roleDescription,
	m.idMember,
	m.name memberName,
	m.isUser
from
	sec.Roles r inner join sec.MemberRoles mr 
on 
	r.idRole = mr.idRole inner join sec.Members m
on
	mr.idMember = m.idMember

GO


/****** Object:  Index [UQ_Group_idMember]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Group_idMember] ON [sec].[Groups]
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Role_Name]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Role_Name] ON [sec].[Roles]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UQ_User_idMember]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_User_idMember] ON [sec].[Users]
(
	[idMember] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_AccessType_AccessName]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_AccessType_AccessName] ON [sec].[AccessTypes]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Member_Name]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Member_Name] ON [sec].[Members]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_SecObject_ObjectName]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_SecObject_ObjectName] ON [sec].[SecObjects]
(
	[ObjectName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Settings_Name]    Script Date: 14.04.2015 11:08:44 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Settings_Name] ON [sec].[Settings]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
ALTER TABLE [sec].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Members] FOREIGN KEY([idMember])
REFERENCES [sec].[Members] ([idMember])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[Users] CHECK CONSTRAINT [FK_Users_Members]
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
/****** Object:  Trigger [sec].[OnDeleteGroup]    Script Date: 14.04.2015 11:08:44 ******/
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

GO
/****** Object:  Trigger [sec].[OnDeleteGroup]    Script Date: 29.04.2015 8:51:56 ******/
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


USE [master]
GO
ALTER DATABASE [$(DatabaseName)] SET  READ_WRITE 
GO

--use master
--go

--drop database [$(DatabaseName)]