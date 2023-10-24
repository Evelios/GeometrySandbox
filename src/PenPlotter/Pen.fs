namespace PenPlotter

open Math.Units

type Pen = { Thickness: Length; Color: string }

module Pen =
    let withThickness (length: Length) (color: string) : Pen = { Thickness = length; Color = color }
    let withColor (color: string) (length: Length) : Pen = { Thickness = length; Color = color }
