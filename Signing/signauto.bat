@echo off

:: Remove any existing signature from bootmgfw.efi
signtool.exe remove /s "bootmgfw.efi"

:: Remove any existing signature from auth.exe
signtool.exe remove /s "auth.exe"

:: Sign the bootmgfw.efi file with the PFX password
signtool.exe sign /f "UTKUDORUKBAYRAKTAR.pfx" /p "UTKUDORUKBAYRAKTAR" /fd SHA256 /t http://timestamp.digicert.com /a "bootmgfw.efi"

:: Sign the auth.exe file with the PFX password
signtool.exe sign /f "UTKUDORUKBAYRAKTAR.pfx" /p "UTKUDORUKBAYRAKTAR" /fd SHA256 /t http://timestamp.digicert.com /a "auth.exe"

echo Files signed successfully, and certificate added to Trusted Root store!
pause
