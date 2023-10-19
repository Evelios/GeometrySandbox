namespace GeometrySandbox

open Math.Geometry
open Math.Units
open PenPlotter

// ---- Library Types ----------------------------------------------------------

open Avalonia.FuncUI.Types

type SimpleGenerator = unit -> IGeometry<Cartesian> seq

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

// ---- Top Level Gui Types ----------------------------------------------------

[<RequireQualifiedAccess>]
type PageViewMode =
    | SinglePage
    | FramedPage
    | MultiplePages
    | FullScreen

[<RequireQualifiedAccess>]
type Action =
    | ChangePageViewMode of PageViewMode
    | ChangeOrientation of Orientation
    | ChangeHeight of float
    | ChangeWidth of float
    | ChangeSeed of Seed
    | ChangeUnit of LengthUnit
    | ZoomIn
    | ZoomOut
    | ZoomToFullSize

// ---- UI Models --------------------------------------------------------------

type Model =
    { Size: Size2D<Meters, Cartesian>
      Unit: LengthUnit
      Seed: int
      ViewScale: float
      PageViewMode: PageViewMode }
