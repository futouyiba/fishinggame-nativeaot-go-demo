# NativeAOT Go Demo Unity Package

这个目录提供一个最小 Unity Package 结构，用来演示 Unity 侧直接复用 `Game.Domain.Core` 源码，而不是调用 NativeAOT 导出的 DLL。

## 目录说明

- `Runtime/`：Unity 运行时示例脚本与 asmdef。
- `Runtime/Shared/`：由 `tools/setup-unity-source-link.ps1` 建立的 Core 源码链接目录。
- `Samples~/RuntimeDriver/`：运行时驱动示例说明。

## 使用方式

1. 在仓库根目录执行：
   `powershell -ExecutionPolicy Bypass -File .\tools\setup-unity-source-link.ps1`
2. 把 `src/UnityGame/Packages/com.nativeaotgo.demo` 作为本地 package 引入 Unity 工程。
3. 在 Unity 中引用 `NativeAotGoDemo.Unity.Runtime` 程序集。
