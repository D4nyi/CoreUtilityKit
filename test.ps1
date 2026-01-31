Clear-Host;


function script:InvokeCommand ([string] $Command) {
    Write-Output $Command;
    Write-Output "";
    Invoke-Expression $Command | Out-Null;
}


function private:RemoveDir([string] $DirPath) {
    if (Test-Path $DirPath) {
        Write-Output "Deleting folder: $DirPath";
        Remove-Item $DirPath -Recurse -Force;
    } else {
        Write-Output "Folder does not exist: $DirPath";
    }
    Write-Output "";
}


function private:ExecuteTests() {
    $TestCommand = "dotnet test .\CoreUtilityKit.sln --collect:`"XPlat Code Coverage`" --settings coverlet.runsettings.xml";

    InvokeCommand("dotnet clean");

    InvokeCommand($TestCommand);
}


function private:GenerateReport([string] $CoverageReportPath) {
    $GenerateReportCommand = "reportgenerator -reports:.\test\coverageresults\*\coverage.cobertura.xml -targetdir:$CoverageReportPath -reporttypes:Html_Dark";

    InvokeCommand($GenerateReportCommand);

    InvokeCommand("start $CoverageReportPath\index.html");
}


$script:i = 1;

#region Paths
$CoverageReportPath = ".\test\coveragereport";
$TestResultsPath    = ".\test\coverageresults";
#$TestProjects       = (Get-ChildItem -Path (".\test") -Directory -Exclude bin, obj, coverage* | Foreach-Object { Join-Path -Path $_.FullName -ChildPath "$($_.Name).csproj" });
#$TestProjects | Foreach-Object { Write-Output ("{0}.   {1}" -f $script:i++, $_) };
#endregion


RemoveDir($CoverageReportPath);
RemoveDir($TestResultsPath);

ExecuteTests;

GenerateReport $CoverageReportPath;
