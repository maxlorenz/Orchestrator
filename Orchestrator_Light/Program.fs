module orchestrator_light

open System
open System.Diagnostics
open System.Threading

let exec command options callback = 
    let psi = new ProcessStartInfo(command, options)
    let proc = new Process(StartInfo = psi, EnableRaisingEvents = true)

    psi.UseShellExecute <- false 
    psi.RedirectStandardOutput <- true
    psi.RedirectStandardError <- true 
    psi.CreateNoWindow <- true
    psi.StandardOutputEncoding <- System.Text.Encoding.GetEncoding(437)
    
    proc.OutputDataReceived.Add(callback)
    proc.Start() |> ignore
    proc.BeginOutputReadLine()
    proc.BeginErrorReadLine()
    proc.WaitForExit()

let onlyMyComputer (line: string) = line.StartsWith(Environment.MachineName)
let removeIdentifier (line: string) = line.Remove(0, line.IndexOf(",") + 1)
let isTime (line: string) = line.StartsWith("time")
let execAndPrint (cmd: string) = exec "cmd" ("/C " + cmd) (fun data -> Console.WriteLine data.Data)

let getCommands() = IO.File.ReadLines(@"orchestrator.txt") |> Seq.filter(onlyMyComputer) |> Seq.map(removeIdentifier)
let getWaitingTime() = IO.File.ReadLines(@"orchestrator.txt") |> Seq.filter(isTime) |> Seq.map(removeIdentifier) |> Seq.head

[<EntryPoint>]
let main argv = 
    while (true) do
        for command in getCommands() do execAndPrint command
        Thread.Sleep(Int32.Parse(getWaitingTime()))

    0 // return an integer exit code
