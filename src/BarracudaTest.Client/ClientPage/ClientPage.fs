module BarracudaTest.Client.ClientPage

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

type ClientPage = Template<"ClientPage/client.html">

type Address =
    {
        line1: string
        line2: string
        town: string
        postcode: string
    }

type Client =
    {
        name: string
        dob: DateTimeOffset
        address: Address
    }


type ClientService = 
    {
        getClients: unit -> Async<Client[]>
    }
    
    interface IRemoteService with
        member this.BasePath = "/clients"


type ClientPageModel = 
    {
        clients: Client[] option
    }

let defaultModel() = 
    {
        clients = None
    }

let clientPage (model: ClientPageModel) =
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
        .Elt()