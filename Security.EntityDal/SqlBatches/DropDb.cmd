sqlcmd -q "alter database SecurityDev set single_user with rollback immediate; drop database SecurityDev; return;"
exit