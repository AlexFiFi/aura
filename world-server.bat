@echo off
rem Aura
rem WorldServer start up script
rem -------------------------------------------------------------------------
rem This script looks for the WorldServer executable in bin\ and its
rem sub-folders (Debug and Release) in the following order, and starts
rem the first one it finds:
rem bin\ > bin\Release\ > bin\Debug\
rem -------------------------------------------------------------------------

set FILENAME=WorldServer

rem Check for a build in bin\ first
if not exist bin\%FILENAME%.exe goto SUB_RELEASE
set PATH=bin\
goto RUN

rem Huh, maybe there's a build in bin\Release?
:SUB_RELEASE
IF NOT EXIST bin\Release\%FILENAME%.exe GOTO SUB_DEBUG
set PATH=bin\Release\
goto RUN

rem Mah... come here debug!
:SUB_DEBUG
IF NOT EXIST bin\Debug\%FILENAME%.exe GOTO ERROR
set PATH=bin\Debug\

rem Go, go, go!
:RUN
echo Running %FILENAME% from %PATH%
%windir%\system32\ping -n 2 127.0.0.1 > nul
cls
cd %PATH%
%FILENAME%.exe
exit

rem Now I'm a saaad panda qq
:ERROR
echo Couldn't find %FILENAME%.exe
pause
