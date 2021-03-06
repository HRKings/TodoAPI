#!/bin/bash

cd Infrastructure && dotnet ef database update; cd ..

dotnet run --project API/API.csproj --urls "http://0.0.0.0:5000;https://0.0.0.0:5001"