namespace PenPlotter

open Math.Geometry


type Layer = { Pen: Pen; Geometry: IGeometry seq }

module Layer =
    let withPen (pen: Pen) (geometry: IGeometry seq) : Layer = { Pen = pen; Geometry = geometry }
