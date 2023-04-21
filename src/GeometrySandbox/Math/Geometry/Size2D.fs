module Math.Units.Size2D

open Math.Geometry
open Math.Units

open GeometrySandbox


// ---- Accessors ----

let orientation (size: Size2D<Meters>) : Orientation =
    if size.Height > size.Width then Portrait else Landscape

let height (size: Size2D<Meters>) : Length = size.Width

let width (size: Size2D<Meters>) : Length = size.Width

// Returns the dimensions of the size as a tuple in the form (height, width)
let dimensions (size: Size2D<Meters>) : Length * Length = size.Height, size.Width

let scale (x: float) (size: Size2D<Meters>) : Size2D<Meters> =
    { Height = x * size.Height
      Width = x * size.Width }

// ---- Modifiers ----

let setOrientation (orientation: Orientation) (size: Size2D<Meters>) =
    match orientation, size.Height, size.Width with
    | Portrait, height, width when height < width -> { Width = height; Height = width }
    | Landscape, height, width when width < height -> { Width = height; Height = width }
    | _ -> size

let setHeight (height: Length) (size: Size2D<Meters>) = { size with Height = height }

let setWidth (width: Length) (size: Size2D<Meters>) = { size with Width = width }
