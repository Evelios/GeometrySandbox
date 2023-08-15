module PenPlotter.Svg

open SharpVG
open Math.Geometry
open Math.Units

open PenPlotter

/// The unit conversion from the typed Length to untyped float used in the Svg coordinate system
let unwrapLength (len: Math.Units.Length) : float = Length.inCssPixels len

let toLength (len: Math.Units.Length) : SharpVG.Length =
    unwrapLength len
    |> SharpVG.Length.ofFloat

/// Create an Svg style based on the pen that is being used
let styleFromPen (pen: PenPlotter.Pen) : SharpVG.Style =
    let strokeWidth = toLength pen.Thickness
    let svgPen = SharpVG.Pen.createWithWidth (Color.ofName Colors.Black) strokeWidth
    Style.empty |> Style.withStrokePen svgPen
    
let toPoint (point: Point2D<Meters, 'Coordinates>) : Point =
    Point.ofFloats (unwrapLength point.X, unwrapLength point.Y)

/// Create an SVG based on a pen and the input geometry.
/// This needs to be used with the Cartesian coordinate system.
let fromGeometry (pen: PenPlotter.Pen) (geometry: IGeometry) : Element =
    let style = styleFromPen pen
    
    match geometry with
    | :? Line2D<Meters, Cartesian> as line ->
        let start = toPoint line.Start
        let finish = toPoint line.Finish
        
        Line.create start finish |> Element.createWithStyle style
        
    | :? Circle2D<Meters, Cartesian> as circle ->
        let center = toPoint circle.Center
        let radius = toLength circle.Radius
        
        Circle.create center radius |> Element.createWithStyle style

    | _ -> failwith $"Unable to create SVG from geometry:\n{geometry}"
