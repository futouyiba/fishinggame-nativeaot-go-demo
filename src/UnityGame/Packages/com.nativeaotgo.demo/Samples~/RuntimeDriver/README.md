# Runtime Driver Sample

把 `FishingRuntimeDriver` 挂到任意 `MonoBehaviour` 节点上，并给 `lureTransform` 指定一个诱饵 Transform，即可在 `LatestSnapshot` 中观察 Core 规则输出。

这个 sample 仍然是演示性质，目的是说明 Unity 应直接复用 Core 源码，而不是通过 ABI 调 DLL。
