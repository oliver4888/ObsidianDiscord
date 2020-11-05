Add-Type -AssemblyName System.Windows.Forms

$currentDirectory = Get-Location

Write-Host "Locate and select your Obsidian install..."

$fileBrowser = New-Object System.Windows.Forms.OpenFileDialog
$fileBrowser.Multiselect = $false
$fileBrowser.InitialDirectory = $currentDirectory
$fileBrowser.Filter = "Obsidian.exe|Obsidian.exe"

if($fileBrowser.ShowDialog() -eq "OK")
{
    $target = Split-Path -Path $fileBrowser.FileName
    $link = Join-Path -Path $currentDirectory "Obsidian"
    cmd /c mklink /J $link $target
}
else
{
    Write-Host "File browser cancelled. Unable to create symbolic link!" -ForegroundColor Red
}

Write-Host "Press any key to continue..."
[System.Console]::ReadKey()