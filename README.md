# fishinggame-nativeaot-go-demo

一个独立的验证型 demo，用来打通这条最小链路：

`纯 C# Core -> NativeAOT 导出 -> Go cgo 调用 -> Unity 直接复用源码`

这个仓库不接入 `D:\fishinggame` 的现有玩法代码，目标是先把跨语言边界、ABI 契约、Windows 构建链路和 Unity 复用方式验证清楚。

## 当前状态

- 已支持无状态接口：`calc_step`
- 已支持最小 session 生命周期：
  - `session_create`
  - `session_step`
  - `session_destroy`
- 已包含：
  - 纯 C# 规则核心
  - NativeAOT 导出层
  - Go `cgo` bridge
  - C# / Go 共用 golden cases
  - Unity package 形态示例
  - walkthrough 测试文档

## 仓库结构

```text
src/
  Game.Domain.Core           纯 C# 规则与模型
  Game.NativeAot.Export      NativeAOT ABI 导出层
  GoServer                   Go cgo bridge 与示例
  UnityGame                  Unity package 示例

tests/
  Game.Domain.Core.Tests     Core 单测
  Game.NativeAot.ContractTests
                             ABI 布局测试
  GoldenCases                C# / Go 共用金样例

docs/
  architecture.md            架构说明
  abi_contract.md            ABI 契约说明
  test_vectors/              测试 walkthrough 文档
```

## 核心约束

- `src/Game.Domain.Core` 只能放纯 C# 规则和模型，不引用 `UnityEngine`
- `src/Game.NativeAot.Export` 只负责 ABI、导出和最小 marshalling
- `src/GoServer` 只通过 `bridge.h` / `bridge.go` 调用原生库
- `src/UnityGame` 只是 Unity 接入示例，不是完整游戏工程

## Windows 前置环境

- `.NET SDK 9`
- `Go 1.23+`
- `LLVM-MinGW (UCRT)`

安装 LLVM-MinGW：

```powershell
winget install --id MartinStorsjo.LLVM-MinGW.UCRT
```

## 快速开始

进入仓库：

```powershell
cd D:\fishinggame-nativeaot-go-demo
```

端到端验证：

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\test-demo.ps1
```

仅发布 NativeAOT：

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\build-native.ps1
```

运行 Go 示例：

```powershell
cd .\src\GoServer
go run .\cmd\demo
```

让 Unity package 直接链接 Core 源码：

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\setup-unity-source-link.ps1
```

## 当前验证结果

已验证通过：

- `powershell -ExecutionPolicy Bypass -File .\tools\test-demo.ps1`
- `go run ./cmd/demo`

当前基线输出示例：

```text
result={ResultCode:0 BiteProb:0.5717213 WeightFactor:1.5639808 TensionDelta:10.740984 FishTypeID:2001 IsHit:false}
session_result={...}
```

## Session 设计

当前 session 先只做“最小闭环”，不提前扩成完整玩法状态机：

- `session_create` 保存 `cast_id`、`seed_base`、`pose_index`、`sample_slot`、`tension`、`water_temp`
- `session_step` 用当前 session 状态拼装 `StepInput` 后执行一步规则计算
- `session_destroy` 释放 session handle

当前状态推进规则：

- 每次成功 `session_step` 后，`sample_slot` 自增 1
- 每次成功 `session_step` 后，`tension = clamp(tension + tension_delta, 0, 100)`

## 文档入口

- [架构说明](./docs/architecture.md)
- [ABI 契约](./docs/abi_contract.md)
- [测试 Walkthrough 索引](./docs/test_vectors/README.md)
- [Codex Handoff](./codex/HANDOFF.md)

## 已知环境问题

### 1. NativeAOT 发布可能卡在 VS DevShell telemetry

已在 `tools/build-native.ps1` 中设置：

```text
VSCMD_SKIP_SENDTELEMETRY=1
```

### 2. Go cgo 在 Windows 下不能直接链接 MSVC `.lib`

当前通过 `tools/build-native.ps1` 自动生成这些文件供 Go 使用：

- `GameNative.dll`
- `GameNative.lib`
- `GameNative.def`
- `GameNative.dll.a`

## 后续方向

- 为 session 增加状态查询或重置接口
- 扩充 golden cases，覆盖 session 连续步进
- 用更接近真实业务的规则替换 demo 规则
- 继续完善 Unity package 的 asmdef、sample 和导入说明
