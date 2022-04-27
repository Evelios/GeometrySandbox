module GeometrySandbox.Views.Viewport

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Media
open Geometry

open GeometrySandbox
open GeometrySandbox.Extensions


let pictureView model viewBoxHeight viewBoxWidth =
    let canvasHeight = Length.inCssPixels model.Size.Height
    let canvasWidth = Length.inCssPixels model.Size.Width

    ViewBox.create [
        Viewbox.stretch Stretch.Uniform
        Viewbox.width viewBoxWidth
        Viewbox.height viewBoxHeight
        Viewbox.child (
            Canvas.create [
                Canvas.height canvasHeight
                Canvas.width canvasWidth
                Canvas.background Theme.palette.pageColor
            ]
        )
    ]


let view model : IView =
    let height, width =
        Size2D.scale model.ViewScale model.Size
        |> Size2D.dimensions
        |> Tuple2.mapBoth Length.inCssPixels
        
    let pictureWithDropShadow =
        Border.create [
            Border.height height
            Border.width width
            Border.child (pictureView model height width)
            Border.boxShadow (Theme.boxShadow Theme.palette.canvasBackgroundShadow)
        ]

    DockPanel.create [
        DockPanel.background Theme.palette.canvasBackground
        DockPanel.children [
            Border.create [
                Border.child pictureWithDropShadow
                Border.boxShadow (Theme.boxShadowInset Theme.palette.canvasBackgroundShadow)
            ]
        ]
    ]
    :> IView
