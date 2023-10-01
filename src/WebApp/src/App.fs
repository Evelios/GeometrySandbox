module App

open Elmish
open Elmish.React
open Feliz
open SharpVG

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

let svgView: ReactElement =
    let position = Point.ofInts (10, 10)
    let area = Area.ofInts (50, 50)

    let style =
        Style.create (Color.ofName Colors.Cyan) (Color.ofName Colors.Blue) (Length.ofInt 3) 1.0 1.0

    let svgElement = Rect.create position area |> Element.createWithStyle style

    Html.div [ prop.dangerouslySetInnerHTML $"{svgElement}" ]

let view (model: Model) dispatch = Html.div [ svgView ]

// App
Program.mkSimple init update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run
