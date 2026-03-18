$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$exportProject = Join-Path $repoRoot "src\Game.NativeAot.Export\Game.NativeAot.Export.csproj"
$publishDir = Join-Path $repoRoot "src\Game.NativeAot.Export\publish\win-x64"
$nativeBinDir = Join-Path $repoRoot "src\Game.NativeAot.Export\bin\Release\net9.0\win-x64\native"
$goLibDir = Join-Path $repoRoot "src\GoServer\internal\nativecalc\lib"

function Resolve-LlvmMingwBin {
    $packagesRoot = Join-Path $env:LOCALAPPDATA "Microsoft\WinGet\Packages"
    $installRoot = Get-ChildItem -Path $packagesRoot -Directory -Filter "MartinStorsjo.LLVM-MinGW.UCRT_*" |
        Select-Object -First 1

    if ($null -eq $installRoot) {
        throw "LLVM-MinGW (UCRT) is not installed. Install it with: winget install --id MartinStorsjo.LLVM-MinGW.UCRT"
    }

    $binDir = Get-ChildItem -Path $installRoot.FullName -Directory -Filter "llvm-mingw-*" |
        ForEach-Object { Join-Path $_.FullName "bin" } |
        Where-Object { Test-Path $_ } |
        Select-Object -First 1

    if ([string]::IsNullOrWhiteSpace($binDir)) {
        throw "LLVM-MinGW bin directory not found under: $($installRoot.FullName)"
    }

    return $binDir
}

$mingwBin = Resolve-LlvmMingwBin
$gendef = Join-Path $mingwBin "gendef.exe"
$dlltool = Join-Path $mingwBin "dlltool.exe"

$env:VSCMD_SKIP_SENDTELEMETRY = "1"
dotnet publish $exportProject -c Release -r win-x64 -o $publishDir
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed with exit code $LASTEXITCODE"
}

New-Item -ItemType Directory -Force -Path $goLibDir | Out-Null
Copy-Item -Path (Join-Path $publishDir "GameNative.dll") -Destination $goLibDir -Force
Copy-Item -Path (Join-Path $nativeBinDir "GameNative.lib") -Destination $goLibDir -Force

Push-Location $goLibDir
try {
    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    & $gendef "GameNative.dll" 1>$null 2>$null
    $gendefExitCode = $LASTEXITCODE
    & $dlltool -d "GameNative.def" -D "GameNative.dll" -l "GameNative.dll.a" 1>$null 2>$null
    $dlltoolExitCode = $LASTEXITCODE
    $ErrorActionPreference = $previousErrorActionPreference

    if ($gendefExitCode -ne 0) {
        throw "gendef failed with exit code $gendefExitCode"
    }

    if ($dlltoolExitCode -ne 0) {
        throw "dlltool failed with exit code $dlltoolExitCode"
    }
}
finally {
    $ErrorActionPreference = "Stop"
    Pop-Location
}

Write-Host "NativeAOT publish completed:"
Write-Host "  Publish: $publishDir"
Write-Host "  Go lib : $goLibDir"
