module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Elmish

open GeometrySandbox.Views

type Model = { Orientation: Orientation }

type Msg =
    | TopIconBarMsg of TopIconBar.Msg
    | PropertiesMsg of Properties.Msg

let init () : Model * Cmd<Msg> = { Orientation = Portrait }, Cmd.none

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with

    | TopIconBarMsg topIconBarMsg ->
        match topIconBarMsg with
        | TopIconBar.Save -> model, Cmd.none
        | TopIconBar.ToggleRuler -> model, Cmd.none

    | PropertiesMsg propertiesMsg ->
        match propertiesMsg with
        | Properties.ChangeOrientation orientation -> { model with Orientation = orientation }, Cmd.none

let propertiesModel (model: Model) : Properties.Model = { Orientation = model.Orientation }

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.background Theme.palette.panelBackground
        DockPanel.children [

            TopIconBar.view (TopIconBarMsg >> dispatch)
            |> DockPanel.child Dock.Top

            Properties.view (propertiesModel model) (PropertiesMsg >> dispatch)
            |> DockPanel.child Dock.Right

            Viewport.view
        ]
    ]
    :> IView
