module PenPlotter.Tests.Plotter

open Math.Units
open Math.Geometry
open NUnit.Framework
open SharpVG

open PenPlotter

[<SetUp>]
let Setup () = ()


let testGeometry (geom: IGeometry<SvgCoordinates>) (expected: string) : unit =
    let pen = Pen.create [ Pen.thickness (Length.cssPixels 2); Pen.color 0x000000 ]
    let elementToString (ele: Element) : string = $"{ele}"
    let actual = Svg.fromGeometry pen geom |> elementToString
    Assert.AreEqual(expected, actual)

[<Test>]
let ``LineSegment2D to SVG`` () =
    let expected: string =
        """<line stroke="0x0" stroke-width="2" opacity="1" x1="0" y1="100" x2="100" y2="100"/>"""

    let geom: LineSegment2D<Meters, SvgCoordinates> =
        LineSegment2D.from (Point2D.pixels 0. 100.) (Point2D.pixels 100. 100.)

    testGeometry geom expected


[<Test>]
let ``Circle2D to SVG`` () =
    let expected: string =
        """<circle stroke="0x0" stroke-width="2" opacity="1" r="25" cx="50" cy="100"/>"""

    let geom: Circle2D<Meters, SvgCoordinates> =
        Circle2D.atPoint (Point2D.pixels 50. 100.) (Length.cssPixels 25.)

    testGeometry geom expected

[<Test>]
let ``BoundingBox2D to SVG`` () =
    let expected: string =
        """<rect stroke="0x0" stroke-width="2" opacity="1" x="0" y="50" width="150" height="50"/>"""

    let geom: BoundingBox2D<Meters, SvgCoordinates> =
        BoundingBox2D.from (Point2D.pixels 0. 50.) (Point2D.pixels 150. 100.)

    testGeometry geom expected

[<Test>]
let ``Rectangle2D to SVG`` () =
    let expected: string =
        """<rect stroke="0x0" stroke-width="2" opacity="1" x="25" y="50" width="50" height="100"/>"""

    let geom: Rectangle2D<Meters, SvgCoordinates> =
        Rectangle2D.withDimensions
            (Size2D.create (Length.cssPixels 50) (Length.cssPixels 100))
            (Angle.degrees 0)
            (Point2D.pixels 50 100)

    testGeometry geom expected

[<Test>]
let ``Polygon2D outer circle to SVG`` () =
    let expected: string =
        """<polygon stroke="0x0" stroke-width="2" opacity="1" points="10,20 30,40 50,60 70,80"/>"""

    let geom: Polygon2D<Meters, SvgCoordinates> =
        Polygon2D.singleLoop
            [ Point2D.pixels 10. 20.
              Point2D.pixels 30. 40.
              Point2D.pixels 50. 60.
              Point2D.pixels 70. 80. ]

    testGeometry geom expected

[<Test>]
let ``Polyline2D to SVG`` () =
    let expected: string =
        """<polyline stroke="0x0" stroke-width="2" opacity="1" points="10,20 30,40 50,60 70,80"/>"""

    let geom: Polyline2D<Meters, SvgCoordinates> =
        Polyline2D.fromVertices
            [ Point2D.pixels 10. 20.
              Point2D.pixels 30. 40.
              Point2D.pixels 50. 60.
              Point2D.pixels 70. 80. ]

    testGeometry geom expected


[<Test>]
let ``Group of LineSegment2D to SVG`` () =
    let expected: string =
        "<g>"
        + """<line stroke="0x0" stroke-width="2" opacity="1" x1="10" y1="20" x2="30" y2="40"/>"""
        + """<line stroke="0x0" stroke-width="2" opacity="1" x1="50" y1="60" x2="70" y2="80"/>"""
        + "</g>"

    let geoms: IGeometry<SvgCoordinates> list =
        [ LineSegment2D.from (Point2D.pixels 10. 20.) (Point2D.pixels 30. 40.)
          LineSegment2D.from (Point2D.pixels 50. 60.) (Point2D.pixels 70. 80.) ]

    let pen = Pen.create [ Pen.thickness (Length.cssPixels 2); Pen.color 0x000000 ]
    let elementToString (ele: Group) : string = $"{ele}"
    let actual = Svg.fromGeometries pen geoms |> elementToString

    Assert.AreEqual(expected, actual)

[<Test>]
let ``Plotter with ViewBox`` () =
    let expected: string =
        """<svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0,0 200,400">"""
        + "<g>"
        + "</g>"
        + "</svg>"

    let canvas =
        Canvas.create (Size2D.create (Length.cssPixels 200.) (Length.cssPixels 400)) (Length.cssPixels 20.)

    let plotter = Plotter.create canvas []
    let actual = Plotter.toSvg plotter

    Assert.AreEqual(expected, actual)

/// The plotter converts from the cartesian space to the Svg space. This
/// includes flipping the Y-axis and adding the padding on the margin
/// for the art piece.
[<Test>]
let ``Plotter from Cartesian to full Svg file`` () =
    let expected: string =
        """<svg xmlns="http://www.w3.org/2000/svg" width="100%" height="100%" viewBox="0,0 200,400">"""
        + "<g>"
        + "<g>"
        + """<line stroke="0x0" stroke-width="2" opacity="1" x1="30" y1="360" x2="50" y2="340"/>"""
        + """<line stroke="0x0" stroke-width="2" opacity="1" x1="70" y1="320" x2="90" y2="300"/>"""
        + "</g>"
        + "<g>"
        + """<line stroke="0xffffff" stroke-width="3" opacity="1" x1="130" y1="260" x2="150" y2="240"/>"""
        + """<line stroke="0xffffff" stroke-width="3" opacity="1" x1="170" y1="220" x2="190" y2="200"/>"""
        + "</g>"
        + "</g>"
        + "</svg>"

    let canvas =
        Canvas.create (Size2D.create (Length.cssPixels 200.) (Length.cssPixels 400)) (Length.cssPixels 20.)


    let black = Pen.create [ Pen.thickness (Length.cssPixels 2); Pen.color 0x000000 ]

    let layer1: Layer =
        Layer.withPen
            black
            [ LineSegment2D.from (Point2D.pixels 10. 20.) (Point2D.pixels 30. 40.)
              LineSegment2D.from (Point2D.pixels 50. 60.) (Point2D.pixels 70. 80.) ]

    let white = Pen.create [ Pen.thickness (Length.cssPixels 3); Pen.color 0XFFFFFF ]

    let layer2: Layer =
        Layer.withPen
            white
            [ LineSegment2D.from (Point2D.pixels 110. 120.) (Point2D.pixels 130. 140.)
              LineSegment2D.from (Point2D.pixels 150. 160.) (Point2D.pixels 170. 180.) ]

    let plotter = Plotter.create canvas [ layer1; layer2 ]
    let actual = Plotter.toSvg plotter

    Assert.AreEqual(expected, actual)
