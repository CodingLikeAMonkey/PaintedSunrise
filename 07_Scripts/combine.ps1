# Define the output file
$outputFile = "Combined.cs"

# Delete the output file if it already exists to avoid appending
if (Test-Path $outputFile) {
    Remove-Item $outputFile
}

# Get all .cs files in the current directory and subdirectories
$csFiles = Get-ChildItem -Path . -Recurse -Filter *.cs | Sort-Object FullName

# Combine each file's content into the output file
foreach ($file in $csFiles) {
    Add-Content -Path $outputFile -Value "// ==================="
    Add-Content -Path $outputFile -Value "// File: $($file.FullName)"
    Add-Content -Path $outputFile -Value "// ==================="
    Add-Content -Path $outputFile -Value (Get-Content -Path $file.FullName)
    Add-Content -Path $outputFile -Value "`n"
}

Write-Output "Combined $($csFiles.Count) C# files into $outputFile"
