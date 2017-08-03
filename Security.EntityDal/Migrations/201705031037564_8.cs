namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER view [sec].[RoleOfMembers]
as
select
	r.idRole,
	r.name roleName,
	r.description roleDescription,
	r.idApplication,
	r.appName,
	m.idMember,
	m.name memberName
from
	(
	select  
		r.idRole,
		r.name,
		r.description,
		a.idApplication,
		a.appName
	from sec.Roles r inner join sec.Applications a on r.idApplication = a.idApplication) r 
	inner join sec.MemberRoles mr 
on 
	r.idRole = mr.idRole inner join sec.Members m
on
	mr.idMember = m.idMember");
        }
        
        public override void Down()
        {
            Sql(@"ALTER view [sec].[RoleOfMembers]
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
	mr.idMember = m.idMember");
        }
    }
}
