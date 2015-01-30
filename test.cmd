@echo off
setlocal

:: Check prerequisites
set _msbuildexe="%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%ProgramFiles%\MSBuild\12.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% set _msbuildexe="%ProgramFiles%\MSBuild\14.0\Bin\MSBuild.exe"
if not exist %_msbuildexe% echo Error: Could not find MSBuild.exe.  Please see https://github.com/Microsoft/vipr/wiki/Developer%%20Guide for test instructions. && exit /b 2

:: Log test command line
set _testprefix=echo
set _testpostfix=^> "%~dp0test.log"
call :test %*

:: Test
set _testprefix=
set _testpostfix=
call :test %*

goto :AfterTest

:test
%_testprefix% %_msbuildexe% "%~dp0test.proj" /nologo /maxcpucount /verbosity:minimal /nodeReuse:false /fileloggerparameters:Verbosity=diag;LogFile="%~dp0test.log";Append %* %_testpostfix%
set TESTERRORLEVEL=%ERRORLEVEL%
goto :eof

:AfterTest

echo.
:: Pull the test summary from the log file
findstr /ir /c:".*Warning(s)" /c:".*Error(s)" /c:"Time Elapsed.*" "%~dp0test.log"
echo Test Exit Code = %TESTERRORLEVEL%

exit /b %TESTERRORLEVEL%