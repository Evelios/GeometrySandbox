module GeometrySandbox.App

open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Elmish
open Math.Geometry
open Math.Geometry.Avalonia
open Math.Units
open Math.Units.Interval

open GeometrySandbox.Views
open GeometrySandbox.Extensions


// ---- Messages ---------------------------------------------------------------

type Msg =
    | TopIconBarMsg of TopIconBar.Msg
    | PropertiesMsg of Properties.Msg
    | PageViewModesMsg of PageViewModes.Msg
    | Action of Action


// ---- Initialize -------------------------------------------------------------

[<Literal>]
let zoomAmount = 0.1

let generator (size: Size2D<Meters, 'Coordinates>) () : IView =
    let margin = Length.inches 1.

    let lineFromX x =
        LineSegment2D.from (Point2D.xy x margin) (Point2D.xy x (size.Height - margin))

    let startPoints = Interval.linspace margin (size.Width - margin) 100

    let verticalLines = Seq.map lineFromX startPoints

    let drawLine line : IView =
        Line.draw line [ Line.strokeThickness 2.; Line.stroke "#000000" ]

    let drawnLines: IView list = Seq.map drawLine verticalLines |> Seq.toList

    Canvas.create [ Canvas.children drawnLines ]



let init () : Model * Cmd<Msg> =
    { Size = Size2D.create (Length.cssPixels 600.) (Length.cssPixels 400.)
      Unit = LengthUnit.Pixels
      Seed = 0
      ViewScale = 1.
      PageViewMode = PageViewMode.SinglePage },

    Cmd.none


// ---- Update -----------------------------------------------------------------

let takeAction (action: Action) (model: Model) : Model =
    match action with
    | Action.ChangePageViewMode pageViewMode ->
        { model with
            PageViewMode = pageViewMode }

    | Action.ChangeOrientation orientation ->
        { model with
            Size = Size2D.setOrientation orientation model.Size }

    | Action.ChangeHeight height ->
        { model with
            Size = Size2D.setHeight (Length.ofUnit model.Unit height) model.Size }

    | Action.ChangeWidth width ->
        { model with
            Size = Size2D.setWidth (Length.ofUnit model.Unit width) model.Size }

    | Action.ChangeSeed seed -> { model with Seed = seed }

    | Action.ChangeUnit unit -> { model with Unit = unit }

    | Action.ZoomIn ->
        { model with
            ViewScale = min 1.5 (model.ViewScale + zoomAmount) }

    | Action.ZoomOut ->
        { model with
            ViewScale = max zoomAmount (model.ViewScale - zoomAmount) }

    | Action.ZoomToFullSize -> { model with ViewScale = 1. }


let topIconBarMsgHandler (msg: TopIconBar.Msg) : Cmd<Msg> =
    match msg with
    | TopIconBar.Action action -> Cmd.ofMsg (Action action)
    | TopIconBar.Save -> Cmd.none
    | TopIconBar.ToggleRuler -> Cmd.none


let propertiesMsgHandler (msg: Properties.Msg) : Cmd<Msg> =
    match msg with
    | Properties.Action action -> Cmd.ofMsg (Action action)

    | Properties.ChangeHeight heightString ->
        match String.toFloat heightString with
        | Some height -> Cmd.ofMsg (Action.ChangeHeight height |> Action)
        | None -> Cmd.none

    | Properties.ChangeWidth widthString ->
        match String.toFloat widthString with
        | Some width -> Cmd.ofMsg (Action.ChangeWidth width |> Action)
        | None -> Cmd.none

    | Properties.ChangeSeed seed -> Cmd.ofMsg (Action.ChangeSeed seed |> Action)

let pageViewModesMsgHandler (msg: PageViewModes.Msg) : Cmd<Msg> =
    match msg with
    | PageViewModes.Action action -> Cmd.ofMsg (Action action)


let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with

    // ---- Components ----
    | TopIconBarMsg topIconBarMsg -> model, topIconBarMsgHandler topIconBarMsg
    | PropertiesMsg propertiesMsg -> model, propertiesMsgHandler propertiesMsg
    | PageViewModesMsg pageViewModesMsg -> model, pageViewModesMsgHandler pageViewModesMsg

    // ---- Actions ----
    | Action action -> takeAction action model, Cmd.none


let view (model: Model) (dispatch: Msg -> unit) : IView =

    DockPanel.create
        [ DockPanel.background Theme.palette.panelBackground
          DockPanel.children
              [

                TopIconBar.view (TopIconBarMsg >> dispatch) |> DockPanel.child Dock.Top

                Properties.view model (PropertiesMsg >> dispatch) |> DockPanel.child Dock.Right

                PageViewModes.view (generator model.Size) model (PageViewModesMsg >> dispatch) ] ]
    :> IView
