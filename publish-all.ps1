$ErrorActionPreference = "Stop"

$project = "src/HKSDEImporter.Console/HKSDEImporter.Console.csproj"
$common = @(
    "-c", "Release",
    "--self-contained", "true",
    "-p:PublishSingleFile=true",
    "-p:IncludeNativeLibrariesForSelfExtract=true",
    "-p:EnableCompressionInSingleFile=true",
    "-p:DebugType=None",
    "-p:DebugSymbols=false",
    "-p:CopyOutputSymbolsToPublishDirectory=false"
)

Write-Host "Publishing win-x64..."
dotnet publish $project -r win-x64 @common -o ./publish/win-x64

Write-Host "Publishing win-x86..."
dotnet publish $project -r win-x86 @common -o ./publish/win-x86

Write-Host "Publishing linux-x64..."
dotnet publish $project -r linux-x64 @common -o ./publish/linux-x64

Write-Host "Done. Outputs:"
Write-Host "  ./publish/win-x64"
Write-Host "  ./publish/win-x86"
Write-Host "  ./publish/linux-x64"
