@echo off
cd /d %~dp0
docker build -f NioZero.AdminWeb/Dockerfile -t niozero/admin-website .
docker build -f NioZero.PublicWeb/Dockerfile -t niozero/public-website .
