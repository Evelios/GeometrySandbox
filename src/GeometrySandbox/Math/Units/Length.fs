module Math.Units.Length

open Math.Units

open GeometrySandbox


let inUnit (unit: LengthUnit) (length: Length) : float =
    length
    |> match unit with
       | LengthUnit.Pixels -> Length.inCssPixels
       | LengthUnit.Points -> Length.inPoints
       | LengthUnit.Inches -> Length.inInches
       | LengthUnit.Meters -> Length.inMeters
       | LengthUnit.Centimeters -> Length.inCentimeters
       | LengthUnit.Millimeters -> Length.inMillimeters

let ofUnit (unit: LengthUnit) (amount: float) : Length =
    amount
    |> match unit with
       | LengthUnit.Pixels -> Length.cssPixels
       | LengthUnit.Points -> Length.points
       | LengthUnit.Inches -> Length.inches
       | LengthUnit.Meters -> Length.meters
       | LengthUnit.Centimeters -> Length.centimeters
       | LengthUnit.Millimeters -> Length.millimeters
