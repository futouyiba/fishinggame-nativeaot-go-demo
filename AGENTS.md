# Agent Guide

## 语言规则

- 默认所有面向用户的最终回复使用中文。
- Markdown 文档统一使用中文。

## 项目定位

- 这是独立 demo，不接入 `D:\fishinggame` 的现有玩法代码。
- 目标是验证 `纯 C# Core -> NativeAOT 导出 -> Go cgo 调用 -> Unity 直接引用源码` 这条链路。
- 当前只有无状态 `calc_step`，还没有 `session_create/session_step/session_destroy`。

## 进入工作前先读

1. `README.md`
2. `docs/architecture.md`
3. `docs/abi_contract.md`
4. `codex/HANDOFF.md`

## 代码分层约束

- `src/Game.Domain.Core` 只能放纯 C# 规则和模型，不允许引用 `UnityEngine`。
- `src/Game.NativeAot.Export` 只负责 ABI、导出和最小 marshalling。
- `src/GoServer` 只通过 `bridge.h` / `bridge.go` 调原生库。
- `src/UnityGame` 只是 Unity 适配示例，不是完整 Unity 项目。

## Windows 构建约束

- NativeAOT 发布时必须设置 `VSCMD_SKIP_SENDTELEMETRY=1`，否则本机环境会卡在 VS DevShell telemetry。
- Go 的 `cgo` 在 Windows 下使用 GNU/MinGW 链接链路，不能直接吃 MSVC `.lib`。
- `tools/build-native.ps1` 已经负责生成 `GameNative.dll.a`，不要手工跳过这一步。
- `LLVM-MinGW (UCRT)` 是当前默认 Go 侧工具链。

## 常用命令

- 端到端验证：`powershell -ExecutionPolicy Bypass -File .\tools\test-demo.ps1`
- 仅发布 NativeAOT：`powershell -ExecutionPolicy Bypass -File .\tools\build-native.ps1`
- 启动 Codex CLI：`powershell -ExecutionPolicy Bypass -File .\tools\start-codex-cli.ps1`

## 与原仓库的关系

- 如需参考原工程或文档，路径分别是 `D:\fishinggame` 和 `D:\fishinggame-docs`。
- 除非用户明确要求，不要把这个 demo 重新并回原 Unity 工程。
