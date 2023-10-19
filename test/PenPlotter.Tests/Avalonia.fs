[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PenPlotter.Tests.Avalonia

open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.VirtualDom
open Avalonia.FuncUI.Types
open Math.Geometry
open Math.Units
open NUnit.Framework

open PenPlotter
open PenPlotter.Avalonia

let pen = Pen.create [ Pen.color "#123456"; Pen.thickness (Length.cssPixels 2.) ]

[<Test>]
let ``Point2D to Point`` () =
    let expected = Avalonia.Point(20., 20.)

    let actual = GeometryToAvalonia.toPoint (Point2D.pixels 20. 20.)

    Assert.AreEqual(actual, expected)


[<Test>]
[<Ignore("Cannot compare Avalonia View objects")>]
let ``LineSegment2D to Line`` () =
    let expected =
        Line.create
            [ Line.startPoint (10., 20.)
              Line.endPoint (40., 80.)
              Line.strokeThickness 2.
              Line.stroke "#123456" ]

    let segment = LineSegment2D.from (Point2D.pixels 10. 20.) (Point2D.pixels 40. 80.)
    let actual = GeometryToAvalonia.toLine pen segment

    Assert.AreEqual(actual, expected)
