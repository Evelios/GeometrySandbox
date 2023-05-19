namespace Plotter

type Geometry = Circle

type Layer = { Pen: Pen; Geometry: Geometry seq }

module Layer =
    let withPen (pen: Pen) (geometry: Geometry seq) : Layer = { Pen = pen; Geometry = geometry }
