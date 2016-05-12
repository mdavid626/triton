#
# Author:: Seth Chisamore (<schisamo@chef.io>)
# Cookbook Name:: sql_server
# Recipe:: client
#
# Copyright:: 2011-2016, Chef Software, Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#

# server installer includes client packages
unless node.recipe?('sql_server::server')
  %w( native_client command_line_utils clr_types smo ps_extensions ).each do |pkg|
    windows_package node['sql_server'][pkg]['package_name'] do
      source node['sql_server'][pkg]['url']
      checksum node['sql_server'][pkg]['checksum']
      installer_type :msi
      options "IACCEPTSQLNCLILICENSETERMS=#{node['sql_server']['accept_eula'] ? 'YES' : 'NO'}"
      action :install
    end
  end

  sql_server_version = node['sql_server']['version']
  if sql_server_version =~ /2008/
    install_dir = '100'
  elsif sql_server_version =~ /2012/
    install_dir = '110'
  else
    Chef::Application.fatal!("SQL Server version #{sql_server_version} not supported")
  end

  # update path
  windows_path "#{node['sql_server']['install_dir']}\\#{install_dir}\\Tools\\Binn" do
    action :add
  end
end
