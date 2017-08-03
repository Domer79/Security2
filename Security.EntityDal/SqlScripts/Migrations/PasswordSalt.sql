use ItisOmsSecurity
go

alter table sec.Users
add passwordSalt nvarchar(100) not null,
constraint DF_Users_PasswordSalt default Lower(replace(cast(NEWID() as nvarchar(100)), N'-', N'')) for passwordSalt
go

create unique index UQ_UserProfile_PasswordSalt on sec.Users(passwordSalt)
go

ALTER view [sec].[UsersView]
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
        inner join sec.Members m on u.idMember = m.idMember

GO

ALTER procedure [sec].[AddUser]
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

select @idMember as idMember
GO

ALTER procedure [sec].[UpdateUser]
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
    idMember = @idMember

GO
