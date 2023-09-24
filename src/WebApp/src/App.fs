module App

open Elmish
open Elmish.React
open Feliz
open PenPlotter

// MODEL

type Model = int

type Msg =
    | Increment
    | Decrement

let init () : Model = 0

// UPDATE

let update (msg: Msg) (model: Model) =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1

// VIEW (rendered with React)

let view (model: Model) dispatch =
    Html.div [
        Html.h1 "This is my Feliz-Fable App!"
    ]

// App
Program.mkSimple init update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run
