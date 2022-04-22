module GeometrySandbox.Views.Properties

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout

open GeometrySandbox
open GeometrySandbox.Extensions


type Msg = ChangeOrientation of Orientation

type Model = { Orientation: Orientation }

let view (model: Model) (dispatch: Msg -> unit) : IView =

    let orientationBlock =
        StackPanel.create [
            StackPanel.children [

                Text.iconTitle
                    (Icon.orientation Icon.large Theme.palette.primary)
                    "Orientation"
                    Theme.palette.foreground

                ListBox.create [
                    ListBox.dataItems [ Portrait; Landscape ]
                    ListBox.selectedItem model.Orientation
                    ListBox.onSelectedItemChanged
                        (fun item ->
                            if not (isNull item) then
                                ChangeOrientation(item :?> Orientation)
                                |> dispatch)
                ]
            ]
        ]

    StackPanel.create [
        StackPanel.minWidth Theme.size.small
        StackPanel.children [ orientationBlock ]
    ]
    :> IView
