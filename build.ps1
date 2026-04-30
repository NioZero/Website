$ErrorActionPreference = "Stop"

if (-not $env:DOTNET_CLI_HOME) {
    $env:DOTNET_CLI_HOME = Join-Path $PSScriptRoot ".dotnet-cli"
}

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"

dotnet build PersonalSite.sln -m:1
