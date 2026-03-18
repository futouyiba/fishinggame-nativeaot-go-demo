# Codex CLI Handoff

## 当前状态

- demo 已从 `D:\fishinggame` 迁移到独立目录 `D:\fishinggame-nativeaot-go-demo`
- C# Core、NativeAOT 导出、Go `cgo` bridge、golden cases、Unity package 示例都已落地
- Windows 机器上已安装：
  - `.NET SDK 9`
  - `Go 1.23.1`
  - `LLVM-MinGW (UCRT)`

## 已验证结果

- `powershell -ExecutionPolicy Bypass -File .\tools\test-demo.ps1`
- `go run ./cmd/demo`

当前 demo 输出包含两段：

```text
result={ResultCode:0 BiteProb:0.5717213 WeightFactor:1.5639808 TensionDelta:10.740984 FishTypeID:2001 IsHit:false}
session_result={...}
```

## 关键实现点

- 纯规则核心：
  - `src/Game.Domain.Core/Simulation/RealtimeStepCalculator.cs`
  - `src/Game.Domain.Core/Simulation/FishingSession.cs`
- NativeAOT 导出：
  - `src/Game.NativeAot.Export/Exports/CalcExports.cs`
  - `src/Game.NativeAot.Export/Exports/SessionExports.cs`
- Go bridge：
  - `src/GoServer/internal/nativecalc/bridge.go`
  - `src/GoServer/internal/nativecalc/bridge.h`
- Unity package：
  - `src/UnityGame/Packages/com.nativeaotgo.demo`
- 跨端金样例：
  - `tests/GoldenCases/step_cases.json`

## 已知环境坑

### 1. NativeAOT 发布卡住

- 原因：会卡在 `findvcvarsall.bat -> VS DevShell telemetry`
- 处理：`tools/build-native.ps1` 里已经固定 `VSCMD_SKIP_SENDTELEMETRY=1`

### 2. Go cgo 无法直接链接 MSVC `.lib`

- 原因：Windows 下 Go 默认走 GNU/MinGW 风格链接
- 处理：`tools/build-native.ps1` 会在 `src/GoServer/internal/nativecalc/lib` 中自动生成：
  - `GameNative.dll`
  - `GameNative.lib`
  - `GameNative.def`
  - `GameNative.dll.a`

## 下一步候选

- 为 session 增加状态查询或重置接口，避免只能通过外部推断状态
- 扩充 golden cases，覆盖 session 连续步进
- 把 demo 规则替换成更接近真实业务的规则
- 给 Unity package 补 asmdef 拆分、sample 场景和导入说明

## CLI 建议入口

- 交互式：`powershell -ExecutionPolicy Bypass -File .\tools\start-codex-cli.ps1`
- 非交互：`Get-Content .\codex\CLI_BOOTSTRAP_PROMPT.md -Raw | codex exec -C . --add-dir D:\fishinggame --add-dir D:\fishinggame-docs -`
