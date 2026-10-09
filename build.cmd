@echo off
setlocal
if not exist "src\StudentsCourse\bin" mkdir "src\StudentsCourse\bin"
"%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /utf8output /target:exe /out:src\StudentsCourse\bin\StudentsCourse.exe /r:System.Web.Extensions.dll src\StudentsCourse\*.cs
exit /b %errorlevel%
