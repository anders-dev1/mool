#!/bin/bash
set -e

npm install -g nswag
dotnet build ../backend/mool/API //t:nswag
nswag swagger2tsclient /WithCredentials:true /ExtensionCode:src/api/api-base.ts /ClientBaseClass:ApiBase /Input:../backend/mool/API/swagger.json /Output:src/api/api-client.ts