#
# Cookbook Name:: cadmus
# Recipe:: default
#
# Copyright 2016, Cymric
#
# All rights reserved - Do Not Redistribute
#

windows_feature 'IIS-WebServerRole' do
  action :install
  all true
end

windows_feature 'IIS-ASPNET45' do
  action :install
  all true
end