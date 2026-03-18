# ABI 契约

当前 demo 暴露 4 个入口：

```c
int32_t calc_step(
    const StepInputNative* input,
    StepOutputNative* output,
    ErrorInfoNative* err);

int32_t session_create(
    const SessionConfigNative* config,
    uint64_t* session,
    ErrorInfoNative* err);

int32_t session_step(
    uint64_t session,
    const SessionStepInputNative* input,
    StepOutputNative* output,
    ErrorInfoNative* err);

int32_t session_destroy(
    uint64_t session,
    ErrorInfoNative* err);
```

## 通用约定

- 返回值为 `0` 表示桥接层调用成功。
- 返回值非 `0` 表示桥接失败，错误详情写入 `ErrorInfoNative`。
- 所有 native struct 都必须保持顺序布局。
- 不跨 ABI 传递字符串、托管对象、容器或回调。

## 错误码

- `0`：成功
- `1000`：空指针
- `2000`：输入校验失败，`detail` 对应核心层 `ValidationErrorDetail`
- `3000`：非法或失效的 session handle
- `9000`：未预期异常

## SessionConfigNative

```c
typedef struct SessionConfigNative {
    uint64_t cast_id;
    uint64_t seed_base;
    int32_t  pose_index;
    int32_t  initial_sample_slot;
    float    initial_tension;
    float    water_temp;
} SessionConfigNative;
```

## SessionStepInputNative

```c
typedef struct SessionStepInputNative {
    float   dt;
    float   lure_depth;
    float   lure_speed;
    int32_t action_flags;
} SessionStepInputNative;
```
