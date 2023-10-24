module PenPlotter.Geometry

open Math.Geometry
open Math.Units

module Size2D =
    let relativeTo
        (frame: Frame2D<Meters, 'InitialCoordinates, 'FinalCoordinates>)
        (size: Size2D<'InitialCoordinates, 'FinalCoordiantes>)
        : Size2D<Meters, 'FinalCoordinates> =
        size

/// Convert a generic geometry object into a new reference frame. This allows
/// for translating geometry into new coordinate systems.
let relativeTo
    (conversionFrame: Frame2D<Meters, 'InitialCoordinates, 'FinalCoordinates>)
    (geometry: IGeometry<'InitialCoordinates>)
    : IGeometry<'FinalCoordinates> =
    match geometry with
    | :? Vector2D<Meters, 'InitialCoordinates> as vector ->
        Vector2D.relativeTo conversionFrame vector :> IGeometry<'FinalCoordinates>
    | :? Size2D<Meters, 'InitialCoordinates> as size ->
        Size2D.relativeTo conversionFrame size :> IGeometry<'FinalCoordinates>
    | :? Point2D<Meters, 'InitialCoordinates> as point ->
        Point2D.relativeTo conversionFrame point :> IGeometry<'FinalCoordinates>
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

    | _ -> failwith "Geometry type is not supported\n{geometry.GetType()}: {geometry}"

let translate
    (translation: Vector2D<Meters, 'Coordinates>)
    (geometry: IGeometry<'Coordinates>)
    : IGeometry<'Coordinates> =
    match geometry with
    | :? LineSegment2D<Meters, 'Coordinates> as line ->
        LineSegment2D.translateBy translation line :> IGeometry<'Coordinates>
    | :? Circle2D<Meters, 'Coordinates> as circle -> Circle2D.translateBy translation circle :> IGeometry<'Coordinates>
    | :? Rectangle2D<Meters, 'Coordinates> as rect ->
        Rectangle2D.translateBy translation rect :> IGeometry<'Coordinates>
    | :? Polygon2D<Meters, 'Coordinates> as polygon ->
        Polygon2D.translateBy translation polygon :> IGeometry<'Coordinates>
    | :? Polyline2D<Meters, 'Coordinates> as polyline ->
        Polyline2D.translateBy translation polyline :> IGeometry<'Coordinates>
    | _ ->
        failwith (
            "Unable to translate geometry, this geometry type is not supported.\n"
            + $"{geometry.GetType()}: {geometry}"
        )
