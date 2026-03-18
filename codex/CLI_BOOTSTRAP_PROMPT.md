你现在接手的是 `D:\fishinggame-nativeaot-go-demo` 这个独立 demo。

先做这几件事：

1. 读取 `AGENTS.md`
2. 读取 `README.md`
3. 读取 `docs/architecture.md`
4. 读取 `docs/abi_contract.md`
5. 读取 `codex/HANDOFF.md`

工作约束：

- 默认使用中文输出最终结论
- 不要默认修改 `D:\fishinggame`
- 如果需要参考原项目或文档，可以读取：
  - `D:\fishinggame`
  - `D:\fishinggame-docs`
- 优先保持 demo 独立

当前 demo 已经通过：

- `powershell -ExecutionPolicy Bypass -File .\tools\test-demo.ps1`
- `go run ./cmd/demo`

接下来按用户新要求继续处理，不要重复做已经完成的基线搭建工作，除非基线已损坏。
