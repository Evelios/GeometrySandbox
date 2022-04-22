namespace GeometrySandbox.Svg

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia
open Avalonia.Controls.Shapes
open Geometry

module Line =
    let createFrom (line: Line2D<Meters, 'Coordinates>) (attrs: IAttr<Line> list) : IView =
        Line.create (
            [ Line.startPoint (Point(Length.inCssPixels line.Start.X, Length.inCssPixels line.Start.Y))
              Line.endPoint (Point(Length.inCssPixels line.Finish.X, Length.inCssPixels line.Finish.Y)) ]
            @ attrs
        )
        :> IView
