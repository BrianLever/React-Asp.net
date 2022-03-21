@echo off
set scriptPath=%~dp0

echo ****************************************************************************
echo "Restoring nuget packages..."
echo ****************************************************************************

echo "Restoring nuget packages..."
nuget.exe restore "%scriptPath%..\Screendox.sln"