# NativeAOT ABI 合约测试 Walkthrough

## 节点目标

验证 NativeAOT 导出层使用的 native struct 布局稳定，避免 C# / Go / C ABI 之间出现字段偏移不一致。

## 覆盖范围

- `StepInputNative`
- `StepOutputNative`
- `ErrorInfoNative`
- `SessionConfigNative`
- `SessionStepInputNative`

## 对应测试

- `tests/Game.NativeAot.ContractTests/NativeAbiTests.cs`

## 执行命令

```powershell
dotnet test .\NativeAotGoDemo.sln -c Release --filter "FullyQualifiedName~Game.NativeAot.ContractTests"
```

## 检查步骤

1. 确认 `Marshal.SizeOf<T>` 与预期一致。
2. 确认每个字段的 `OffsetOf` 与 `bridge.h` 契约一致。
3. session 相关 native struct 新增后，同步验证新布局。

## 最新结果

- 时间：2026-03-18
- 状态：通过
- 结果摘要：
  - `Game.NativeAot.ContractTests.dll` 通过 5 项，失败 0 项
  - `session_create` / `session_step` 所需结构体已纳入 ABI 校验

## 最近一次失败记录

- 时间：无
- 状态：暂无失败记录
