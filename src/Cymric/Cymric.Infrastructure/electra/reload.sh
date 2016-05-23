#!/bin/bash

sudo cp /mnt/vagrant/cert/cymric_me.key /etc/nginx/cert.key
sudo cp /mnt/vagrant/cert/cymric_me.crt /etc/nginx/cert.crt
sudo cp /mnt/vagrant/cert/teamcity.key /etc/nginx/teamcity.key
sudo cp /mnt/vagrant/cert/teamcity.crt /etc/nginx/teamcity.crt
sudo cp /mnt/vagrant/cert/dhparam.pem /etc/ssl/certs/dhparam.pem

sudo cp /mnt/vagrant/default /etc/nginx/sites-available/default
sudo cp /mnt/vagrant/index.html /usr/share/nginx/html
sudo cp /mnt/vagrant/icons/favicon.ico /usr/share/nginx/html
sudo nginx -s reload