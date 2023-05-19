module GeometrySandbox.Views.PageViewModes

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Media
open Math.Geometry
open Math.Units

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg = Action of Action

// ---- Internal Functions ----

let page (generator: SimpleGenerator) canvasSize viewSize : IView =
    let viewHeight, viewWidth =
        viewSize |> Size2D.dimensions |> Tuple2.mapBoth Length.inCssPixels

    let canvasHeight, canvasWidth =
        canvasSize |> Size2D.dimensions |> Tuple2.mapBoth Length.inCssPixels

    let canvasContent = generator ()

    let canvasView =
        Viewbox.create
            [ Viewbox.stretch Stretch.Uniform
              Viewbox.height viewHeight
              Viewbox.width viewWidth
              Viewbox.child (
                  Canvas.create
                      [ Canvas.height canvasHeight
                        Canvas.width canvasWidth
                        Canvas.background Theme.palette.pageColor
                        Canvas.children [ canvasContent ] ]
              ) ]

    Border.create
        [ Border.height viewHeight
          Border.width viewWidth
          Border.child canvasView
          Border.boxShadow (Theme.boxShadow Theme.palette.canvasBackgroundShadow)
          Border.margin Theme.spacing.large ]
    :> IView

let multiplePageView generator model : IView =
    let pageScale = 1. / 3.
    let numPages = 10

    let singlePage = page generator model.Size (Size2D.scale pageScale model.Size)

    let pages = List.replicate numPages singlePage

    WrapPanel.create [ WrapPanel.margin Theme.spacing.huge; WrapPanel.children pages ] :> IView

// ---- Page Views ----

let view (generator: SimpleGenerator) (model: Model) (dispatch: Msg -> unit) : IView =
    let pageView =
        match model.PageViewMode with
        | PageViewMode.SinglePage -> page generator model.Size (Size2D.scale model.ViewScale model.Size)
        | PageViewMode.MultiplePages -> multiplePageView generator model
        | PageViewMode.FramedPage -> page generator model.Size (Size2D.scale model.ViewScale model.Size)
        | PageViewMode.FullScreen -> page generator model.Size (Size2D.scale model.ViewScale model.Size)

    DockPanel.create
        [ DockPanel.background Theme.palette.canvasBackground
          DockPanel.children
              [ Border.create
                    [ Border.child pageView
                      Border.boxShadow (Theme.boxShadowInset Theme.palette.canvasBackgroundShadow) ] ]
          DockPanel.onPointerWheelChanged (fun e ->
              if e.Delta.Y > 0. then
                  e.Handled <- true
                  Action Action.ZoomIn |> dispatch

              else if e.Delta.Y < 0. then
                  e.Handled <- true
                  Action Action.ZoomOut |> dispatch

          ) ]
    :> IView
