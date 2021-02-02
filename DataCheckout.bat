@echo off
SET SVN=C:\Program Files\TortoiseSVN\bin
SET SVN_URL=https://svn.riouxsvn.com/project-alphine

"%SVN%\TortoiseProc.exe" /command:checkout /path:"%CD%/Assets/Resources" /url:"%SVN_URL%"