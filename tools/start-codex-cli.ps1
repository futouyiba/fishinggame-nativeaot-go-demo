$ErrorActionPreference = "Stop"

param(
    [switch]$FullAuto,
    [switch]$Search
)

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$bootstrapPrompt = Get-Content (Join-Path $repoRoot "codex\CLI_BOOTSTRAP_PROMPT.md") -Raw

$args = @(
    "-C", $repoRoot,
    "--add-dir", "D:\fishinggame",
    "--add-dir", "D:\fishinggame-docs",
    "--no-alt-screen"
)

if ($FullAuto) {
    $args += "--full-auto"
}
else {
    $args += @("-s", "workspace-write", "-a", "on-request")
}

if ($Search) {
    $args += "--search"
}

& codex @args $bootstrapPrompt
