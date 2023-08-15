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
let ``Line2D to SVG`` () =
    let expected: string =
        """<line stroke="black" stroke-width="2" opacity="1" x1="0" y1="100" x2="100" y2="100"/>"""

    let geom: Line2D<Meters, Cartesian> =
        Line2D.through (Point2D.pixels 0. 100.) (Point2D.pixels 100. 100.)

    testGeometry geom expected


[<Test>]
let ``Circle2D to SVG`` () =
    let expected: string =
        """<circle stroke="black" stroke-width="2" opacity="1" r="25" cx="50" cy="100"/>"""

    let geom: Circle2D<Meters, Cartesian> =
        Circle2D.atPoint (Point2D.pixels 50. 100.) (Length.cssPixels 25.)

    testGeometry geom expected

[<Test>]
let ``BoundingBox2D to SVG`` () =
    let expected: string =
        """<rect stroke="black" stroke-width="2" opacity="1" x="0" y="50" width="150" height="50"/>"""

    let geom: BoundingBox2D<Meters, Cartesian> =
        BoundingBox2D.from (Point2D.pixels 0. 50.) (Point2D.pixels 150. 100.)

    testGeometry geom expected

[<Test>]
let ``Rectangle2D to SVG`` () =
    let expected: string =
        """<rect stroke="black" stroke-width="2" opacity="1" x="25" y="50" width="50" height="100"/>"""

    let geom: Rectangle2D<Meters, Cartesian> =
            Rectangle2D.withDimensions
                 (Size2D.create (Length.cssPixels 50) (Length.cssPixels 100))
                 (Angle.degrees 0)
                 (Point2D.pixels 50 100)

    testGeometry geom expected
    
[<Test>]
let ``Polygon2D outer circle to SVG`` () =
    let expected: string =
        """<polygon stroke="black" stroke-width="2" opacity="1" points="10,20 30,40 50,60 70,80"/>"""

    let geom: Polygon2D<Meters, Cartesian> =
        Polygon2D.singleLoop [
            Point2D.pixels 10. 20.
            Point2D.pixels 30. 40.
            Point2D.pixels 50. 60.
            Point2D.pixels 70. 80.
        ]

    testGeometry geom expected
    
[<Test>]
let ``Polyline2D to SVG`` () =
    let expected: string =
        """<polyline stroke="black" stroke-width="2" opacity="1" points="10,20 30,40 50,60 70,80"/>"""

    let geom: Polyline2D<Meters, Cartesian> =
        Polyline2D.fromVertices [
            Point2D.pixels 10. 20.
            Point2D.pixels 30. 40.
            Point2D.pixels 50. 60.
            Point2D.pixels 70. 80.
        ]

    testGeometry geom expected
