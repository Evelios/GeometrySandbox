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


// ---- Modifiers ----

let setOrientation (orientation: Orientation) (size: Size2D<'Unit>) =
    match orientation, size.Height, size.Width with
    | Portrait, height, width when height < width -> { Width = height; Height = width }
    | Landscape, height, width when width < height -> { Width = height; Height = width }
    | _ -> size

let setHeight (height: Length<'Unit>) (size: Size2D<'Unit>) = { size with Height = height }

let setWidth (width: Length<'Unit>) (size: Size2D<'Unit>) = { size with Width = width }
