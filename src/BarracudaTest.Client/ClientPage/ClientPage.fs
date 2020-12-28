module BarracudaTest.Client.ClientPage

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

open Structure
open Messages

type ClientPage = Template<"ClientPage/client.html">

type ClientService = 
    {
        getClients: unit -> Async<Client[]>
        saveClient: Client -> Async<unit>
    }
    
    interface IRemoteService with
        member this.BasePath = "/clients"


let defaultModel() : ClientPageModel = 
    {
        clients = None
        client = { name = ""; dob = DateTimeOffset.Parse("1990-11-07"); address = { line1 = ""; line2 = ""; town = ""; postcode = "" } }
    }

let clientPage (model: ClientPageModel) dispatch =
    printfn "Client model :: %A" model |> ignore
    ClientPage()
        .Rows(cond model.clients <| function
            | None -> ClientPage.EmptyRow().Elt()
            | Some clients -> 
                forEach clients <| fun client ->
                    tr [] [
                        td [] [text client.name]
                        td [] [text (client.dob.ToString("yyyy-MM-dd"))]
                        td [] [text client.address.line1]
                        td [] [text ""]
                    ])
        .Name(model.client.name, fun n ->  dispatch (SetName n))
        .Dob(
            model.client.dob.ToString("yyyy-MM-dd"), 
            fun n -> 
                let dt = DateTimeOffset.Parse(n)
                dispatch (SetDob dt)
        )
        .Line1(model.client.address.line1, fun n -> dispatch (SetLine1 n))
        .Line2(model.client.address.line2, fun n -> dispatch (SetLine2 n))
        .Town(model.client.address.town, fun n -> dispatch (SetTown n))
        .Postcode(model.client.address.postcode, fun n -> dispatch (SetPostcode n))
        .Submit(fun _ -> dispatch (SaveClient model.client))
        .Cancel(fun _ -> dispatch ClearClient)
        .Elt()