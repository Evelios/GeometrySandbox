module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Elmish

type Model = { count: int }

type Msg =
    | Increment
    | Decrement
    | Reset

let init () : Model * Cmd<Msg> = { count = 0 }, Cmd.none


let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | Increment -> { model with count = model.count + 1 }, Cmd.none
    | Decrement -> { model with count = model.count - 1 }, Cmd.none
    | Reset -> init ()

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.children [
            Button.create [
                Button.dock Dock.Bottom
                Button.onClick (fun _ -> dispatch Reset)
                Button.content "reset"
            ]
            Button.create [
                Button.dock Dock.Bottom
                Button.onClick (fun _ -> dispatch Decrement)
                Button.content "-"
            ]
            Button.create [
                Button.dock Dock.Bottom
                Button.onClick (fun _ -> dispatch Increment)
                Button.content "+"
            ]
            TextBlock.create [
                TextBlock.dock Dock.Top
                TextBlock.fontSize 48.0
                TextBlock.verticalAlignment VerticalAlignment.Center
                TextBlock.horizontalAlignment HorizontalAlignment.Center
                TextBlock.text (string model.count)
            ]
        ]
    ]
    :> IView
