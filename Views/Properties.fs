module GeometrySandbox.Views.Properties

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Geometry

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg =
    | ChangeOrientation of Orientation
    | ChangeHeight of string
    | ChangeWidth of string
    | ChangeSeed of int

type Model = { Size: Size2D<Meters>; Seed: int }

let orientationBlock size dispatch =
    StackPanel.create [
        StackPanel.children [

            Text.iconTitle (Icon.orientation Icon.large Theme.palette.primary) "Orientation" Theme.palette.foreground

            ListBox.create [
                ListBox.dataItems [ Portrait; Landscape ]
                ListBox.selectedItem  (Size2D.orientation size)
                ListBox.onSelectedItemChanged
                    (fun item ->
                        if not (isNull item) then
                            ChangeOrientation(item :?> Orientation)
                            |> dispatch)
            ]
        ]
    ]

let sizeBlock model dispatch =
    StackPanel.create [
        StackPanel.children [

            Text.iconTitle (Icon.resize Icon.large Theme.palette.primary) "Size" Theme.palette.foreground

            Form.formElement
                {| Name = "Height"
                   Orientation = Orientation.Vertical
                   Element =
                       TextBox.create [
                           TextBox.text $"{Length.inCssPixels model.Size.Height}"
                           TextBox.onTextChanged (fun text -> ChangeHeight text |> dispatch)
                       ] |}

            Form.formElement
                {| Name = "Width"
                   Orientation = Orientation.Vertical
                   Element =
                       TextBox.create [
                           TextBox.text $"{Length.inCssPixels model.Size.Width}"
                           TextBox.onTextChanged (fun text -> ChangeWidth text |> dispatch)
                       ] |}

            ]
    ]

let seed model dispatch =
    Form.formElement
        {| Name = "Seed"
           Orientation = Orientation.Vertical
           Element =
               NumericUpDown.create [
                   NumericUpDown.value (float model.Seed)
                   NumericUpDown.onValueChanged (fun newSeed -> ChangeSeed(int newSeed) |> dispatch)
                   NumericUpDown.increment 1.
               ] |}


let view (model: Model) (dispatch: Msg -> unit) : IView =
    StackPanel.create [
        StackPanel.minWidth Theme.size.small
        StackPanel.children [
            orientationBlock model.Size dispatch
            sizeBlock model dispatch
            seed model dispatch
        ]
    ]
    :> IView
