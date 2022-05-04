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