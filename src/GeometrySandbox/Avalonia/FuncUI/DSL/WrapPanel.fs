namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module WrapPanel =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<WrapPanel> list) : IView<WrapPanel> = ViewBuilder.Create<WrapPanel>(attrs)

    type WrapPanel with

// static member selectedItems<'t when 't :> WrapPanel>(items: IList) : IAttr<'t> =
//     AttrBuilder<'t>
//         .CreateProperty<IList>(WrapPanel.SelectedItemsProperty, items, ValueNone)

// static member onSelectedItemsChanged<'t when 't :> WrapPanel>(func: IList -> unit, ?subPatchOptions) =
//     AttrBuilder<'t>
//         .CreateSubscription<IList>(WrapPanel.SelectedItemsProperty, func, ?subPatchOptions = subPatchOptions)

// static member selectionMode<'t when 't :> WrapPanel>(mode: SelectionMode) : IAttr<'t> =
//     AttrBuilder<'t>
//         .CreateProperty<SelectionMode>(WrapPanel.SelectionModeProperty, mode, ValueNone)
