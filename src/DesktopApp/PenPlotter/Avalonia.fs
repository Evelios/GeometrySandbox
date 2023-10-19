module PenPlotter.Avalonia


open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.Types
open Math.Geometry
open Math.Units

open PenPlotter

/// Convert data types from the Math.Geometry library to the Avalonia format for
/// rendering to the screen.
module GeometryToAvalonia =
    /// Convert a Point2D object into an Avalonia.Point
    let toPoint (point: Point2D<Meters, SvgCoordinates>) : Avalonia.Point =
        Avalonia.Point(Length.inCssPixels point.X, Length.inCssPixels point.Y)

    let toLine (pen: PenPlotter.Pen) (line: LineSegment2D<Meters, SvgCoordinates>) : IView<Line> =
        Line.create
            [ Line.startPoint (Length.inCssPixels line.Start.X, Length.inCssPixels line.Start.Y)
              Line.endPoint (Length.inCssPixels line.Finish.X, Length.inCssPixels line.Finish.Y)
              Line.strokeThickness (Length.inCssPixels pen.Thickness)
              Line.stroke pen.Color ]

    let toCircle (pen: PenPlotter.Pen) (circle: Circle2D<Meters, SvgCoordinates>) : IView<Ellipse> =
        Ellipse.create
            [ Ellipse.height (Length.inCssPixels circle.Radius)
              Ellipse.width (Length.inCssPixels circle.Radius)
              Ellipse.top (circle.Center.Y - circle.Radius / 2. |> Length.inCssPixels)
              Ellipse.left (circle.Center.X - circle.Radius / 2. |> Length.inCssPixels)
              Rectangle.strokeThickness (Length.inCssPixels pen.Thickness)
              Rectangle.stroke pen.Color ]

    let toBoundingBoxRectangle (pen: PenPlotter.Pen) (bbox: BoundingBox2D<Meters, SvgCoordinates>) : IView<Rectangle> =
        Rectangle.create
            [ Rectangle.width (BoundingBox2D.width bbox |> Length.inCssPixels)
              Rectangle.height (BoundingBox2D.height bbox |> Length.inCssPixels)
              Rectangle.top (Length.inCssPixels bbox.TopLeft.Y)
              Rectangle.left (Length.inCssPixels bbox.TopLeft.X)
              Rectangle.strokeThickness (Length.inCssPixels pen.Thickness)
              Rectangle.stroke pen.Color ]

    // TODO: Allow for rotated rectangles
    let toRectangle (pen: PenPlotter.Pen) (rect: Rectangle2D<Meters, SvgCoordinates>) : IView<Rectangle> =
        let boundingBox = Rectangle2D.boundingBox rect
        toBoundingBoxRectangle pen boundingBox

    // TODO: Allow for internal polygons
    let toPolygon (pen: PenPlotter.Pen) (polygon: Polygon2D<Meters, SvgCoordinates>) : IView<Polygon> =
        let outerPoints = List.map toPoint polygon.OuterLoop

        Polygon.create
            [ Polygon.points outerPoints
              Polygon.strokeThickness (Length.inCssPixels pen.Thickness)
              Polygon.stroke pen.Color ]

    let toPolyline (pen: PenPlotter.Pen) (polyline: Polyline2D<Meters, SvgCoordinates>) : IView<Polyline> =
        let points = List.map toPoint polyline.Vertices

        Polyline.create
            [ Polyline.points points
              Polyline.strokeThickness (Length.inCssPixels pen.Thickness)
              Polyline.stroke pen.Color ]

/// The Cartesian to SvgCoordinates reference frame.
/// The top left point in Svg coordinates is the origin point for this system.
/// This reference frame excludes the margins of the canvas and only
/// includes the working area of the canvas (everything except the margins)
let svgFrame (canvas: Canvas) : Frame2D<Meters, Cartesian, SvgCoordinates> =
    let width = -canvas.Margin
    let height = (Canvas.height canvas) - canvas.Margin
    let topLeft = Point2D.xy width height

    Frame2D.atPoint topLeft |> Frame2D.reverseY


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

    let geometryViews = List.map (fromGeometry pen) geometries

    Avalonia.FuncUI.DSL.Canvas.create [ Canvas.children geometryViews ]

let convertGeometry
    (conversionFrame: Frame2D<Meters, Cartesian, SvgCoordinates>)
    (geometry: IGeometry<Cartesian>)
    : IGeometry<SvgCoordinates> =
    match geometry with
    | :? LineSegment2D<Meters, Cartesian> as line ->
        LineSegment2D.relativeTo conversionFrame line :> IGeometry<SvgCoordinates>
    | :? Circle2D<Meters, Cartesian> as circle ->
        Circle2D.relativeTo conversionFrame circle :> IGeometry<SvgCoordinates>
    | :? Rectangle2D<Meters, Cartesian> as rect ->
        Rectangle2D.relativeTo conversionFrame rect :> IGeometry<SvgCoordinates>
    | :? Polygon2D<Meters, Cartesian> as polygon ->
        Polygon2D.relativeTo conversionFrame polygon :> IGeometry<SvgCoordinates>
    | :? Polyline2D<Meters, Cartesian> as polyline ->
        Polyline2D.relativeTo conversionFrame polyline :> IGeometry<SvgCoordinates>

    | _ ->
        failwith (
            "Unable to create SVG from geometry.\n"
            + " This can be caused by not using the SvgCoordinates coordinate system:\n"
            + $"{geometry.GetType()}: {geometry}"
        )

/// The coordinate mapping from geometry objects existing in the Cartesian space
/// and converts them to be in the SVG y-down coordinate system.
let private coordinateConversion
    (conversionFrame: Frame2D<Meters, Cartesian, SvgCoordinates>)
    (geometries: IGeometry<Cartesian> seq)
    : IGeometry<SvgCoordinates> seq =
    Seq.map (convertGeometry conversionFrame) geometries


// ---- Svg Generation -----------------------------------------------------

/// Create an Svg group from a single pen plotter layer
let fromLayer (conversionFrame: Frame2D<Meters, Cartesian, SvgCoordinates>) (layer: Layer) : IView =
    let convertedLayerGeometry = coordinateConversion conversionFrame layer.Geometry
    fromGeometries layer.Pen (List.ofSeq convertedLayerGeometry)

/// A plotter needs to convert all the geometries within it from the Cartesian
/// coordinate system to the SvgCoordinates system. This translation also adds
/// the margin to the page.
let fromPlotter (plotter: Plotter) : IView =
    let svgReferenceFrame = svgFrame plotter.Canvas

    let layers = List.map (fromLayer svgReferenceFrame) (List.ofSeq plotter.Layers)

    Avalonia.FuncUI.DSL.Canvas.create
        [ Avalonia.Controls.Canvas.width (Length.inCssPixels plotter.Canvas.Size.Width)
          Avalonia.Controls.Canvas.height (Length.inCssPixels plotter.Canvas.Size.Width)
          Avalonia.Controls.Canvas.children layers ]
