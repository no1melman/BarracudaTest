module BarracudaTest.Client.Messages

open System

open Structure

type ClientMessage = 
    | GetClients
    | GotClients of Client[]
    | SetName of name: string
    | SetDob of dob: DateTimeOffset
    | SetLine1 of line1: string
    | SetLine2 of line2: string
    | SetTown of town: string
    | SetPostcode of postcode: string
    | SaveClient of Client
    | ClearClient

type Message =
    | SetPage of Page
    | Increment
    | Decrement
    | SetCounter of int
    | GetBooks
    | GotBooks of Book[]
    | SetUsername of string
    | SetPassword of string
    | GetSignedInAs
    | RecvSignedInAs of option<string>
    | SendSignIn
    | RecvSignIn of option<string>
    | SendSignOut
    | RecvSignOut
    | Error of exn
    | ClearError
    | ClientMessages of ClientMessage