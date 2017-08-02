namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _17 : DbMigration
    {
        public override void Up()
        {
            Sql(@"alter table sec.Members
add id uniqueidentifier not null rowguidcol,
constraint DF_Members_Id default newsequentialid() for id");

            Sql(@"alter view [sec].[UsersView]
as
select
    m.idMember,
    m.name as login,    
	m.id,
	u.firstName,
    u.lastName,
    u.middleName,
    u.email,
    u.status,
	u.passwordSalt,
    u.dateCreated,
    u.lastActivityDate
from 
    sec.Users u 
        inner join sec.Members m on u.idMember = m.idMember");

            Sql(@"ALTER view [sec].[GroupsView]
as
select
	m.id,
    m.idMember,
	m.name,
    g.description
from 
    sec.Groups g
        inner join sec.Members m on g.idMember = m.idMember");

            Sql(@"ALTER procedure [sec].[AddUser]
	@id uniqueidentifier,
	@login nvarchar(200),
    @firstName nvarchar(200),
    @lastName nvarchar(200),
    @middleName nvarchar(200),
    @email nvarchar(450),
    @status bit,
	@passwordSalt nvarchar(100),
    @dateCreated datetime2,
    @lastActivityDate datetime2
as
set nocount on

declare @idMember int

insert into Members(name) values(@login)
select @idMember = SCOPE_IDENTITY()

if @dateCreated is null
    set @dateCreated = GETDATE()

insert into Users(idMember, firstName, lastName, middleName, email, status, passwordSalt, dateCreated, lastActivityDate) 
    values(@idMember, @firstName, @lastName, @middleName, @email, @status, @passwordSalt, @dateCreated, @lastActivityDate)

select @idMember as idMember");

            Sql(@"ALTER procedure [sec].[UpdateUser]
	@id uniqueidentifier,
	@idMember int,
	@login nvarchar(200),
    @firstName nvarchar(200),
    @lastName nvarchar(200),
    @middleName nvarchar(200),
    @email nvarchar(450),
    @status bit,
	@passwordSalt nvarchar(100),
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
	passwordSalt = @passwordSalt,
    dateCreated = @dateCreated, 
    lastActivityDate = @lastActivityDate 
where 
    idMember = @idMember");

            Sql(@"ALTER procedure [sec].[AddGroup]
	@id uniqueidentifier,
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

ALTER procedure [sec].[UpdateGroup]
	@id uniqueidentifier,
	@idMember int,
	@name nvarchar(200),
	@description nvarchar(max)
as
set nocount on
update sec.Members set name = @name where idMember = @idMember
update sec.Groups set description = @description where idMember = @idMember");
        }

        public override void Down()
        {
            Sql(@"ALTER procedure [sec].[UpdateGroup]
	@idMember int,
	@name nvarchar(200),
	@description nvarchar(max)
as
set nocount on
update sec.Members set name = @name where idMember = @idMember
update sec.Groups set description = @description where idMember = @idMember");

            Sql(@"ALTER procedure [sec].[AddGroup]
	@name nvarchar(200),
	@description nvarchar(max)
as
set nocount on

declare @idMember int

insert into Members(name) values(@name)
select @idMember = SCOPE_IDENTITY()
insert into Groups(idMember, description) values(@idMember, @description)
select @idMember as idMember");

            Sql(@"ALTER procedure [sec].[UpdateUser]
	@idMember int,
	@login nvarchar(200),
    @firstName nvarchar(200),
    @lastName nvarchar(200),
    @middleName nvarchar(200),
    @email nvarchar(450),
    @status bit,
	@passwordSalt nvarchar(100),
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
	passwordSalt = @passwordSalt,
    dateCreated = @dateCreated, 
    lastActivityDate = @lastActivityDate 
where 
    idMember = @idMember");

            Sql(@"ALTER procedure [sec].[AddUser]
	@login nvarchar(200),
    @firstName nvarchar(200),
    @lastName nvarchar(200),
    @middleName nvarchar(200),
    @email nvarchar(450),
    @status bit,
	@passwordSalt nvarchar(100),
    @dateCreated datetime2,
    @lastActivityDate datetime2
as
set nocount on

declare @idMember int

insert into Members(name) values(@login)
select @idMember = SCOPE_IDENTITY()

if @dateCreated is null
    set @dateCreated = GETDATE()

insert into Users(idMember, firstName, lastName, middleName, email, status, passwordSalt, dateCreated, lastActivityDate) 
    values(@idMember, @firstName, @lastName, @middleName, @email, @status, @passwordSalt, @dateCreated, @lastActivityDate)

select @idMember as idMember");

            Sql(@"ALTER view [sec].[GroupsView]
as
select
    m.idMember,
	m.name,
    g.description
from 
    sec.Groups g
        inner join sec.Members m on g.idMember = m.idMember");

            Sql(@"alter view [sec].[UsersView]
as
select
    m.idMember,
    m.name as login,    
	u.firstName,
    u.lastName,
    u.middleName,
    u.email,
    u.status,
	u.passwordSalt,
    u.dateCreated,
    u.lastActivityDate
from 
    sec.Users u 
        inner join sec.Members m on u.idMember = m.idMember");

            Sql(@"alter table sec.Members
drop constraint DF_Members_Id");

            Sql(@"alter table sec.Members
drop column id");
        }
    }
}
