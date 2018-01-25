@echo off
setlocal enableextensions disabledelayedexpansion
set search=__TARGET__PATH__
set replace="%cd%\ScreenLocker.exe"
set textFile=ScreenLocker.xml
for /f "delims=" %%i in ('type "%textFile%" ^& break ^> "%textFile%" ') do (
    set "line=%%i"
    setlocal enabledelayedexpansion
    >>"%textFile%" echo(!line:%search%=%replace%!
    endlocal
)
schtasks /create /tn ScreenLocker /xml "%cd%\ScreenLocker.xml"
del /f /q ScreenLocker.xml
del /f /q run.bat