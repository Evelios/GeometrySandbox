module GeometrySandbox.Views.Plotter

open Avalonia.FuncUI.DSL
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.Types
open Math.Geometry
open Math.Units

open PenPlotter

module GeometryToAvalonia =
    let toLine (pen: PenPlotter.Pen) (line: LineSegment2D<Meters, SvgCoordinates>) : IView<Line> =
        Line.create
            [ Line.startPoint (Length.inCssPixels line.Start.X, Length.inCssPixels line.Start.Y)
              Line.endPoint (Length.inCssPixels line.Finish.X, Length.inCssPixels line.Finish.Y)
              Line.strokeThickness (Length.inCssPixels pen.Thickness)
              Line.stroke "#00000" ]

    let toCircle (pen: PenPlotter.Pen) (circle: Circle2D<Meters, SvgCoordinates>) : IView<Ellipse> =
        Ellipse.create
            [ Ellipse.height (Length.inCssPixels circle.Radius)
              Ellipse.width (Length.inCssPixels circle.Radius)
              Ellipse.top (Length.inCssPixels (circle.Center.Y - circle.Radius / 2))
              Ellipse.left (Length.inCssPixels (circle.Center.X - circle.Radius / 2)) ]

    let toRectangle (pen: PenPlotter.Pen) (rect: Rectangle2D<Meters, SvgCoordinates>) : IView<Rectangle> =
        Rectangle.create []

    let toBoundingBoxRectangle (pen: PenPlotter.Pen) (bbox: BoundingBox2D<Meters, SvgCoordinates>) : IView<Rectangle> =
        Rectangle.create []

    let toPolygon (pen: PenPlotter.Pen) (polygon: Polygon2D<Meters, SvgCoordinates>) : IView<Polygon> =
        Polygon.create []

    let toPolyline (pen: PenPlotter.Pen) (polygon: Polyline2D<Meters, SvgCoordinates>) : IView<Polyline> =
        Polyline.create []



/// Create an SVG based on a pen and the input geometry.
/// This needs to be used with the SvgCoordinates coordinate system.
let fromGeometry (pen: PenPlotter.Pen) (geometry: IGeometry<SvgCoordinates>) : IView =
    match geometry with
    | :? LineSegment2D<Meters, SvgCoordinates> as line -> GeometryToAvalonia.toLine pen line

    | :? Circle2D<Meters, SvgCoordinates> as circle -> GeometryToAvalonia.toCircle pen circle

    | :? Rectangle2D<Meters, SvgCoordinates> as rect -> GeometryToAvalonia.toRectangle pen rect

    | :? BoundingBox2D<Meters, SvgCoordinates> as bbox -> GeometryToAvalonia.toBoundingBoxRectangle pen bbox

    | :? Polygon2D<Meters, SvgCoordinates> as polygon -> GeometryToAvalonia.toPolygon pen polygon

    | :? Polyline2D<Meters, SvgCoordinates> as polyline -> GeometryToAvalonia.toPolyline pen polyline

    | _ ->
        failwith (
            "Unable to create SVG from geometry.\n"
            + " This can be caused by not using the SvgCoordinates coordinate system:\n"
            + $"{geometry.GetType()}: {geometry}"
        )

/// Create an Svg group of geometries all bundled under the same tag
let fromGeometries
    (pen: PenPlotter.Pen)
    (geometries: IGeometry<SvgCoordinates> list)
    : IView<Avalonia.Controls.Canvas> =
    Avalonia.FuncUI.DSL.Canvas.create []
