/*Rollback*/
/*Сущность приложения sec.Applications*/


alter table sec.Roles drop constraint FK_Roles_Applications
alter table sec.SecObjects drop constraint FK_SecObjects_Applications
alter table sec.AccessTypes drop constraint FK_AccessTypes_Applications

alter table sec.Roles drop constraint DF_IdApplication_Roles
alter table sec.SecObjects drop constraint DF_IdApplication_SecObjects
alter table sec.AccessTypes drop constraint DF_IdApplication_AccessTypes

/****** Object:  Index [UQ_Role_Name]    Script Date: 14.04.2015 11:08:44 ******/
if exists(select 1 from sys.indexes where name = N'UQ_Role_Name' and OBJECT_NAME(object_id) = N'Roles')
begin
	DROP INDEX [UQ_Role_Name] ON [sec].[Roles];
end

CREATE UNIQUE NONCLUSTERED INDEX [UQ_Role_Name] ON [sec].[Roles]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [UQ_SecObject_ObjectName]    Script Date: 14.04.2015 11:08:44 ******/
if exists(select 1 from sys.indexes where name = N'UQ_SecObject_ObjectName' and OBJECT_NAME(object_id) = N'SecObjects')
begin
	DROP INDEX [UQ_SecObject_ObjectName] ON [sec].[SecObjects];
end

CREATE UNIQUE NONCLUSTERED INDEX [UQ_SecObject_ObjectName] ON [sec].[SecObjects]
(
	[ObjectName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [UQ_SecObject_ObjectName]    Script Date: 14.04.2015 11:08:44 ******/
if exists(select 1 from sys.indexes where name = N'UQ_AccessType_AccessName' and OBJECT_NAME(object_id) = N'AccessTypes')
begin
	DROP INDEX [UQ_AccessType_AccessName] ON [sec].[AccessTypes];
end

CREATE UNIQUE NONCLUSTERED INDEX [UQ_AccessType_AccessName] ON [sec].[AccessTypes]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

alter table sec.Roles drop column idApplication
alter table sec.SecObjects drop column idApplication
alter table sec.AccessTypes drop column idApplication

if exists(select 1 from sys.procedures where name = N'AddApp' and schema_id = SCHEMA_ID(N'sec') and type in (N'P'))
	drop procedure sec.AddApp
go

if exists(select 1 from sys.procedures where name = N'UpdateApp' and schema_id = SCHEMA_ID(N'sec') and type in (N'P'))
	drop procedure sec.UpdateApp
go

if exists(select 1 from sys.procedures where name = N'DeleteApp' and schema_id = SCHEMA_ID(N'sec') and type in (N'P'))
	drop procedure sec.DeleteApp
go

if exists(select 1 from sys.indexes where name = N'UQ_Application_Name' and object_id = OBJECT_ID(N'sec.Applications'))
	drop index UQ_Application_Name on sec.Applications

drop table sec.Applications
go