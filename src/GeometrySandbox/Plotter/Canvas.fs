namespace Plotter

open Math.Units
open Math.Geometry

type Canvas = { Size: Size2D<Meters> }

module Canvas =
    let create (size: Size2D<Meters>) : Canvas = { Size = size }
