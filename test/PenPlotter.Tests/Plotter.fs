module PenPlotter.Tests.Plotter

open Math.Units
open Math.Geometry
open NUnit.Framework
open SharpVG

open PenPlotter

[<SetUp>]
let Setup () = ()

type StartCoord = StartCoord
type EndCoord = EndCoord


[<Test>]
let ``Change coordinate systems`` () =
    let black = Pen.withColor "#000000" (Length.millimeters 0.3)

    let expected: Plotter<EndCoord> =
        let margin = Length.inches 1.
        let canvas = Canvas.a4 Orientation.Landscape margin
        Plotter.create canvas [ Layer.withPen black [ Circle2D.atPoint (Point2D.meters 1. 2.) (Length.meters 3.) ] ]

    let given: Plotter<StartCoord> =
        let margin = Length.inches 1.
        let canvas = Canvas.a4 Orientation.Landscape margin
        Plotter.create canvas [ Layer.withPen black [ Circle2D.atPoint (Point2D.meters 1. 2.) (Length.meters 3.) ] ]

    let referenceFrame = Frame2D.atPoint (Point2D.inches 2. 3.)

    let actual: Plotter<EndCoord> = Plotter.relativeTo referenceFrame given

    Assert.AreEqual(expected, actual)
