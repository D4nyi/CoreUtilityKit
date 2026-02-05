Clear-Host;

$UtcNow = [DateTime]::UtcNow
$BuildTime = $UtcNow.ToString('yyyy-MM-ddTHH:mm:ssZ')
$ReleaseYear = $UtcNow.Year

function script:UpdateProjectVersion([string] $CsprojPath, [string] $CsprojName)
{
    $xml = [xml](Get-Content -Path $CsprojPath -Encoding UTF8)

    $currentVersion = $xml.Project.PropertyGroup.Version

    $nextVersion = Read-Host "Next version of $CsprojName ($currentVersion)"

    if ([string]::IsNullOrWhiteSpace($nextVersion))
    {
        Write-Host "Not modified" -ForegroundColor Yellow
        return
    }

    $xml.Project.PropertyGroup.Version = $nextVersion
    $xml.Project.PropertyGroup.InformationalVersion = "$nextVersion Built: $BuildTime"
    $xml.Project.PropertyGroup.Copyright = "Copyright © $ReleaseYear. Dániel Szöllősi"

    Write-Host "$CsprojName saved" -ForegroundColor Green

    $xml.Save($CsprojPath)
}


$SrcPath = Join-Path $PSScriptRoot "src"

Get-ChildItem -Path $SrcPath -File -Filter "*.csproj" -Recurse -Depth 1 | ForEach-Object {
    UpdateProjectVersion -CsprojPath $_.FullName -CsprojName $_.Name
}
