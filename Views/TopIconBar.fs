module GeometrySandbox.Views.TopIconBar

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg =
    | Save
    | ToggleRuler

let view (dispatch: Msg -> unit) : IView =
    let iconButtons : IView list =
        [ "Save", Icon.save, Save
          "Toggle Ruler", Icon.rulerSquare, ToggleRuler ]
        |> List.map
            (fun (name, icon, msg) ->
                Button.create [
                    Button.padding Theme.spacing.small
                    Button.margin Theme.spacing.small
                    Button.onClick (Event.handleEvent msg >> dispatch)
                    Button.content (icon Icon.large Theme.palette.primaryLightest)
                    Button.tip name
                ]
                :> IView)

    StackPanel.create [
        StackPanel.orientation Orientation.Horizontal
        StackPanel.children iconButtons
    ]
    :> IView
