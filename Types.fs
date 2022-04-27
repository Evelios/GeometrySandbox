namespace GeometrySandbox

open Geometry

// ---- Top Level Types --------------------------------------------------------

[<RequireQualifiedAccess>]
type LengthUnit =
    | Pixels
    | Points
    | Inches
    | Meters
    | Centimeters
    | Millimeters

type Orientation =
    | Landscape
    | Portrait

type Seed = int

[<RequireQualifiedAccess>]
type Action =
    | ChangeOrientation of Orientation
    | ChangeHeight of float
    | ChangeWidth of float
    | ChangeSeed of Seed
    | ChangeUnit of LengthUnit
    | ZoomIn
    | ZoomOut
    | ZoomToFullSize

// ---- UI Models --------------------------------------------------------------

type UserViewport = UserViewport

type YDownCoordinates = YDownCoordinates

type Model =
    { Size: Size2D<Meters>
      Unit: LengthUnit
      Seed: int
      ViewScale: float }
