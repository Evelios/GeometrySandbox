module GeometrySandbox.Extensions.Size2D

open Geometry
open GeometrySandbox


// ---- Accessors ----

let orientation (size: Size2D<'Unit>) : Orientation =
    if size.Height > size.Width then
        Portrait
    else
        Landscape

let height (size: Size2D<'Unit>) : Length<'Unit> = size.Width

let width (size: Size2D<'Unit>) : Length<'Unit> = size.Width

// Returns the dimensions of the size as a tuple in the form (height, width)
let dimensions (size: Size2D<'Unit>) : Length<'Unit> * Length<'Unit> = size.Height, size.Width

let scale (x: float) (size: Size2D<'Unit>) : Size2D<'Unit> =
    { Height = x * size.Height
      Width = x * size.Width }

// ---- Modifiers ----

let setOrientation (orientation: Orientation) (size: Size2D<'Unit>) =
    match orientation, size.Height, size.Width with
    | Portrait, height, width when height < width -> { Width = height; Height = width }
    | Landscape, height, width when width < height -> { Width = height; Height = width }
    | _ -> size

let setHeight (height: Length<'Unit>) (size: Size2D<'Unit>) = { size with Height = height }

let setWidth (width: Length<'Unit>) (size: Size2D<'Unit>) = { size with Width = width }
