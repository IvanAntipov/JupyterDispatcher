# JupyterDispatcher

Helpers for invoke actions in single thread. (Workaround to use [pythonnet](https://github.com/henon/pythonnet_netstandard) from [dotnet interactive](https://github.com/dotnet/interactive) )

// Cell 1

```
#r "nuget: JupyterDispatcher, 1.0.0"
#r "nuget: pythonnet_netstandard_py38_linux, 2.5.1.1"
#r "nuget: FSharp.Interop.Dynamic, 5.0.1.268"

open Python.Runtime
open FSharp.Interop.Dynamic

open System
open System.IO

open JupyterDispatcher.JupyterDispatcher
```

// Cell 2

```
let res = 
    (fun () ->
            PythonEngine.Initialize() |> ignore
            PythonEngine.BeginAllowThreads() |> ignore
            "dd") 
    |> run 

res
```

// Cell 3

```
let res2 = 
    (fun () -> 
        use gil = Py.GIL() 
        let m = PythonEngine.ImportModule("builtins")
        m.ToString())
    |> run

res2
```