Import-Module PowershellForXti -Force

$BaseXtiPublish = ${Function:Xti-Publish}

function Xti-Publish {
    param(
        [ValidateSet("Production", "Development")]
        [string] $EnvName="Development"
    )
    & $BaseXtiPublish @PsBoundParameters
}

$BaseXtiInstall = ${Function:Xti-Install}

function Xti-Install {
    param(
        [ValidateSet("Development", "Production", "Staging", "Test")]
        $EnvName = "Development"
    )
    & $BaseXtiInstall @PsBoundParameters
}

function Add-JobDBMigrations {
    param ([Parameter(Mandatory)]$Name)
    $env:DOTNET_ENVIRONMENT="Development"
    dotnet ef --startup-project ./ScheduledJobsWebApp/Internal/XTI_JobsDbTool migrations add $Name --project ./ScheduledJobsWebApp/Internal/XTI_JobsDB.SqlServer
}

function Xti-ResetHubDb {
    param (
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [ValidateSet(“Development", "Production", "Staging", "Test")]
        [string] $EnvName='Test',
        [switch] $Force
    )
    dotnet run --environment $EnvName --project ./ScheduledJobsWebApp/Internal/XTI_JobsDbTool --Command reset --Force $Force
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Reset failed"
    }
}

function Xti-BackupHubDb {
    param (
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [ValidateSet(“Development", "Production", "Staging", "Test")]
        [string] $EnvName='Production', 
        [Parameter(Mandatory)]
        [string] $BackupFilePath
    )
    if($EnvName -eq "Production" -or $EnvName -eq "Staging") {
        $dirPath = [System.IO.Path]::GetDirectoryName($BackupFilePath)
        if(-not(Test-Path $dirPath -PathType Container)) { 
            New-Item -ItemType Directory -Force -Path $dirPath
        }
    }
    dotnet run --environment $EnvName --project ./ScheduledJobsWebApp/Internal/XTI_JobsDbTool --Command backup --BackupFilePath=$BackupFilePath
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Backup failed"
    }
}

function Xti-RestoreHubDb {
    param (
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [ValidateSet(“Development", "Production", "Staging", "Test")]
        [string] $EnvName='Staging', 
        [Parameter(Mandatory)]
        [string] $BackupFilePath
    )
    dotnet run --environment $EnvName --project ./Tools/HubDbTool --Command restore --BackupFilePath $BackupFilePath
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Restore failed"
    }
}

function Xti-UpdateHubDb {
    param (
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [ValidateSet(“Development", "Production", "Staging", "Test")]
        $EnvName='Test'
    )
    dotnet run --environment $EnvName --project ./Tools/HubDbTool --Command update
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Update failed"
    }
}
