namespace PenPlotter

open Math.Geometry

type Plotter = { Canvas: Canvas; Layers: Layer seq }


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Plotter =
    let create (canvas: Canvas) (layers: Layer seq) : Plotter = { Canvas = canvas; Layers = layers }

    let private coordinateConversion (geometry: IGeometry<Cartesian> seq) : IGeometry<SvgCoordinates> seq = geometry

    let toSvg (plotter: Plotter) : string = ""
