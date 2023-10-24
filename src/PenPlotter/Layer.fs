namespace PenPlotter

open Math.Units
open Math.Geometry


type Layer<'Coordinates> =
    { Pen: Pen
      Geometry: IGeometry<'Coordinates> seq }

module Layer =
    let withPen (pen: Pen) (geometry: IGeometry<'Coordinates> seq) : Layer<'Coordinates> =
        { Pen = pen; Geometry = geometry }

    let mapGeometry (map: IGeometry<'ACoord> -> IGeometry<'BCoord>) (layer: Layer<'ACoord>) : Layer<'BCoord> =
        let mappedGeometry: IGeometry<'BCoord> seq = Seq.map map layer.Geometry

        { Pen = layer.Pen
          Geometry = mappedGeometry }

    let relativeTo (frame: Frame2D<Meters, 'ACoords, 'BCoords>) (layer: Layer<'ACoords>) =
        mapGeometry (Geometry.relativeTo frame) layer
