/// Convert Math.Geometry objects into Svg files.
module PenPlotter.Svg

open Math.Geometry
open Math.Units
open SharpVG
open System

open PenPlotter


/// Convert a Math.Geometry library to a SharpVG object for SVG creation.
module GeometryToSharpVG =
    /// The unit conversion from the typed Length to untyped float used in the Svg coordinate system
    let unwrapLength (len: Math.Units.Length) : float = Length.inCssPixels len

    let toLength (len: Math.Units.Length) : SharpVG.Length =
        unwrapLength len |> SharpVG.Length.ofFloat

    let toPoint (point: Point2D<Meters, 'Coordinates>) : SharpVG.Point =
        Point.ofFloats (unwrapLength point.X, unwrapLength point.Y)

    let toLine (line: LineSegment2D<Meters, 'Coordinates>) : SharpVG.Line =
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
    let hexColor = Convert.ToInt32(pen.Color, 16)
    let svgPen = SharpVG.Pen.createWithWidth (Color.ofHex hexColor) strokeWidth
    Style.empty |> Style.withStrokePen svgPen

/// Create an SVG based on a pen and the input geometry.
/// This needs to be used with the SvgCoordinates coordinate system.
let fromGeometry (pen: PenPlotter.Pen) (geometry: IGeometry<SvgCoordinates>) : Element =
    let style = styleFromPen pen

    match geometry with
    | :? LineSegment2D<Meters, SvgCoordinates> as line -> GeometryToSharpVG.toLine line |> Element.createWithStyle style

    | :? Circle2D<Meters, SvgCoordinates> as circle ->
        GeometryToSharpVG.toCircle circle |> Element.createWithStyle style

    | :? Rectangle2D<Meters, SvgCoordinates> as rect ->
        GeometryToSharpVG.toRectangle rect |> Element.createWithStyle style

    | :? BoundingBox2D<Meters, SvgCoordinates> as bbox ->
        GeometryToSharpVG.bboxToRectangle bbox |> Element.createWithStyle style

    | :? Polygon2D<Meters, SvgCoordinates> as polygon ->
        GeometryToSharpVG.toPolygon polygon |> Element.createWithStyle style

    | :? Polyline2D<Meters, SvgCoordinates> as polyline ->
        GeometryToSharpVG.toPolyline polyline |> Element.createWithStyle style

    | _ ->
        failwith (
            "Unable to create SVG from geometry.\n"
            + " This can be caused by not using the SvgCoordinates coordinate system:\n"
            + $"{geometry.GetType()}: {geometry}"
        )

/// Create an Svg group of geometries all bundled under the same tag
let fromGeometries (pen: PenPlotter.Pen) (geometries: IGeometry<SvgCoordinates> list) : Group =
    List.map (fromGeometry pen) geometries |> Group.ofList

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
let fromLayer
    (conversionFrame: Frame2D<Meters, Cartesian, SvgCoordinates>)
    (layer: Layer<'Coordinates>)
    : SharpVG.Group =
    let convertedLayerGeometry = coordinateConversion conversionFrame layer.Geometry

    fromGeometries layer.Pen (List.ofSeq convertedLayerGeometry)

/// The Cartesian to SvgCoordinates reference frame.
/// The top left point in Svg coordinates is the origin point for this system.
/// This reference frame excludes the margins of the canvas and only
/// includes the working area of the canvas (everything except the margins)
let svgFrame (canvas: Canvas<'Coordinates>) : Frame2D<Meters, Cartesian, SvgCoordinates> =
    let width = -canvas.Margin
    let height = (Canvas.height canvas) - canvas.Margin
    let topLeft = Point2D.xy width height

    Frame2D.atPoint topLeft |> Frame2D.reverseY

/// A plotter needs to convert all the geometries within it from the Cartesian
/// coordinate system to the SvgCoordinates system. This translation also adds
/// the margin to the page.
let fromPlotter (plotter: Plotter<'Coordiantes>) : SharpVG.Svg =
    let svgReferenceFrame = svgFrame plotter.Canvas

    let layerGroup: SharpVG.Group =
        let layerGroups =
            List.map (fromLayer svgReferenceFrame) (List.ofSeq plotter.Layers)
            |> List.map GroupElement.Group

        Group.ofList [] |> Group.withBody layerGroups

    let viewBox: SharpVG.ViewBox =
        ViewBox.create (GeometryToSharpVG.toPoint Point2D.origin) (GeometryToSharpVG.toArea plotter.Canvas.Size)

    Svg.ofGroup layerGroup |> Svg.withViewBox viewBox


let toSvg (plotter: Plotter<'Coordinates>) : string = Svg.toString (fromPlotter plotter)
