#!/bin/bash

openssl req -new -newkey rsa:4096 -nodes -keyout server.key -out server.csr