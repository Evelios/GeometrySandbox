module GeometrySandbox.Views.Viewport

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types

open GeometrySandbox

let view : IView =
    StackPanel.create [
        StackPanel.background Theme.palette.canvasBackground
    ]
    :> IView
