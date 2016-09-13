 
(**********************************************************************************************************************
*** SOURCE: https://github.com/swlaschin/low-risk-ways-to-use-fsharp-at-work/blob/master/dev-website-responding.fsx ***
**********************************************************************************************************************)

open FSharp.Data

let queryServer uri queryParams =
  try
    let response = Http.Request(uri, query = queryParams, silentHttpErrors = true)
    Some response
  with
    | :? System.Net.WebException as ex -> None

let sendAlert uri message =
  printfn "Error for %s. Message=%O" uri message

let checkServer (uri, queryParams) =
  match queryServer uri queryParams with
    | Some response ->
      printfn "Response for %s is %O" uri response.StatusCode
      if (response.StatusCode <> 200) then
        sendAlert uri response.StatusCode
    | None ->
      sendAlert uri "No response"

(*****************
*** END SOURCE ***
*****************)

[<EntryPoint>]
let main argv =
  checkServer ("http://StrubT.ch", [])
  0
