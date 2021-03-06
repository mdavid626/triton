#
# Cookbook Name:: cadmus
# Recipe:: web
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
  only_if node['cadmus']['webdeploy']['url'].nil?
end

# User
newuser = node['cadmus']['user']['username']

if node['cadmus']['user']['create'] then
  user newuser do
  	username newuser
  	password node['cadmus']['user']['password']
	only_if newuser.nil?
  end
end

group "IIS_IUSRS" do
  action :modify
  members newuser
  append true
  only_if newuser.nil?
end

# .NET Framework
include_recipe "dotnetframework"
