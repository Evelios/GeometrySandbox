namespace PenPlotter

open Math.Units

type Pen = { Thickness: Length; Color: string }

module Pen =
    type Attribute =
        | Thickness of Length
        | Color of string

    let create (attrs: Attribute seq) : Pen =
        let defaultPen = { Thickness = Length.zero; Color = "" }

        let attributeAssignment pen attr =
            match attr with
            | Thickness thickness -> { pen with Thickness = thickness }
            | Color color -> { pen with Color = color }

        Seq.fold attributeAssignment defaultPen attrs

    let thickness (length: Length) : Attribute = Thickness length

    let color (color: string) : Attribute = Color color
