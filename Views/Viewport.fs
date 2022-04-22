module GeometrySandbox.Views.Viewport

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types

open GeometrySandbox


let pictureView =
    Canvas.create [
        Canvas.height 400.
        Canvas.width 600.
        Canvas.background Theme.palette.pageColor
    ]

let view () : IView =
    let pictureWithDropShadow =
        Border.create [
            Border.height 400.
            Border.width 600.
            Border.child pictureView
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
