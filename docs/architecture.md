# 架构说明

这个 demo 只验证一条最小链路：

- `Game.Domain.Core` 保存纯 C# 规则核心。
- `Game.NativeAot.Export` 把核心逻辑包装成稳定的 C ABI，并发布成 NativeAOT 动态库。
- `GoServer` 通过 `cgo` 调用 NativeAOT 导出的 `calc_step`、`session_create`、`session_step`、`session_destroy`。
- `UnityGame` 只放 Unity package 形态的适配示例，说明 Unity 应直接复用 Core 源码，而不是调用 DLL。

## 目录约束

- `src/Game.Domain.Core` 是唯一共享规则源。
- `src/Game.NativeAot.Export` 只负责 ABI、导出和最小 marshalling。
- `src/GoServer` 不关心 C# 对象模型，只面向 `bridge.h`。
- `src/UnityGame/Packages/com.nativeaotgo.demo` 是 Unity 侧演示 package。
- `tests/GoldenCases` 是跨 C# 和 Go 共用的金样例。

## Session 设计

首版 demo 先用无状态 `calc_step` 验证 ABI；当前已补充最小 session 闭环：

- `session_create`：创建 session，保存 `cast_id`、`seed_base`、`pose_index`、`sample_slot`、`tension`、`water_temp`
- `session_step`：使用当前 session 状态拼装 `StepInput`，执行一步规则计算
- `session_destroy`：释放 session handle

当前 session 的状态推进规则：

- 每次成功 `session_step` 后，`sample_slot` 自增 1
- 每次成功 `session_step` 后，`tension = clamp(tension + tension_delta, 0, 100)`

这样可以先验证生命周期管理、句柄管理和跨语言状态推进，而不提前把问题扩大成完整玩法状态机。
