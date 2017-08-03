namespace Security.EntityDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER trigger [sec].[OnInsertGrant] on [sec].[Grants]
after INSERT, UPDATE
as
--Триггер проверяет, при добавлении или обновлении (хотя такого никогда не должно быть) записей, 
--что нет ни одной из таковых, в которой бы объект безопасности, роль и тип доступа не принадлежали 
--одному и тому же приложению
if exists(
			select 
				1
			from
			(
				select 
					gr.idSecObject, gr.idRole, gr.idAccessType, so.idApplication secObjectIdApp, r.idApplication roleIdApp, a.idApplication accessTypeIdApp 
				from 
					inserted gr left join sec.SecObjects so 
				on 
					gr.idSecObject = so.idSecObject left join sec.Roles r 
				on 
					gr.idRole = r.idRole left join sec.AccessTypes a 
				on 
					gr.idAccessType = a.idAccessType
			) as extendGrants
			where
				secObjectIdApp <> roleIdApp or secObjectIdApp <> accessTypeIdApp
		)
raiserror(N'appconflict', 16, 1)");
        }
        
        public override void Down()
        {
        }
    }
}
