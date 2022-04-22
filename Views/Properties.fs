module GeometrySandbox.Views.Properties

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout

open GeometrySandbox
open GeometrySandbox.Extensions


type Msg = | NoMsg

let view (dispatch: Msg -> unit) : IView =
    let panels =
        [ "Orientation"; "Size"; "Seed" ]
        |> List.map (fun name -> TextBlock.create [ TextBlock.text name ] :> IView)

    StackPanel.create [
        StackPanel.minWidth Theme.size.small
        StackPanel.children panels
    ]
    :> IView
