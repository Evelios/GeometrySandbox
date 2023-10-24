module PenPlotter.Tests.CoordinateConversion

open Math.Units
open Math.Geometry
open NUnit.Framework
open SharpVG

open PenPlotter

[<SetUp>]
let Setup () = ()

type TestCoordinates = TestCoordinates


[<Test>]
let ``Geometry Translation`` () =
    let expected: IGeometry<TestCoordinates> =
        LineSegment2D.from (Point2D.meters 6. 8.) (Point2D.meters 8. 10.)

    let given: IGeometry<TestCoordinates> =
        LineSegment2D.from (Point2D.meters 1. 2.) (Point2D.meters 3. 4.)

    let actual: IGeometry<TestCoordinates> =
        Geometry.translate (Vector2D.meters 5. 6) given

    Assert.AreEqual(expected, actual)
