set PATH=C:\Program Files\Java\jdk1.7.0_75\bin;%PATH%

:: Code in a GitHub repo is checked out to %KOKORO_ARTIFACTS_DIR%\github.
:: The final directory name in this path is determined by the scm name specified
:: in the job configuration.
cd %KOKORO_ARTIFACTS_DIR%\github\windows-storage-scripts

call build.bat
exit %ERRORLEVEL%
