namespace PenPlotter

open Math.Geometry


type Layer<'Coordinates> =
    { Pen: Pen
      Geometry: IGeometry<'Coordinates> seq }

module Layer =
    let withPen (pen: Pen) (geometry: IGeometry<'Coordinates> seq) : Layer<'Coordinates> =
        { Pen = pen; Geometry = geometry }
        
    let mapGeometry (f: IGeometry<'InitialCoordinates> -> IGeometry<'FinialCoordinates>) (layer: Layer<'InitialCoordinates>) : Layer<'FinalCoordinates> =
        { layer with Geometry = layer.Geometry }
        
