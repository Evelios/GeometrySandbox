module PenPlotter.CoordinateConversion

open Math.Geometry
open Math.Units

/// Convert a generic geometry object into a new reference frame. This allows
/// for translating geometry into new coordinate systems.
let withFrame
    (conversionFrame: Frame2D<Meters, 'InitialCoordinates, 'FinalCoordinates>)
    (geometry: IGeometry<'InitialCoordinates>)
    : IGeometry<'FinalCoordinates> =
    match geometry with
    | :? LineSegment2D<Meters, 'InitialCoordinates> as line ->
        LineSegment2D.relativeTo conversionFrame line :> IGeometry<'FinalCoordinates>
    | :? Circle2D<Meters, 'InitialCoordinates> as circle ->
        Circle2D.relativeTo conversionFrame circle :> IGeometry<'FinalCoordinates>
    | :? Rectangle2D<Meters, 'InitialCoordinates> as rect ->
        Rectangle2D.relativeTo conversionFrame rect :> IGeometry<'FinalCoordinates>
    | :? Polygon2D<Meters, 'InitialCoordinates> as polygon ->
        Polygon2D.relativeTo conversionFrame polygon :> IGeometry<'FinalCoordinates>
    | :? Polyline2D<Meters, 'InitialCoordinates> as polyline ->
        Polyline2D.relativeTo conversionFrame polyline :> IGeometry<'FinalCoordinates>

    | _ ->
        failwith (
            "Unable to create SVG from geometry.\n"
            + " This can be caused by not using the SvgCoordinates coordinate system:\n"
            + $"{geometry.GetType()}: {geometry}"
        )
