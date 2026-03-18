# NativeAOT + Go Demo

这个目录是独立 demo，不接入当前主工程玩法代码。

当前已包含：

- 纯 C# 规则核心
- NativeAOT 导出层
- Go `cgo` bridge
- C# / Go 共用 golden cases
- Unity package 形态示例

## Windows 前置环境

- `.NET SDK 9`
- `Go 1.23+`
- `LLVM-MinGW (UCRT)`

安装 LLVM-MinGW：

```powershell
winget install --id MartinStorsjo.LLVM-MinGW.UCRT
```

## 常用命令

```powershell
cd D:\fishinggame-nativeaot-go-demo
.\tools\test-demo.ps1
```

仅发布 NativeAOT：

```powershell
.\tools\build-native.ps1
```

如果要让 Unity 直接复用同一份 Core 源码：

```powershell
.\tools\setup-unity-source-link.ps1
```

如果要继续用 Codex CLI 处理这个 demo：

```powershell
.\tools\start-codex-cli.ps1
```
