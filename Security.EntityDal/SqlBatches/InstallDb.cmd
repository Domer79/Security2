sqlcmd -i ..\SqlScripts\Database.sql
sqlcmd -q "use SecurityDev;"
sqlcmd -i ..\SqlScripts\Entity_Applications.sql
