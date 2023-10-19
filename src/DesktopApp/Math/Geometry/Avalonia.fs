module Math.Geometry.Avalonia

open Math.Geometry
open Math.Units

open Avalonia.Controls.Shapes
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types

module Line =
    let draw (line: LineSegment2D<Meters, 'Coordinates>) (attrs: IAttr<Line> list) : IView<Line> =
        Line.create (
            [ Line.startPoint (Length.inCssPixels line.Start.X, Length.inCssPixels line.Start.Y)
              Line.endPoint (Length.inCssPixels line.Finish.X, Length.inCssPixels line.Finish.Y) ]
            @ attrs
        )
