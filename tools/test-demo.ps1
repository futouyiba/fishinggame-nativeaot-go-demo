$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$goRoot = Join-Path $repoRoot "src\GoServer"
$goLibDir = Join-Path $goRoot "internal\nativecalc\lib"

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
$gcc = Join-Path $mingwBin "gcc.exe"
$gpp = Join-Path $mingwBin "g++.exe"

& (Join-Path $scriptDir "build-native.ps1")

dotnet test (Join-Path $repoRoot "NativeAotGoDemo.sln") -c Release

if (-not (Test-Path $gcc)) {
    throw "gcc.exe not found: $gcc"
}

$env:CC = $gcc
$env:CXX = $gpp
$env:CGO_ENABLED = "1"
$env:PATH = "$goLibDir;$mingwBin;$env:PATH"

Push-Location $goRoot
try {
    go test ./... -count=1
}
finally {
    Pop-Location
}

Write-Host "C# tests and Go interop tests completed."
