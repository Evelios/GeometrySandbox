module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Elmish

open GeometrySandbox.Views
open GeometrySandbox.Extensions

type Model = { count: int }

type Msg = TopIconBarMsg of TopIconBar.Msg

let init () : Model * Cmd<Msg> = { count = 0 }, Cmd.none


let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | TopIconBarMsg topIconBarMsg -> model, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.children [
            TopIconBar.view (TopIconBarMsg >> dispatch) |> DockPanel.child Dock.Top
        ]
    ]
    :> IView
