Clear-Host;
Write-Output "`n`n`n`n`n";


$script:Progress = 0;
function script:Progress ([string] $Activity, [string] $Status) {
    $script:Progress += 11.1111111111111;
    Write-Progress -Activity $Activity -Status $Status -PercentComplete $Progress;
}


function script:InvokeCommand ([string] $Command) {
    Progress -Activity "Running command..." -Status $Command;
    
    Write-Output $Command;
    Write-Output "";
    Invoke-Expression $Command | Out-Null;
}


function private:RemoveDir([string] $DirPath) {
    Progress -Activity "Previous test results are removed!" -Status $DirPath;
    
    if (Test-Path $DirPath) {
        Write-Output "Deleting folder: $DirPath";
        Remove-Item $DirPath -Recurse -Force;
    } else {
        Write-Output "Folder does not exist: $DirPath";
    }
    Write-Output "";
}


function private:PrintReports([string] $TestResultsPath) {
    $CoverageReports = (Get-ChildItem -Path $TestResultsPath -Directory | Foreach-Object { Join-Path -Path $_.FullName -ChildPath "coverage.cobertura.xml" });

    Progress -Activity "Collected coverage reports" -Status "Count: $($CoverageReports.Length)";

    Write-Output "Found $($CoverageReports.Length) test result directories:";

    $i = 1;
    $CoverageReports | Foreach-Object { Write-Output ("{0}.   {1}" -f $i++, $_) };
    Write-Output "";
}


function private:ExecuteTests([string[]] $TestProjects) {
    $TestCommand = "dotnet test {0} --collect:`"XPlat Code Coverage`" --settings coverlet.runsettings.xml";

    InvokeCommand("dotnet clean");

    foreach ($TestProject in $TestProjects)
    {
        InvokeCommand(($TestCommand -f $TestProject));
    }
}


function private:GenerateReport([string] $CoverageReportPath) {
    $GenerateReportCommand = "reportgenerator -reports:.\test\coverageresults\**\coverage.cobertura.xml -targetdir:$CoverageReportPath -reporttypes:Html_Dark";
    InvokeCommand($GenerateReportCommand);

    InvokeCommand("start $CoverageReportPath\index.html");
}


$script:i = 1;

#region Paths
$CoverageReportPath = ".\test\coveragereport";
$TestResultsPath    = ".\test\coverageresults";
$TestProjects       = (Get-ChildItem -Path (".\test") -Directory -Exclude bin, obj, coverage* | Foreach-Object { Join-Path -Path $_.FullName -ChildPath "$($_.Name).csproj" });
$TestProjects | Foreach-Object { Write-Output ("{0}.   {1}" -f $script:i++, $_) };
#endregion


RemoveDir($CoverageReportPath);
RemoveDir($TestResultsPath);


ExecuteTests($TestProjects);


PrintReports($TestResultsPath);


GenerateReport($CoverageReportPath);


Start-Sleep -Milliseconds 1000;
