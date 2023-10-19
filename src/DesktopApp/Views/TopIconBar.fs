module GeometrySandbox.Views.TopIconBar

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout

open GeometrySandbox
open GeometrySandbox.Extensions

type Msg =
    | Action of Action
    | Save
    | ToggleRuler

let view (dispatch: Msg -> unit) : IView =
    let iconButtons: (string * (Icon.Size -> string -> IView<Viewbox>) * Msg) list =
        [ "Save", Icon.save, Save
          "Toggle Ruler", Icon.rulerSquare, ToggleRuler
          "Zoom In", Icon.zoomIn, Action Action.ZoomIn
          "Zoom Out", Icon.zoomOut, Action Action.ZoomOut
          "Zoom Full Size", Icon.zoomReturn, Action Action.ZoomToFullSize
          "Single Page View", Icon.singlePage, Action(Action.ChangePageViewMode PageViewMode.SinglePage)
          "Frame View", Icon.framedPage, Action(Action.ChangePageViewMode PageViewMode.FramedPage)
          "Multiple Page View", Icon.multiplePages, Action(Action.ChangePageViewMode PageViewMode.MultiplePages)
          "Fullscreen View", Icon.fullscreen, Action(Action.ChangePageViewMode PageViewMode.FullScreen) ]

    let iconButtonView (name: string, icon: Icon.Size -> string -> IView<Viewbox>, msg) =
        Button.create
            [ Button.padding Theme.spacing.small
              Button.margin Theme.spacing.small
              Button.onClick (Event.handleEvent msg >> dispatch)
              Button.content (icon Icon.large Theme.palette.primaryLightest)
              Button.tip name ]
        :> IView

    let iconButtonViews = List.map iconButtonView iconButtons

    StackPanel.create
        [ StackPanel.orientation Orientation.Horizontal
          StackPanel.children iconButtonViews ]
    :> IView
