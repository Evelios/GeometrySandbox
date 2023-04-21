module GeometrySandbox.Views.Properties

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Input
open Avalonia.Layout
open Math.Geometry
open Math.Units

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg =
    | Action of Action
    | ChangeHeight of string
    | ChangeWidth of string
    | ChangeSeed of int

let orientationBlock size dispatch =
    StackPanel.create
        [ StackPanel.children
              [

                Text.iconTitle
                    (Icon.orientation Icon.large Theme.palette.primary)
                    "Orientation"
                    Theme.palette.foreground
                ListBox.create
                    [ ListBox.dataItems [ Portrait; Landscape ]
                      ListBox.selectedItem (Size2D.orientation size)
                      ListBox.onSelectedItemChanged (fun item ->
                          if not (isNull item) then
                              Action.ChangeOrientation(item :?> Orientation) |> Action |> dispatch) ] ] ]

let sizeBlock model dispatch =
    let title: IView =
        Text.iconTitle (Icon.resize Icon.large Theme.palette.primary) "Size" Theme.palette.foreground :> IView

    let lengths =
        [ "Height", model.Size.Height, ChangeHeight
          "Width", model.Size.Width, ChangeWidth ]

    let unitDropdown =
        ComboBox.create
            [ ComboBox.dataItems DiscriminatedUnion.allCasesOf<LengthUnit>
              ComboBox.selectedItem model.Unit
              ComboBox.onSelectedItemChanged (fun item -> Action.ChangeUnit(item :?> LengthUnit) |> Action |> dispatch) ]

    let lengthInput (name, amount, msg) : IView =
        let length = Length.inUnit model.Unit amount |> string

        Form.formElement
            {| Name = name
               Orientation = Orientation.Vertical
               Element =
                TextBox.create
                    [ TextBox.text length
                      TextBox.onLostFocus (fun e -> msg (e.Source :?> TextBox).Text |> dispatch)
                      TextBox.onKeyDown (fun e ->
                          if e.Key = Key.Enter then
                              e.Handled <- true
                              msg (e.Source :?> TextBox).Text |> dispatch) ] |}
        :> IView


    let lengthInputs = List.map lengthInput lengths

    let panes = title :: lengthInputs

    StackPanel.create [ StackPanel.orientation Orientation.Vertical; StackPanel.children panes ]

let seed model dispatch =
    Form.formElement
        {| Name = "Seed"
           Orientation = Orientation.Vertical
           Element =
            NumericUpDown.create
                [ NumericUpDown.value model.Seed
                  NumericUpDown.onValueChanged (fun newSeed ->
                      if newSeed.HasValue then
                          ChangeSeed(int newSeed.Value) |> dispatch)
                  NumericUpDown.increment 1 ] |}


let view (model: Model) (dispatch: Msg -> unit) : IView =
    StackPanel.create
        [ StackPanel.minWidth Theme.size.small
          StackPanel.children
              [ orientationBlock model.Size dispatch
                sizeBlock model dispatch
                seed model dispatch ] ]
    :> IView
