[<RequireQualifiedAccess>]
module GeometrySandbox.Extensions.View

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.LogicalTree
open System


/// Add an attribute to an existing view
let withAttr (attr: IAttr<'view>) (view: IView<'view>) =
    { ViewType = view.ViewType
      Attrs = attr :: view.Attrs
      ViewKey = view.ViewKey
      ConstructorArgs = view.ConstructorArgs
      Outlet = view.Outlet }
    :> IView<'view>

/// Add several attributes to an existing view
let withAttrs (attrs: IAttr<'view> list) (view: IView<'view>) =
    { ViewType = view.ViewType
      Attrs = view.Attrs |> List.append attrs
      ViewKey = view.ViewKey
      ConstructorArgs = view.ConstructorArgs
      Outlet = view.Outlet }
    :> IView<'view>

/// Try to find a child control of a given name using breadth first search
let findChildControl (name: string) (source: Control) : Control option =
    let rec findChildControlHelper (children: ILogical list) =
        match children with
        | first :: remaining ->
            if (first :?> Control).Name = name then
                Some(first :?> Control)
            else
                findChildControlHelper (remaining @ (List.ofSeq first.LogicalChildren))

        | [] -> None

    findChildControlHelper (List.ofSeq (source.GetLogicalChildren()))

/// Traverse to the root of the tree and do a breadth first search for the element
let findControl (name: String) (source: Control) : Control option =
    // if source.Name = name then Some source
    // else if source.VisualRoot = null then None
    // else findChildControl name (source.Visual :?> Control)
    // TODO: Avalonia update needs new solution to getting parent control
    None
