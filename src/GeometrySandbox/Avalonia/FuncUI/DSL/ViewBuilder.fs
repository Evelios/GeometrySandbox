module Avalonia.FuncUI.DSL.ViewBuilder

open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.Types


type ViewBuilder with

    static member Create<'view>(attrs: IAttr<'view> list, constructorArgs: obj array) : IView<'view> =
        { View.ViewType = typeof<'view>
          View.ViewKey = ValueNone
          View.Attrs = attrs
          View.ConstructorArgs = constructorArgs
          View.Outlet = ValueNone }
        :> IView<'view>
