module PenPlotter.Tests.Plotter

open NUnit.Framework

open Math.Units
open Math.Geometry
open SharpVG

open PenPlotter

[<SetUp>]
let Setup () = ()


let testGeometry (geom: IGeometry) (expected: string) : unit =
    let pen = Pen.create [ Pen.thickness (Length.cssPixels 2); Pen.color "#000000" ]
    let elementToString (ele: Element) : string = $"{ele}"
    let actual = Svg.fromGeometry pen geom |> elementToString
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Circle2D to SVG`` () =
    let expected: string =
        """<circle stroke="black" stroke-width="2" opacity="1" r="25" cx="50" cy="100"/>"""

    let geom: Circle2D<Meters, Cartesian> =
        Circle2D.atPoint (Point2D.pixels 50. 100.) (Length.cssPixels 25.)

    testGeometry geom expected

[<Test>]
let ``Line2D to SVG`` () =
    let expected: string =
        """<line stroke="black" stroke-width="2" opacity="1" x1="0" y1="100" x2="100" y2="100"/>"""

    let geom: Line2D<Meters, Cartesian> =
        Line2D.through (Point2D.pixels 0. 100.) (Point2D.pixels 100. 100.)

    testGeometry geom expected
