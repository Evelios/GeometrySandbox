module PenPlotter.Tests.Plotter

open NUnit.Framework

open PenPlotter
open Math.Units
open Math.Geometry

[<SetUp>]
let Setup () = ()

[<Test>]
let ``Line2D to SVG`` () =
    let expected: string =
        """<line stroke="black" stroke-width="2" opacity="1" x1="0" y1="100" x2="100" y2="100"/>"""
    

    let line: Line2D<Meters, Cartesian> =
        Line2D.through
            (Point2D.pixels 0. 100.)
            (Point2D.pixels 100. 100.)

    let black = Pen.create [ Pen.thickness (Length.cssPixels 2); Pen.color "#000000" ]

    let svg: string =
        Plotter.Svg.fromGeometry black line
        |> sprintf "%O"

    Assert.AreEqual(expected,  svg)
