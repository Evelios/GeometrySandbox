namespace PenPlotter

open Math.Geometry
open Math.Units

open SharpVG

type Plotter = { Canvas: Canvas; Layers: Layer seq }


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Plotter =
    let create (canvas: Canvas) (layers: Layer seq) : Plotter = { Canvas = canvas; Layers = layers }

    /// The Cartesian to SvgCoordinates reference frame.
    /// The top left point in Svg coordinates is the origin point for this system.
    /// This reference frame excludes the margins of the canvas and only
    /// includes the working area of the canvas (everything except the margins)
    let svgFrame (canvas: Canvas) : Frame2D<Meters, Cartesian, SvgCoordinates> =
        let width = -canvas.Margin
        let height = (Canvas.height canvas) - canvas.Margin
        let topLeft = Point2D.xy width height

        Frame2D.atPoint topLeft |> Frame2D.reverseY

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
    let fromLayer (conversionFrame: Frame2D<Meters, Cartesian, SvgCoordinates>) (layer: Layer) : SharpVG.Group =
        let convertedLayerGeometry = coordinateConversion conversionFrame layer.Geometry

        PenPlotter.Svg.fromGeometries layer.Pen (List.ofSeq convertedLayerGeometry)

    /// A plotter needs to convert all the geometries within it from the Cartesian
    /// coordinate system to the SvgCoordinates system. This translation also adds
    /// the margin to the page.
    let fromPlotter (plotter: Plotter) : SharpVG.Svg =
        let svgReferenceFrame = svgFrame plotter.Canvas

        let layerGroup: SharpVG.Group =
            let layerGroups =
                List.map (fromLayer svgReferenceFrame) (List.ofSeq plotter.Layers)
                |> List.map GroupElement.Group

            Group.ofList [] |> Group.withBody layerGroups

        let viewBox: SharpVG.ViewBox =
            ViewBox.create
                (Svg.GeometryToSharpVG.toPoint Point2D.origin)
                (Svg.GeometryToSharpVG.toArea plotter.Canvas.Size)

        Svg.ofGroup layerGroup |> Svg.withViewBox viewBox


    let toSvg (plotter: Plotter) : string = Svg.toString (fromPlotter plotter)
