namespace PenPlotter

open Math.Units
open Math.Geometry

type Canvas<'Coordinates> =
    { Size: Size2D<Meters, 'Coordinates>
      Margin: Length }

module Canvas =
    // ---- Builders -----------------------------------------------------------

    /// Create a canvas of a size with padding around the borders to create a margin where nothing is drawn.
    let create (size: Size2D<Meters, 'Coordinates>) (margin: Length) : Canvas<'Coordinates> =
        { Size = size; Margin = margin }

    /// Create a canvas of a size without any margins.
    let ofSize (size: Size2D<Meters, 'Coordinates>) : Canvas<'Coordinates> = { Size = size; Margin = Length.zero }

    // ---- Modifiers ----------------------------------------------------------

    /// Modify the canvas to have the desired margin border around the page.
    let withMargin (margin: Length) (canvas: Canvas<'Coordinates>) : Canvas<'Coordinates> =
        { canvas with Margin = margin }

    // ---- Accessors ----------------------------------------------------------

    /// The total height of the canvas including the margins
    let height (canvas: Canvas<'Coordinates>) : Length = canvas.Size.Height

    /// The total width of the canvas including the margins
    let width (canvas: Canvas<'Coordinates>) : Length = canvas.Size.Width

    /// The height of the area that is being drawn on. This is the canvas height WITHOUT the margins.
    let workingHeight (canvas: Canvas<'Coordinates>) : Length = canvas.Size.Height - 2. * canvas.Margin

    /// The width of the area that is being drawn on. This is the canvas width WITHOUT the margins.
    let workingWidth (canvas: Canvas<'Coordinates>) : Length = canvas.Size.Width - 2. * canvas.Margin

    // ---- Page Sizes ---------------------------------------------------------

    /// Create a size with a particular orientation. This is a helper
    /// function that makes it easier to create default page sizes.
    let private withOrientation (orientation: Orientation) (side1: Length) (side2: Length) : Size2D<Meters, Cartesian> =
        let min = Length.min side1 side2
        let max = Length.max side1 side2

        match orientation with
        | Orientation.Portrait -> Size2D.create min max
        | Orientation.Landscape -> Size2D.create max min

    let a4 (orientation: Orientation) : Size2D<Meters, Cartesian> =
        withOrientation orientation (Length.millimeters 1189.) (Length.millimeters 841.)
