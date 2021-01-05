module BarracudaTest.Client.Structure

open System
open Bolero
open Elmish

let noCmd model =
    model, Cmd.none

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

type ClientPageModel = 
    {
        clients: Client[] option
        client: Client
    }

/// Routing endpoints definition.
type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/counter">] Counter
    | [<EndPoint "/data">] Data
    | [<EndPoint "/clients">] Clients of PageModel<ClientPageModel>

/// The Elmish application's model.
type Model =
    {
        page: Page
        counter: int
        books: Book[] option
        client: ClientPageModel
        error: string option
        username: string
        password: string
        signedInAs: option<string>
        signInFailed: bool
    }

and Book =
    {
        title: string
        author: string
        publishDate: DateTime
        isbn: string
    }

