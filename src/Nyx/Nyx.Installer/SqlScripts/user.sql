IF (NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '$(Username)'))
BEGIN
    PRINT('Creating login $(Username)')
	if ($(Windows) = 0)
		CREATE LOGIN [$(Username)] WITH PASSWORD=N'$(Password)', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
	else
		CREATE LOGIN [$(Username)] FROM WINDOWS WITH DEFAULT_DATABASE=[master]
END

GO

IF (NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '$(Username)'))
BEGIN
    PRINT ('Creating user $(Username)')
	CREATE USER [$(Username)] FOR LOGIN [$(Username)]
END

GO

IF (IS_ROLEMEMBER('db_datareader', '$(Username)') = 0)
BEGIN
	PRINT ('Adding user $(Username) to role db_datareader')
	ALTER ROLE [db_datareader] ADD MEMBER [$(Username)]
END