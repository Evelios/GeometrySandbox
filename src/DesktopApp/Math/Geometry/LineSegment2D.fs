module Math.Geometry.LineSegment2D

open Math.Units
open Math.Geometry


/// Get the linearly spaced points from the start point of the segment to the end point of the segment.
let linspace (line: LineSegment2D<Meters, 'Coordinates>) (n: int) : Point2D<Meters, 'Coordinates> seq =
    seq { for i in 0 .. n - 1 -> Point2D.lerp line.Start line.Finish (float i / (float n - 1.)) }
