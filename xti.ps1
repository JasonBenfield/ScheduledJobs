Import-Module PowershellForXti -Force

function Add-JobDBMigrations {
    param ([Parameter(Mandatory)]$Name)
    $env:DOTNET_ENVIRONMENT="Development"
    dotnet ef --startup-project ./ScheduledJobsWebApp/Internal/XTI_JobsDbTool migrations add $Name --project ./ScheduledJobsWebApp/Internal/XTI_JobsDB.SqlServer
}

function Xti-ResetJobDb {
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

function Xti-BackupJobDb {
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

function Xti-RestoreJobDb {
    param (
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [ValidateSet(“Development", "Production", "Staging", "Test")]
        [string] $EnvName='Staging', 
        [Parameter(Mandatory)]
        [string] $BackupFilePath
    )
    dotnet run --environment $EnvName --project ./ScheduledJobsWebApp/Internal/XTI_JobsDbTool --Command restore --BackupFilePath $BackupFilePath
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Restore failed"
    }
}

function Xti-UpdateJobDb {
    param (
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [ValidateSet(“Development", "Production", "Staging", "Test")]
        $EnvName='Test'
    )
    dotnet run --environment $EnvName --project ./ScheduledJobsWebApp/Internal/XTI_JobsDbTool --Command update
    if( $LASTEXITCODE -ne 0 ) {
        Throw "Update failed"
    }
}

function Xti-UpdateNpm {
	Start-Process -FilePath "cmd.exe" -WorkingDirectory ScheduledJobsWebApp/Apps/ScheduledJobsWebApp -ArgumentList "/c", "npm install @jasonbenfield/sharedwebapp@latest @jasonbenfield/hubwebapp@latest"
}