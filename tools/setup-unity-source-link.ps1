$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$unitySharedRoot = Join-Path $repoRoot "src\UnityGame\Packages\com.nativeaotgo.demo\Runtime\Shared"
$coreRoot = Join-Path $repoRoot "src\Game.Domain.Core"
$sourceFolders = @(
    "Compatibility",
    "Math",
    "Model",
    "Rng",
    "Rules",
    "Simulation",
    "Validation"
)

New-Item -ItemType Directory -Force -Path $unitySharedRoot | Out-Null

foreach ($folder in $sourceFolders) {
    $linkPath = Join-Path $unitySharedRoot $folder
    $targetPath = Join-Path $coreRoot $folder

    if (Test-Path $linkPath) {
        throw "Link path already exists: $linkPath"
    }

    New-Item -ItemType Junction -Path $linkPath -Target $targetPath | Out-Null
    Write-Host "Created Unity source link:"
    Write-Host "  $linkPath -> $targetPath"
}
