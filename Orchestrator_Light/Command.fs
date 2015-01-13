module Command

open System.Diagnostics

let Exec command options callback = 
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