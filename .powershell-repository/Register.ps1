Write-Host "Register-PSRepository -Name ""PowerShell-Transforming"" -InstallationPolicy Trusted -SourceLocation ""$PSScriptRoot""?";
Write-Host;
Write-Host "Confirm: [y/n] " -ForegroundColor Yellow -NoNewLine;
$confirmed = Read-Host;
Write-Host;
if($confirmed -eq "y") {
    Register-PSRepository -Name "PowerShell-Transforming" -InstallationPolicy Trusted -SourceLocation "$PSScriptRoot";
}
Write-Host "Press any key to exit..." -ForegroundColor Yellow -NoNewline:$true;
Read-Host;