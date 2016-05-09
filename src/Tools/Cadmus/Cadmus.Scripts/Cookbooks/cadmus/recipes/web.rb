#
# Cookbook Name:: cadmus
# Recipe:: default
#
# Copyright 2016, Cymric
#
# All rights reserved - Do Not Redistribute
#

include_recipe "dotnetframework"

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
windows_package 'Web Deploy 3.6' do
  source 'http://10.0.0.1:8080/WebDeploy/WebDeploy_amd64_en-US.msi'
  action :install
end
