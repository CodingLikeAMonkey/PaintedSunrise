# CleanUIDs.ps1 - Deletes all .cs.uid files in this folder and subfolders
Get-ChildItem -Recurse -Filter *.cs.uid | Remove-Item -Force
Write-Host ".cs.uid files deleted."
