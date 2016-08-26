@echo off
setlocal

:: Check prerequisites
set _msbuildexe="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%ProgramFiles%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% echo Error: Could not find MSBuild.exe.  Please see https://github.com/Microsoft/vipr/wiki/Developer%%20Guide for build instructions. && exit /b 2

:: Log build command line
set _buildprefix=echo
set _buildpostfix=^> "%~dp0build.log"
call :build %*

:: Build
set _buildprefix=
set _buildpostfix=
call :build %*

goto :AfterBuild

:build
%_buildprefix% %_msbuildexe% "%~dp0build.proj" /nologo /maxcpucount /verbosity:minimal /nodeReuse:false /fileloggerparameters:Verbosity=diag;LogFile="%~dp0build.log";Append %* %_buildpostfix%
set BUILDERRORLEVEL=%ERRORLEVEL%
goto :eof

:AfterBuild

echo.
:: Pull the build summary from the log file
findstr /ir /c:".*Warning(s)" /c:".*Error(s)" /c:"Time Elapsed.*" "%~dp0build.log"
echo Build Exit Code = %BUILDERRORLEVEL%

exit /b %BUILDERRORLEVEL%