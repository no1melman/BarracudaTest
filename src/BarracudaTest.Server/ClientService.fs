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

    let clients =
        let json = Path.Combine(env.ContentRootPath, "data/clients.json") |> File.ReadAllText
        JsonSerializer.Deserialize<Client.ClientPage.Client[]>(json)
        |> ResizeArray

    override this.Handler =
        {
            getClients = ctx.Authorize <| fun () -> async {
                return clients.ToArray()
            }
        }
