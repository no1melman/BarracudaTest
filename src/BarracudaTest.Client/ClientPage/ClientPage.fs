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

let updateClient (update: Client -> Client) (clientPageModel: ClientPageModel) =
    {
        clientPageModel with client = update clientPageModel.client
    }

let updateClients (update: ClientPageModel -> ClientPageModel) (model: Model) : Model =
    match model.page with
    | Clients model' ->  
        printfn "updated client" |> ignore
        let innerModel = update model'.Model
        { model with page = Clients { Model = innerModel }; client = innerModel  }
    | _ -> model

let defaultModel() : ClientPageModel = 
    {
        clients = None
        client = { name = ""; dob = DateTimeOffset.Parse("1990-11-07"); address = { line1 = ""; line2 = ""; town = ""; postcode = "" } }
    }

let updateClientPage (clientService: ClientService) message model =
    match message with
    | GetClients ->
        let cmd = Cmd.OfAsync.either clientService.getClients () (GotClients >> ClientMessages) Error
        { model with books = None }, cmd
    | GotClients clients ->
        let model = model |> updateClients (fun client -> { client with clients = Some clients })
        noCmd model
    | SetName name ->
        let model = model |>  updateClients (fun client -> updateClient (fun client -> { client with name = name }) client)
        noCmd model
    | SetDob dob ->
        let model = model |>  updateClients (fun client -> updateClient (fun client -> { client with dob = dob }) client)
        noCmd model
    | SetLine1 line1 ->
        let model = model |>  updateClients (fun client -> updateClient (fun client -> { client with address = { client.address with line1 = line1 }}) client)
        noCmd model
    | SetLine2 line2 ->
        let model = model |>  updateClients (fun client -> updateClient (fun client -> { client with address = { client.address with line2 = line2 }}) client)
        noCmd model
    | SetTown town ->
        let model = model |>  updateClients (fun client -> updateClient (fun client -> { client with address = { client.address with town = town }}) client)
        noCmd model
    | SetPostcode pc ->
        let model = model |>  updateClients (fun client -> updateClient (fun client -> { client with address = { client.address with postcode = pc }}) client)
        noCmd model
    | ClearClient ->
        let model = model |>  updateClients (fun client -> updateClient (fun _ -> defaultModel().client) client)
        noCmd model
    | SaveClient client ->
        let cmd = Cmd.OfAsync.either clientService.saveClient (client) (fun _ -> ClientMessages GetClients) Error
        model, cmd

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
        .Name(model.client.name, fun n ->  dispatch (SetName n |> ClientMessages))
        .Dob(
            model.client.dob.ToString("yyyy-MM-dd"), 
            fun n -> 
                let dt = DateTimeOffset.Parse(n)
                dispatch (SetDob dt |> ClientMessages)
        )
        .Line1(model.client.address.line1, fun n -> dispatch (SetLine1 n |> ClientMessages))
        .Line2(model.client.address.line2, fun n -> dispatch (SetLine2 n |> ClientMessages))
        .Town(model.client.address.town, fun n -> dispatch (SetTown n |> ClientMessages))
        .Postcode(model.client.address.postcode, fun n -> dispatch (SetPostcode n |> ClientMessages))
        .Submit(fun _ -> dispatch (SaveClient model.client |> ClientMessages))
        .Cancel(fun _ -> dispatch (ClientMessages ClearClient))
        .Elt()