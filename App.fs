module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Elmish
open Geometry

open GeometrySandbox.Views
open GeometrySandbox.Extensions

type Model = { Size: Size2D<Meters>; Seed: int }

type Msg =
    | TopIconBarMsg of TopIconBar.Msg
    | PropertiesMsg of Properties.Msg

let init () : Model * Cmd<Msg> =
    { Size = Size2D.create (Length.cssPixels 600.) (Length.cssPixels 400.)
      Seed = 0 },
    Cmd.none

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with

    | TopIconBarMsg topIconBarMsg ->
        match topIconBarMsg with
        | TopIconBar.Save -> model, Cmd.none
        | TopIconBar.ToggleRuler -> model, Cmd.none

    | PropertiesMsg propertiesMsg ->
        match propertiesMsg with
        | Properties.ChangeOrientation orientation ->
            { model with
                  Size = Size2D.setOrientation orientation model.Size },
            Cmd.none

        | Properties.ChangeHeight newHeightString ->
            match String.toInt newHeightString with
            | Some newHeight ->
                { model with
                      Size = Size2D.setHeight (Length.cssPixels (float newHeight)) model.Size },
                Cmd.none
            | None -> model, Cmd.none

        | Properties.ChangeWidth newWidthString ->
            match String.toInt newWidthString with
            | Some newWidth ->
                { model with
                      Size = Size2D.setWidth (Length.cssPixels (float newWidth)) model.Size },
                Cmd.none
            | None -> model, Cmd.none

        | Properties.ChangeSeed newSeed -> { model with Seed = newSeed }, Cmd.none

let propertiesModel (model: Model) : Properties.Model =
    { Size =  model.Size
      Seed = model.Seed }

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.background Theme.palette.panelBackground
        DockPanel.children [

            TopIconBar.view (TopIconBarMsg >> dispatch)
            |> DockPanel.child Dock.Top

            Properties.view (propertiesModel model) (PropertiesMsg >> dispatch)
            |> DockPanel.child Dock.Right

            Viewport.view model.Size
        ]
    ]
    :> IView
