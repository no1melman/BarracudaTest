module BarracudaTest.Client.ClientPage

open System
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting
open Bolero.Remoting.Client
open Bolero.Templating.Client

type ClientPage = Template<"ClientPage/client.html">

let clientPage =
    ClientPage()
        .Who(b [] [text "Who"])
        .Is(b [] [text "Is"])
        .Elt()