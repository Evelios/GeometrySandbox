(*** hide ***)
#r "../src/PenPlotter/bin/Debug/net6.0/PenPlotter.dll"
// #r "../src/PenPlotter/bin/Release/net6.0/PenPlotter.dll"
#r "nuget: Math.Units"
#r "nuget: Math.Geometry"

(**

# Pen Plotter API

The `PenPlotter` module provides the ability to render geometry onto the canvas using multiple different pens.

In order to use our project, we have a couple dependencies that we need to
include. We have our `PenPlotter` API to include. This library depends on
the `Math.Units` and the `Math.Geometry` packages as the main ways of creating
content for an artwork.
*)

open PenPlotter
open Math.Units
open Math.Geometry

(**
## Setup

Here we need to create the page and pens that we are going to use to draw our project.

![Explanation of canvas working dimensions being the width and height without the margins](./img/Canvas Dimensions.png)
*)


// Create the canvas that we are going to be working on
let canvas =
    let margin = Length.inch
    let a3 = Canvas.Size.a4 Orientation.Landscape

    Canvas.create a3 margin

// We can get the dimensions of the page within the margin that we will be drawing on
let width = Canvas.workingWidth canvas
let height = Canvas.workingHeight canvas

(**
## Creating Geometries

When working with the Pen Plotter API, you work within the cartesian coordinate
system. The origin point (0, 0) is the bottom left hand corner with the y-axis
increasing upward, and the x-axis increasing to the right. This library takes
care of converting the coordinate system from the cartesian coordinate system
to the SVG, y down coordinate system when rendering. This simplifies the
geometry creation process to work within the more common coordinate system.
*)

let circle: Circle2D<Meters, Cartesian> = 
    Circle2D.withRadius (height/4.) (Point2D.xy (width/3.) (height/3.))
    
let square : BoundingBox2D<Meters, Cartesian> =
    BoundingBox2D.from 
        (Point2D.xy (width/2.) (height/2.))
        (Point2D.xy (width*2./3.) (height*2./3.))
        
let triangle : Triangle2D<Meters, Cartesian> = 
    Triangle2D.from
        (Point2D.xy (width/2.) (height/2.))
        (Point2D.xy (width/3.) (height/3.))
        (Point2D.xy (width*2./3.) (height/3.))

(**
## Drawing to the canvas

To draw to the canvas, we need to create some pen configurations that we
will use to draw the geometries with.
*)

let red = 
    Pen.create [
        Pen.thickness (Length.millimeters 0.5)
        Pen.color "#FF0000" ]
    
let green = 
    Pen.create [
        Pen.thickness (Length.millimeters 0.7)
        Pen.color "#00FF00" ]
    
let blue = 
    Pen.create [
        Pen.thickness (Length.millimeters 0.3)
        Pen.color "#0000FF" ]

(**
Now that we have created our pens, we can use them to draw our
geometries to the canvas.
*)

// Drawing all the shapes with the same pen color
let plotRed = Plotter.create canvas [
    Layer.withPen red [ circle; square; triangle ]
]

// Drawing each shape with a different pen color
let plotColored = Plotter.create canvas [
    Layer.withPen red [ circle ]
    Layer.withPen green [ square ]
    Layer.withPen blue [ triangle ]
]

(**
## Rendering

Now that we have created our pen plotter, we can render it to SVG so that we
can view the work, or save it to send to pen plotter software for drawing.

*)

let svg = Plotter.toSvg plotColored
