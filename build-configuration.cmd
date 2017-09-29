@echo off

%FAKE% %NYX% "target=clean" -st
%FAKE% %NYX% "target=RestoreNugetPackages" -st

IF NOT [%1]==[] (set RELEASE_NUGETKEY="%1")

SET SUMMARY="First OSS project in the open space"
SET DESCRIPTION="First OSS project in the open space"

%FAKE% %NYX% appName=Elders.Cronus.Persistence.CosmosDb appSummary=%SUMMARY% appDescription=%DESCRIPTION% nugetPackageName=Cronus.Persistence.CosmosDb nugetkey=%RELEASE_NUGETKEY%
