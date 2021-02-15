namespace JupyterDispatcher


[<AutoOpen>]
module JupyterDispatcher =
    open System
    open System.Threading
    open System.Threading.Tasks

    let private queue = new System.Collections.Concurrent.BlockingCollection<Func<obj> * TaskCompletionSource<obj>>()

    let private startProcessing() =
        let m() = 
            while true do
                let f, tcs = queue.Take()

                try
                    let res = f.Invoke()
                    tcs.SetResult(res)
                with
                | ex -> tcs.SetException(ex)
        System.Threading.Tasks.Task.Run(fun () -> m())

    do 
        startProcessing() |> ignore

    let run (f: unit -> 'T) : 'T =
        let tcs = TaskCompletionSource<obj>()
        queue.Add((Func<obj>(fun () -> f() :> obj ), tcs))
        tcs.Task.Result :?> 'T