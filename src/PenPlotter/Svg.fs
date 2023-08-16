/// Convert Math.Geometry objects into Svg files.
module PenPlotter.Svg

open SharpVG
open Math.Geometry
open Math.Units

open PenPlotter


module GeometryToSharpVG =
    /// The unit conversion from the typed Length to untyped float used in the Svg coordinate system
    let unwrapLength (len: Math.Units.Length) : float = Length.inCssPixels len

    let toLength (len: Math.Units.Length) : SharpVG.Length =
        unwrapLength len |> SharpVG.Length.ofFloat

    let toPoint (point: Point2D<Meters, 'Coordinates>) : SharpVG.Point =
        Point.ofFloats (unwrapLength point.X, unwrapLength point.Y)

    let toLine (line: Line2D<Meters, 'Coordinates>) : SharpVG.Line =
        let start = toPoint line.Start
        let finish = toPoint line.Finish
        Line.create start finish

    let toCircle (circle: Circle2D<Meters, 'Coordinates>) : SharpVG.Circle =
        let center = toPoint circle.Center
        let radius = toLength circle.Radius
        Circle.create center radius

    let toArea (size: Size2D<Meters, 'Coordinates>) : SharpVG.Area =
        Area.ofFloats (unwrapLength size.Width, unwrapLength size.Height)

    let toRectangle (rectangle: Rectangle2D<Meters, 'Coordinates>) : SharpVG.Rect =
        let anchorPoint = List.head (Rectangle2D.vertices rectangle) |> toPoint
        let dimensions = Rectangle2D.dimensions rectangle |> toArea
        Rect.create anchorPoint dimensions

    /// TODO: take care of inner loop geometry
    let toPolygon (polygon: Polygon2D<Meters, 'Coordinates>) : SharpVG.Polygon =
        if not <| polygon.InnerLoops.IsEmpty then
            failwith "Inner loops within Polygon2D are not generated into Svg currently."

        List.map toPoint (Polygon2D.outerLoop polygon) |> Polygon.ofSeq

    let bboxToRectangle (bbox: BoundingBox2D<Meters, 'Coordinates>) : SharpVG.Rect =
        Rectangle2D.fromBoundingBox bbox |> toRectangle

    let toPolyline (polyline: Polyline2D<Meters, 'Coordinates>) : SharpVG.Polyline =
        List.map toPoint (Polyline2D.vertices polyline) |> Polyline.ofSeq


/// Create an Svg style based on the pen that is being used
let styleFromPen (pen: PenPlotter.Pen) : SharpVG.Style =
    let strokeWidth = GeometryToSharpVG.toLength pen.Thickness
    let svgPen = SharpVG.Pen.createWithWidth (Color.ofName Colors.Black) strokeWidth
    Style.empty |> Style.withStrokePen svgPen

/// Create an SVG based on a pen and the input geometry.
/// This needs to be used with the SvgCoordinates coordinate system.
let fromGeometry (pen: PenPlotter.Pen) (geometry: IGeometry) : Element =
    let style = styleFromPen pen

    match geometry with
    | :? Line2D<Meters, SvgCoordinates> as line -> GeometryToSharpVG.toLine line |> Element.createWithStyle style

    | :? Line2D<Meters, obj> as line -> GeometryToSharpVG.toLine line |> Element.createWithStyle style

    | :? Circle2D<Meters, SvgCoordinates> as circle ->
        GeometryToSharpVG.toCircle circle |> Element.createWithStyle style

    | :? Circle2D<Meters, obj> as circle -> GeometryToSharpVG.toCircle circle |> Element.createWithStyle style

    | :? Rectangle2D<Meters, SvgCoordinates> as rect ->
        GeometryToSharpVG.toRectangle rect |> Element.createWithStyle style

    | :? Rectangle2D<Meters, obj> as rect -> GeometryToSharpVG.toRectangle rect |> Element.createWithStyle style

    | :? BoundingBox2D<Meters, SvgCoordinates> as bbox ->
        GeometryToSharpVG.bboxToRectangle bbox |> Element.createWithStyle style

    | :? BoundingBox2D<Meters, obj> as bbox -> GeometryToSharpVG.bboxToRectangle bbox |> Element.createWithStyle style

    | :? Polygon2D<Meters, SvgCoordinates> as polygon ->
        GeometryToSharpVG.toPolygon polygon |> Element.createWithStyle style

    | :? Polygon2D<Meters, obj> as polygon -> GeometryToSharpVG.toPolygon polygon |> Element.createWithStyle style

    | :? Polyline2D<Meters, SvgCoordinates> as polyline ->
        GeometryToSharpVG.toPolyline polyline |> Element.createWithStyle style

    | :? Polyline2D<Meters, obj> as polyline -> GeometryToSharpVG.toPolyline polyline |> Element.createWithStyle style

    | _ ->
        failwith (
            "Unable to create SVG from geometry.\n"
            + " This can be caused by not using the SvgCoordinates coordinate system:\n"
            + $"{geometry.GetType()}: {geometry}"
        )

/// Create an Svg group of geometries all bundled under the same tag
let fromGeometries (pen: PenPlotter.Pen) (geometries: IGeometry list) : Group =
    List.map (fromGeometry pen) geometries |> Group.ofList

/// Create an Svg group from a single pen plotter layer
let fromLayer (layer: Layer) : SharpVG.Group =
    fromGeometries layer.Pen (List.ofSeq layer.Geometry)

let fromPlotter (plotter: Plotter) : SharpVG.Svg =
    let canvas = plotter.Canvas
    let layers = plotter.Layers

    let layerGroup: SharpVG.Group =
        let layerGroups =
            List.map fromLayer (List.ofSeq layers) |> List.map GroupElement.Group

        Group.ofList [] |> Group.withBody layerGroups

    let viewBox: SharpVG.ViewBox =
        ViewBox.create (GeometryToSharpVG.toPoint Point2D.origin) (GeometryToSharpVG.toArea canvas.Size)

    Svg.ofGroup layerGroup |> Svg.withViewBox viewBox
