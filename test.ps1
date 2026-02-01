Clear-Host;


function script:InvokeCommand ([string] $Command) {
    Write-Host $Command;
    Write-Host "";
    Invoke-Expression $Command | Out-Null;
}


function private:RemoveDir([string] $DirPath) {
    if (Test-Path $DirPath) {
        Write-Host "Deleting folder: $DirPath";
        Remove-Item $DirPath -Recurse -Force;
    } else {
        Write-Host "Folder does not exist: $DirPath";
    }
    Write-Host "";
}


function private:ExecuteTests() {
    $TestCommand = "dotnet test .\CoreUtilityKit.slnx --collect:`"XPlat Code Coverage`" --settings coverlet.runsettings.xml";

    InvokeCommand("dotnet clean");

    InvokeCommand($TestCommand);
}


function private:GenerateReport([string] $CoverageReportPath) {
    $GenerateReportCommand = "reportgenerator -reports:.\test\coverageresults\*\coverage.cobertura.xml -targetdir:$CoverageReportPath -reporttypes:Html_Dark";

    InvokeCommand($GenerateReportCommand);

    InvokeCommand("Start-Process $CoverageReportPath\index.html");
}


$script:i = 1;

#region Paths
$CoverageReportPath = ".\test\coveragereport";
$TestResultsPath    = ".\test\coverageresults";
#$TestProjects       = (Get-ChildItem -Path (".\test") -Directory -Exclude bin, obj, coverage* | Foreach-Object { Join-Path -Path $_.FullName -ChildPath "$($_.Name).csproj" });
#$TestProjects | Foreach-Object { Write-Host ("{0}.   {1}" -f $script:i++, $_) };
#endregion


RemoveDir($CoverageReportPath);
RemoveDir($TestResultsPath);

ExecuteTests;

Write-Host "Wait 5sec for the tests to finish in the background";
Start-Sleep -Seconds 5

GenerateReport $CoverageReportPath;
