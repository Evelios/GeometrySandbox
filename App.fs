module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Elmish
open Geometry

open GeometrySandbox.Views
open GeometrySandbox.Extensions

type Msg =
    | TopIconBarMsg of TopIconBar.Msg
    | PropertiesMsg of Properties.Msg
    | Action of Action

let init () : Model * Cmd<Msg> =
    { Size = Size2D.create (Length.cssPixels 600.) (Length.cssPixels 400.)
      Unit = LengthUnit.Pixels
      Seed = 0 },
    Cmd.none


// ---- Update -----------------------------------------------------------------


let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with

    | Action action ->
        match action with

        | Action.ChangeOrientation orientation ->
            { model with
                  Size = Size2D.setOrientation orientation model.Size },
            Cmd.none

        | Action.ChangeHeight height ->
            { model with
                  Size = Size2D.setHeight (Length.ofUnit model.Unit height) model.Size },
            Cmd.none

        | Action.ChangeWidth width ->
            { model with
                  Size = Size2D.setWidth (Length.ofUnit model.Unit width) model.Size },
            Cmd.none

        | Action.ChangeSeed seed -> { model with Seed = seed }, Cmd.none
        
        | Action.ChangeUnit unit -> { model with Unit = unit }, Cmd.none



    // ---- Components ----

    | TopIconBarMsg topIconBarMsg ->
        match topIconBarMsg with

        | TopIconBar.Save -> model, Cmd.none
        | TopIconBar.ToggleRuler -> model, Cmd.none


    | PropertiesMsg propertiesMsg ->
        match propertiesMsg with
        | Properties.Action action -> model, Cmd.ofMsg (Action action)

        | Properties.ChangeHeight heightString ->
            match String.toFloat heightString with
            | Some height -> model, Cmd.ofMsg (Action.ChangeHeight height |> Action)
            | None -> model, Cmd.none

        | Properties.ChangeWidth widthString ->
            match String.toFloat widthString with
            | Some width -> model, Cmd.ofMsg (Action.ChangeWidth width |> Action)
            | None -> model, Cmd.none

        | Properties.ChangeSeed seed -> model, Cmd.ofMsg (Action.ChangeSeed seed |> Action)

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.background Theme.palette.panelBackground
        DockPanel.children [

            TopIconBar.view (TopIconBarMsg >> dispatch)
            |> DockPanel.child Dock.Top

            Properties.view model (PropertiesMsg >> dispatch)
            |> DockPanel.child Dock.Right

            Viewport.view model.Size
        ]
    ]
    :> IView
