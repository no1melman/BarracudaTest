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

type ClientPageModel = 
    {
        clients: Client list
    }

let clientPage =
    ClientPage()
        .Elt()