module GeometrySandbox.Views.Viewport

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Geometry

open GeometrySandbox


let pictureView size =
    Canvas.create [
        Canvas.height (Length.inCssPixels size.Height)
        Canvas.width (Length.inCssPixels size.Width)
        Canvas.background Theme.palette.pageColor
    ]

let view size : IView =
    let pictureWithDropShadow =
        Border.create [
            Border.height (Length.inCssPixels size.Height)
            Border.width (Length.inCssPixels size.Width)
            Border.child (pictureView size)
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
