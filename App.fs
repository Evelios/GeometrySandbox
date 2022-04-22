module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Elmish
open Geometry

open GeometrySandbox.Views
open GeometrySandbox.Extensions

type Model =
    { Orientation: Orientation
      Width: Length<Meters>
      Height: Length<Meters>
      Seed: int }

type Msg =
    | TopIconBarMsg of TopIconBar.Msg
    | PropertiesMsg of Properties.Msg

let init () : Model * Cmd<Msg> =
    { Orientation = Portrait
      Width = Length.cssPixels 600.
      Height = Length.cssPixels 400.
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
        | Properties.ChangeOrientation orientation -> { model with Orientation = orientation }, Cmd.none
        | Properties.ChangeHeight newHeightString ->
            match String.toFloat newHeightString with
            | Some newHeight ->
                { model with
                      Height = Length.cssPixels newHeight },
                Cmd.none
            | None -> model, Cmd.none

        | Properties.ChangeWidth newWidthString ->
            match String.toFloat newWidthString with
            | Some newWidth ->
                { model with
                      Width = Length.cssPixels newWidth },
                Cmd.none
            | None -> model, Cmd.none

        | Properties.ChangeSeed newSeed -> { model with Seed = newSeed }, Cmd.none

let propertiesModel (model: Model) : Properties.Model =
    { Orientation = model.Orientation
      Height = model.Height
      Width = model.Width
      Seed = model.Seed }

let view (model: Model) (dispatch: Msg -> unit) : IView =
    DockPanel.create [
        DockPanel.background Theme.palette.panelBackground
        DockPanel.children [

            TopIconBar.view (TopIconBarMsg >> dispatch)
            |> DockPanel.child Dock.Top

            Properties.view (propertiesModel model) (PropertiesMsg >> dispatch)
            |> DockPanel.child Dock.Right

            Viewport.view ()
        ]
    ]
    :> IView
