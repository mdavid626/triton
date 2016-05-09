#
# Cookbook Name:: cadmus
# Recipe:: default
#
# Copyright 2016, Cymric
#
# All rights reserved - Do Not Redistribute
#

file 'c:\\test.txt' do
  content '<html>This is a placeholder for the home page.</html>'
  action :create
end