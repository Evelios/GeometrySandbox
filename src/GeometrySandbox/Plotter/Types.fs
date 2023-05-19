namespace Plotter

open Math.Geometry
open Math.Units

type PlotterGeometry<'Coordinates> =
    | Line of Line2D<Meters, 'Coordinates>
    | Circle of Circle2D<Meters, 'Coordinates>
