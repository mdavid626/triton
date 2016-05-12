default['sql_server']['accept_eula'] = true
default['sql_server']['product_key'] = '82YJF-9RP6B-YQV9M-VXQFR-YJBGX'
default['sql_server']['version'] = '2014'

default['sql_server']['install_dir']    = 'C:\Program Files\Microsoft SQL Server'
default['sql_server']['port'] = 1433

default['sql_server']['instance_name'] = 'MSSQLSERVER'
default['sql_server']['instance_dir']   = 'C:\Program Files\Microsoft SQL Server'
default['sql_server']['shared_wow_dir'] = 'C:\Program Files (x86)\Microsoft SQL Server'
default['sql_server']['feature_list'] = 'SQLENGINE,SNAC_SDK'
default['sql_server']['agent_account'] =  'NT AUTHORITY\NETWORK SERVICE'
default['sql_server']['agent_startup'] =  'Disabled'
default['sql_server']['rs_mode'] = 'FilesOnlyMode'
default['sql_server']['rs_account'] = 'NT AUTHORITY\NETWORK SERVICE'
default['sql_server']['rs_startup'] = 'Automatic'
default['sql_server']['browser_startup'] = 'Disabled'
default['sql_server']['sysadmins'] = ['Administrator']
default['sql_server']['sql_account'] = 'NT AUTHORITY\NETWORK SERVICE'
default['sql_server']['update_enabled'] = true # applies to SQL Server 2012 and later

default['sql_server']['server']['installer_timeout'] = 1500

# Set these to specify the URL, checksum, and package name. Otherwise, the cookbook will
# use default values based on the value of node['sql_server']['version'] and the
# server architecture (x86 or x64).
default['sql_server']['server']['url'] = File.join(Chef::Config[:file_cache_path], "sql\\setup.exe")
default['sql_server']['server']['checksum'] = '46EDC61D2B4AA18E6D1DD7AB8DFDF6319B9DFC2EE86B1B3062E45317376006D2'
default['sql_server']['server']['package_name'] = 'setup.exe'
