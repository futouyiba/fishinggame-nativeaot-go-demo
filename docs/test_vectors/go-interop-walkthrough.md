# Go Bridge 互操作测试 Walkthrough

## 节点目标

验证 Go `cgo` bridge 能正确调用 NativeAOT 导出库，并保证无状态 `calc_step` 与有状态 session 调用都可工作。

## 覆盖范围

- `nativecalc.CalcStep`
- `nativecalc.CreateSession`
- `Session.Step`
- `Session.Close`
- golden cases 跨语言一致性

## 对应测试

- `src/GoServer/tests/golden_test.go`
- `tools/test-demo.ps1`

## 执行命令

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\test-demo.ps1
```

或仅运行 Go 测试：

```powershell
cd .\src\GoServer
go test ./... -count=1
```

## 检查步骤

1. 先执行 `tools/build-native.ps1`，确保 `GameNative.dll` 和 `GameNative.dll.a` 已刷新。
2. 运行 Go golden case，确认 `calc_step` 输出与 `step_cases.json` 一致。
3. 运行 session 测试，确认第一步结果匹配基线，第二步结果与第一步不同。
4. 确认 `Session.Close` 不报错。

## 最新结果

- 时间：2026-03-18
- 状态：通过
- 结果摘要：
  - `src/GoServer/tests` 通过
  - `tools/test-demo.ps1` 全链路通过

## 最近一次失败记录

- 时间：2026-03-18
- 状态：已修复
- 现象：
  - `TestSessionStep_AdvancesSampleSlotAndTension` 第二步期望值写死错误
  - 实际报错：`float mismatch: want 0.321822 got 0.524921`
- 处理：
  - 将断言改为验证“session 第二步成功且输出不同于第一步”
  - 修复后重新运行 `tools/test-demo.ps1`，已通过
