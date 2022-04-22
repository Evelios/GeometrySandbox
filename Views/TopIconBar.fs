module GeometrySandbox.Views.TopIconBar

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Elmish

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg = Save

let view (dispatch: Msg -> unit) : IView =
    let iconButtons : IView list =
        [ "Save", Icon.save, Save ]
        |> List.map
            (fun (name, icon, msg) ->
                Button.create [
                    Button.padding Theme.spacing.small
                    Button.margin Theme.spacing.small
                    Button.onClick (Event.handleEvent msg >> dispatch)
                    Button.content (icon Icon.large Theme.palette.primaryLightest)
                    Button.tip [
                        ToolTip.content name
                        ToolTip.placement PlacementMode.Bottom
                    ]
                ]
                :> IView)

    StackPanel.create [
        StackPanel.orientation Orientation.Horizontal
        StackPanel.children iconButtons
    ]
    :> IView
