namespace PenPlotter

open Math.Units

type Pen = { Thickness: Length; Color: int }

module Pen =
    type Attribute =
        | Thickness of Length
        | Color of int

    let create (attrs: Attribute seq) : Pen =
        let defaultPen = { Thickness = Length.zero; Color = 0x000000 }

        let attributeAssignment pen attr =
            match attr with
            | Thickness thickness -> { pen with Thickness = thickness }
            | Color color -> { pen with Color = color }

        Seq.fold attributeAssignment defaultPen attrs

    let thickness (length: Length) : Attribute = Thickness length

    /// Set the color with a hex value, eg Pen.color 0xFF34AB
    let color (color: int) : Attribute = Color color
