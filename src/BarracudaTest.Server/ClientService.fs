namespace BarracudaTest.Server

open System
open System.IO
open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.AspNetCore.Hosting
open Bolero
open Bolero.Remoting
open Bolero.Remoting.Server
open BarracudaTest

type ClientService(ctx: IRemoteContext, env: IWebHostEnvironment) =
    inherit RemoteHandler<Client.ClientPage.ClientService>()

    let dataPath = Path.Combine(env.ContentRootPath, "data/clients.json")
    let clients () =
        let json = dataPath |> File.ReadAllText
        JsonSerializer.Deserialize<Client.Structure.Client[]>(json)
        |> ResizeArray
    let jsonSerialiserOptions =
        let options = JsonSerializerOptions()
        options.WriteIndented <- true
        options

    override this.Handler =
        {
            getClients = ctx.Authorize <| fun () -> async {
                return clients().ToArray()
            }
            saveClient = ctx.Authorize <| fun (client: Client.Structure.Client) -> async {
                let readClients = clients()
                readClients.Add(client) |> ignore

                let json = JsonSerializer.Serialize(readClients, jsonSerialiserOptions)
                printfn "Save %A" clients
                do! File.WriteAllTextAsync(dataPath, json) |> Async.AwaitTask
            }
        }
