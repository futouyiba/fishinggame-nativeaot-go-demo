# Unity Package 接入测试 Walkthrough

## 节点目标

验证 `src/UnityGame/Packages/com.nativeaotgo.demo` 作为本地 Unity package 时，能够直接复用 `Game.Domain.Core` 源码，而不是依赖 NativeAOT DLL。

## 覆盖范围

- `package.json`
- `NativeAotGoDemo.Unity.Runtime.asmdef`
- `Runtime/Shared` 源码链接
- `FishingRuntimeDriver` 运行时示例

## 对应脚本与文件

- `tools/setup-unity-source-link.ps1`
- `src/UnityGame/Packages/com.nativeaotgo.demo`

## 执行步骤

1. 在仓库根目录运行：

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\setup-unity-source-link.ps1
```

2. 在 Unity 工程中以本地 package 方式引入：
   `src/UnityGame/Packages/com.nativeaotgo.demo`
3. 检查 `Runtime/Shared` 下是否已建立 Core 源码目录链接。
4. 在 Unity 中给任意对象挂载 `FishingRuntimeDriver`，确认脚本能编译。

## 最新结果

- 时间：2026-03-18
- 状态：未执行自动化测试
- 结果摘要：
  - package 结构、asmdef、README 和 source-link 脚本已完成
  - 当前仓库内没有 Unity Editor 自动化测试或 CI 验证

## 最近一次失败记录

- 时间：无
- 状态：暂无失败记录

## 待补充

- 增加 Unity Editor 导入验证
- 增加 sample 场景或最小可运行说明
