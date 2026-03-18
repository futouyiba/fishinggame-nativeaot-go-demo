# Core 规则与 Session 测试 Walkthrough

## 节点目标

验证 `src/Game.Domain.Core` 的纯 C# 规则计算稳定，且新增 `FishingSession` 后状态推进符合当前 demo 约定。

## 覆盖范围

- `RealtimeStepCalculator.Step`
- golden cases 一致性
- `FishingSession.Step`
- `FishingSession.Snapshot`

## 对应测试

- `tests/Game.Domain.Core.Tests/RealtimeStepCalculatorTests.cs`

## 执行命令

```powershell
dotnet test .\NativeAotGoDemo.sln -c Release --filter "FullyQualifiedName~Game.Domain.Core.Tests"
```

## 检查步骤

1. 运行测试项目，确认 `SameInput_ShouldProduceSameOutput` 通过。
2. 确认 `GoldenCases_ShouldMatchExpected` 逐条匹配 `tests/GoldenCases/step_cases.json`。
3. 确认 `FishingSession_ShouldAdvanceSampleSlotAndTension` 通过。
4. 检查 session 连续两步输出不同，且 `Snapshot.CurrentSampleSlot == 14`。

## 最新结果

- 时间：2026-03-18
- 状态：通过
- 结果摘要：
  - `Game.Domain.Core.Tests.dll` 通过 5 项，失败 0 项
  - session 最小闭环已覆盖到单测

## 最近一次失败记录

- 时间：无
- 状态：暂无失败记录
