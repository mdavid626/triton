#
# Cookbook Name:: cadmus
# Recipe:: sql
#
# Copyright 2016, Cymric
#
# All rights reserved - Do Not Redistribute
#

remote_file File.join(Chef::Config[:file_cache_path], 'sources.zip') do
  source "http://10.0.0.1:8080/Windows_Server_2012_R2/sources.zip"
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


