use IPPS
go

begin try
begin transaction

create table #secUsersView
(
	login nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS,
    firstName nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS,
    lastName nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS,
    middleName nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS,
    email nvarchar(450) collate SQL_Latin1_General_CP1_CI_AS,
    status bit,
    dateCreated datetime2,
    lastActivityDate datetime2
) 

insert into #secUsersView select login, firstName, lastName, middleName, email, status, dateCreated, lastActivityDate from ItisOmsSecurity.sec.UsersView

declare @ippsUsers table(
	id uniqueidentifier not null,
	login nvarchar(max) collate Cyrillic_General_CI_AS,
	Password varbinary(max),
	FirstName nvarchar(max) collate Cyrillic_General_CI_AS,
	SurName nvarchar(max) collate Cyrillic_General_CI_AS,
	Email nvarchar(max) collate Cyrillic_General_CI_AS,
	status bit,
	PasswordSalt nvarchar(max),
	DateCreated datetime
)

declare @id uniqueidentifier
declare @login nvarchar(max)
declare @Password varbinary(max)
declare @FirstName nvarchar(max)
declare @SurName nvarchar(max)
declare @Email nvarchar(max)
declare @status bit
declare @passwordSalt nvarchar(max)
declare @DateCreated datetime

declare ippsUsersCursor cursor for select users.id, users.Login, Password, FirstName, SurName, Email, case Status when 20 then cast(1 as bit) else cast(0 as bit) end status, PasswordSalt, DateCreated from Users right join (select Login from Users where Status not in (40) group by Users.Login) grouppedUsers on Users.Login = grouppedUsers.Login where Status not in (40) and users.Login not in (select Login from #secUsersView) order by users.Login COLLATE Cyrillic_General_CI_AS
open ippsUsersCursor
fetch next from ippsUsersCursor into @id, @login, @password, @FirstName, @SurName, @Email, @status, @passwordSalt, @DateCreated
while @@FETCH_STATUS = 0
begin
if not exists(select 1 from @ippsUsers where login = @login)
begin
	if @Email is null or exists(select 1 from @ippsUsers where Email = @Email)
		set @Email = @login + '@it.ru'

	insert into @ippsUsers values(@id, @login, @Password, @FirstName, @SurName, @Email, @status, @passwordSalt, @DateCreated)
end

fetch next from ippsUsersCursor into @id, @login, @password, @FirstName, @SurName, @Email, @status, @passwordSalt, @DateCreated
end

close ippsUsersCursor
deallocate ippsUsersCursor

drop table #secUsersView

/*DEBUG*/--select * from @ippsUsers

insert into ItisOmsSecurity.sec.Members(id, name) select id, login from @ippsUsers

insert into ItisOmsSecurity.sec.Users(idMember, password, firstName, lastName, middleName, email, status, passwordSalt, dateCreated) 
	select idMember, Password, FirstName, SurName, null, Email, status, PasswordSalt, DateCreated from ItisOmsSecurity.sec.Members m inner join @ippsUsers u on m.name = u.login

/*DEBUG*/--select * from ItisOmsSecurity.sec.UsersView
/*DEBUG*/--rollback

/*NOT DEBUG*/commit
end try
begin catch
rollback
select ERROR_MESSAGE() errorMessage, ERROR_LINE() lineNumber

end catch

/*NOT DEBUG*/select * from ItisOmsSecurity.sec.UsersView