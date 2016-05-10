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

# .NET Framework
include_recipe "dotnetframework"
