namespace PenPlotter

open Math.Geometry


type Layer = { Pen: Pen; Geometry: IGeometry<Cartesian> seq }

module Layer =
    let withPen (pen: Pen) (geometry: IGeometry<Cartesian> seq) : Layer = { Pen = pen; Geometry = geometry }
