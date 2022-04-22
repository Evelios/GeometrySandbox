module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Elmish

open GeometrySandbox.Views

type Model = { count: int }

type Msg =
    | TopIconBarMsg of TopIconBar.Msg
    | PropertiesMsg of Properties.Msg

let init () : Model * Cmd<Msg> = { count = 0 }, Cmd.none


let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | TopIconBarMsg topIconBarMsg -> model, Cmd.none
    | PropertiesMsg propertiesMsg -> model, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.background Theme.palette.panelBackground
        DockPanel.children [

            TopIconBar.view (TopIconBarMsg >> dispatch)
            |> DockPanel.child Dock.Top

            Properties.view (PropertiesMsg >> dispatch)
            |> DockPanel.child Dock.Right

            Viewport.view
        ]
    ]
    :> IView
