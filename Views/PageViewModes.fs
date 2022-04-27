module GeometrySandbox.Views.PageViewModes

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Input
open Avalonia.Media
open Geometry

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg = Action of Action

// ---- Internal Functions ----

let page canvasSize viewSize : IView =
    let viewHeight, viewWidth =
        viewSize
        |> Size2D.dimensions
        |> Tuple2.mapBoth Length.inCssPixels

    let canvasHeight, canvasWidth =
        canvasSize
        |> Size2D.dimensions
        |> Tuple2.mapBoth Length.inCssPixels

    let canvasView =
        ViewBox.create [
            Viewbox.stretch Stretch.Uniform
            Viewbox.height viewHeight
            Viewbox.width viewWidth
            Viewbox.child (
                Canvas.create [
                    Canvas.height canvasHeight
                    Canvas.width canvasWidth
                    Canvas.background Theme.palette.pageColor
                ]
            )
        ]

    Border.create [
        Border.height viewHeight
        Border.width viewWidth
        Border.child canvasView
        Border.boxShadow (Theme.boxShadow Theme.palette.canvasBackgroundShadow)
        Border.margin Theme.spacing.large
    ]
    :> IView

let multiplePageView model : IView =
    let pageScale = 1. / 3.
    let numPages = 10

    let singlePage =
        page model.Size (Size2D.scale pageScale model.Size)

    let pages = List.replicate numPages singlePage

    WrapPanel.create [
        WrapPanel.margin Theme.spacing.huge
        WrapPanel.children pages
    ]
    :> IView

// ---- Page Views ----

let view model dispatch : IView =
    let pageView =
        match model.PageViewMode with
        | PageViewMode.SinglePage -> page model.Size (Size2D.scale model.ViewScale model.Size)
        | PageViewMode.MultiplePages -> multiplePageView model
        | PageViewMode.FramedPage -> page model.Size (Size2D.scale model.ViewScale model.Size)
        | PageViewMode.FullScreen -> page model.Size (Size2D.scale model.ViewScale model.Size)

    DockPanel.create [
        DockPanel.background Theme.palette.canvasBackground
        DockPanel.children [
            Border.create [
                Border.child pageView
                Border.boxShadow (Theme.boxShadowInset Theme.palette.canvasBackgroundShadow)
            ]
        ]
        DockPanel.onPointerWheelChanged (fun e ->
            printfn $"{e.Delta}"
            if e.Delta.Y > 0. then
                Action Action.ZoomIn  |> dispatch
            if e.Delta.Y < 0. then
                Action Action.ZoomOut  |> dispatch
                
            )
    ]
    :> IView
    
