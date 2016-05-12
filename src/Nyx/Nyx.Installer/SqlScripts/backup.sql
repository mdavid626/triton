declare @backupFolder nvarchar(max)
declare @dbName nvarchar(max)
declare @defaultBackupDir nvarchar(4000)
declare @cmd nvarchar(max)
declare @timestamp nvarchar(max)
declare @date datetime
declare @targetPath nvarchar(max)

EXEC master.dbo.xp_instance_regread 
            N'HKEY_LOCAL_MACHINE', 
            N'Software\Microsoft\MSSQLServer\MSSQLServer',N'BackupDirectory', 
            @defaultBackupDir OUTPUT,  
            'no_output' 

set @dbName=db_name()
set @backupFolder=@defaultBackupDir

if substring(@backupFolder, len(@backupFolder),1)<>'\' set @backupFolder=@backupFolder+'\'

set @date=getdate()
set @timestamp = cast(YEAR(@date) as varchar(4)) + right('0' + cast(MONTH(@date) AS varchar(2)),2) + right('0' + cast(DAY(getdate()) as varchar(2)),2) + cast(cast(@date as time) as varchar(2)) + substring(cast(cast(@date as time) as varchar(5)), 4,2) + substring(cast(cast(getdate() as time) as varchar(8)), 7,2)
set @targetPath = @backupFolder + @dbName + '_' + @timestamp + '.bak'

set @cmd=N'BACKUP DATABASE [' + @dbName + '] TO  DISK = ''' + @targetPath + ''' WITH COPY_ONLY, NOFORMAT, INIT, NAME = N''' +
           @dbName + '-Full Database Backup''' + ', SKIP, NOREWIND, NOUNLOAD,  STATS = 10, CHECKSUM'

print('DB name=' + @dbName)
print('Target Path=' + @targetPath)
print('Executing backup procedure...')

print(@cmd)
exec (@cmd)

-- verify
declare @backupSetId as int
declare @msg nvarchar(4000)

select @backupSetId = position from msdb..backupset where database_name=@dbName and backup_set_id=(select max(backup_set_id) from msdb..backupset where database_name=@dbName)

if @backupSetId is null 
begin
    set @msg=N'Verify failed. Backup information for database ''' + @dbName + ''' not found.'
	raiserror(@msg, 16, 1)
end

RESTORE VERIFYONLY FROM  DISK = @targetPath WITH  FILE = @backupSetId,  NOUNLOAD,  NOREWIND