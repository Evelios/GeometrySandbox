module GeometrySandbox.Views.Viewport

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Geometry

open GeometrySandbox


let pictureView height width =
    Canvas.create [
        Canvas.height height 
        Canvas.width width
        Canvas.background Theme.palette.pageColor
    ]

let view model : IView =
    let height =(Length.inCssPixels model.Size.Height) *  model.ViewScale
    let width = (Length.inCssPixels model.Size.Width) * model.ViewScale
    
    let pictureWithDropShadow =
        Border.create [
            Border.height height
            Border.width  width
            Border.child (pictureView height width)
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
