Clear-Host;

$GitStatus = [String[]]$(git status -s);
if ($GitStatus.Length -ne 0) {
    Write-Error "Repository not clean! Commit changes before publish!";
    exit 1;
}

$SourceDir = Join-Path -Path $PSScriptRoot -ChildPath "\src";

$AssemblyPath = Join-Path -Path $SourceDir -ChildPath "\Properties\Assembly.cs";

$AssemblyDataFile = [String[]][System.IO.File]::ReadAllLines($AssemblyPath);
$Version = $AssemblyDataFile[6] -match "(\d{1,2})\.(\d{1,2})\.(\d{1,2})\.(\d{3})"

if(!($Version)) {
    Write-Error "Error: Could not parse version from $AssemblyPath";
    exit 1;
}

$BuildTime = Get-Date
$Revision = [Int32](New-TimeSpan -Start $BuildTime.Date -End $BuildTime).TotalMinutes;

$BuildPart = [Int32]$Matches.3;

$NewVersionGenerated = "$($Matches.1).$($Matches.2).$($BuildPart + 1)";

Write-Host "Accept the generated or enter a new version number";
Write-Host "Generated version $NewVersionGenerated";
Write-Host "Version format must be Major.Minor.Build (\d{1,2})\.(\d{1,2})\.(\d{1,2})";
$Result = Read-Host -Prompt "[aA]ccept or enter a new version number";

if ($Result -eq "a" -or $Result -eq "A" -or $Result -eq "") {
    $NewVersionPrefix = $NewVersionGenerated;
}
elseif ($Result -match "(\d{1,2})\.(\d{1,2})\.(\d{1,2})") {
   $NewVersionPrefix = $Matches.0;
}
else {
    Write-Error "Invalid version format!";
    exit 1;
}

$NewVersion = $NewVersionPrefix + ".$Revision";

$Year               = (Get-Date).Year;
$BuildTimeFormatted = Get-Date -Format "yyyy. MM. dd. HH:mm:ss";
$VersionLine        = "[assembly: AssemblyVersion(`"$NewVersion`")]";
$FileVersionLine    = "[assembly: AssemblyFileVersion(`"$NewVersion`")]";
$BuildTime          = "[assembly: AssemblyInformationalVersion(`"$NewVersion Build: ($BuildTimeFormatted)`")]";
$AssemblyCompany    = "[assembly: AssemblyCompany(`"Dániel Szöllősi`")]";
$CopyRightWitYear   = "[assembly: AssemblyCopyright(`"Copyright © $Year. Dániel Szöllősi`")]";

$AssemblyDataFile[3]  = $VersionLine;
$AssemblyDataFile[4]  = $FileVersionLine;
$AssemblyDataFile[5]  = $BuildTime;
$AssemblyDataFile[8] = $AssemblyCompany;
$AssemblyDataFile[9] = $CopyRightWitYear;

[System.IO.File]::WriteAllLines($AssemblyPath, $AssemblyDataFile, [System.Text.Encoding]::UTF8);

$CsprojPath = "$SourceDir\Essentials.csproj";
$Csproj     = [String[]][System.IO.File]::ReadAllLines($CsprojPath);
$Csproj[3]  = "		<VersionPrefix>$NewVersionPrefix</VersionPrefix>";
[System.IO.File]::WriteAllLines($CsprojPath, $Csproj, [System.Text.Encoding]::UTF8);

$PublishCommand = "dotnet publish $BuildProject --nologo -v q -c Release /p:warnaserror=NU1605 /p:analyzerconfig=$PSScriptRoot\.editorconfig /p:utf8output=true /p:highentropyva=true /p:nullable=enable /p:filealign=512 /p:Deterministic=true /p:Optimize=true";
Write-Host $PublishCommand;

dotnet clean --nologo -v q;
Invoke-Expression $PublishCommand;

git add .;
git commit -m "Build $NewVersion";
git push --follow-tags;