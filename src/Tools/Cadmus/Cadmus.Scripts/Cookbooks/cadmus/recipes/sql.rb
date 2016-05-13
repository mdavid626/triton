#
# Cookbook Name:: cadmus
# Recipe:: sql
#
# Copyright 2016, Cymric
#
# All rights reserved - Do Not Redistribute
#

remote_file File.join(Chef::Config[:file_cache_path], 'sources.zip') do
  source node['sql_server']['server']['dotnetsources']
  not_if { ::File.exist?('C:/Windows/Microsoft.NET/Framework/v3.5') }
end

powershell_script 'unzip' do
  cwd Chef::Config[:file_cache_path]
  not_if { ::File.exist?('C:/Windows/Microsoft.NET/Framework/v3.5') }
  code <<-EOH
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  [System.IO.Compression.ZipFile]::ExtractToDirectory('sources.zip', 'sources')
  EOH
end

reboot 'Restart Computer' do
  action :nothing
  reason '.NET 3.5 install'
end

windows_feature 'NetFx3' do
  action :install
  all true
  source File.join(Chef::Config[:file_cache_path], 'sources')
  not_if { ::File.exist?('C:/Windows/Microsoft.NET/Framework/v3.5') }
  notifies :reboot_now, 'reboot[Restart Computer]', :immediately
end

# SQL package
remote_file File.join(Chef::Config[:file_cache_path], 'sql.zip') do
  source node['sql_server']['server']['installerurl']
  not_if { ::File.exist?(node['sql_server']['instance_dir']) }
end

powershell_script 'unzip' do
  cwd Chef::Config[:file_cache_path]
  not_if { ::File.exist?(node['sql_server']['instance_dir']) }
  code <<-EOH
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  [System.IO.Compression.ZipFile]::ExtractToDirectory('sql.zip', 'sql')
  EOH
end

# Firewall Rule
powershell_script 'Firewall Rule' do
  code <<-EOH
  if (-Not (Get-NetFirewallRule -DisplayName 'SQL Port'))
  {
	New-NetFirewallRule -DisplayName 'SQL Port' -Direction Inbound -Protocol TCP -Action Allow -LocalPort #{node['sql_server']['port']} 
  }
  EOH
end

include_recipe 'sql_server::server'
include_recipe "dotnetframework"