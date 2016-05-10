#
# Cookbook Name:: cadmus
# Recipe:: default
#
# Copyright 2016, Cymric
#
# All rights reserved - Do Not Redistribute
#

# Server Roles
windows_feature 'IIS-WebServerRole' do
  action :install
  all true
end

# Role services
windows_feature 'IIS-ASPNET45' do
  action :install
  all true
end

# Web Deploy
windows_package 'Web Deploy' do
  source node['cadmus']['webdeploy']['url']
  action :install
end

# WMF 5.0
powershell_script 'install-wmf5' do
  code <<-EOH
  $hotfix = Get-HotFix -Id KB3134758
  if (-Not $hotfix) {
      wget http://10.0.0.1:8080/MSU/Win8.1AndW2K12R2-KB3134758-x64.msu -OutFile package.msu
      $path = (Get-ChildItem package.msu).FullName
      $args = @('/install', "$path", '/quiet', '/norestart')   
      $code = Start-Process wusa.exe -NoNewWindow -Wait -ArgumentList $args -PassThru -ErrorAction Stop
	  Remove-Item -Path package.msu -Force
      if ($code.ExitCode -ne 3010) {
          throw "Instalation failed"   
      }
  }
  EOH
end

# .NET Framework
include_recipe "dotnetframework"

# User
newuser = node['cadmus']['user']['username']
if node['cadmus']['user']['create'] then
  user newuser do
  	username newuser
  	password node['cadmus']['user']['password']
  end
  
  group "IIS_IUSRS" do
  	action :modify
  	members newuser
  	append true
  end
end
