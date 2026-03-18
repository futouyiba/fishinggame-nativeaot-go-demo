# UnityGame

`src/UnityGame` 现在采用 package 形态承载 Unity 示例，而不是继续把脚本直接堆在 `Assets/` 下。

当前 package 根目录：

- `Packages/com.nativeaotgo.demo`

如果要把 Core 源码链接进 Unity package，请执行：

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\setup-unity-source-link.ps1
```
