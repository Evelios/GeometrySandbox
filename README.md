[fantomas]: https://github.com/fsprojects/fantomas
[f# formatting]: https://marketplace.visualstudio.com/items?itemName=asti.fantomas-vs
[fable]: https://fable.io/

# GeometrySandbox

This is a framework to get started creating generative art in F#. This framework's goal is to get you up and running
creating generative art as fast as possible! The aim of this framework is to enable everything in the workflow of making
creative works within a beautiful environment but still allow your work to go to living anywhere you want it to go. If
you want this work to live on a wall, resizing and saving is made easy for you. Let's say that you made an animation and
want people to be able to see the animation playing online. You can either export the rendering as a video or gif
format. If you really want to allow the user to have full interaction with the work you are creating, you can even embed
the code into your website by converting your animation into javascript with [Fable].

![Preview of the generative framework gui](./img/GeometrySandbox.png)

## Usage

The starting point of all the drawings is the canvas. From the canvas, we can then reference the height and width which
will help us to place our shapes on the page.

```fsharp
open Plotter

let canvas = Canvas.A4 Orientation.Landscape
let height = Canvas.height canvas
let width = Canvas.width canvas
```

Every drawing is done with some sort of pen. This can be anything that you can put in your pen-plotter, but for all of
them, the API is the same.

```fsharp
open Math.Units

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
```

We then need to create the geometries that we want to draw to the canvas. I'm doing my shapes so that they are drawn in
relation to the canvas width and height. If you are always going to use the fixed size canvas, you can use specific\
measurements for example if that process is easier for you.

```fsharp
open Math.Geometry

let circle = 
    Circle2D.withRadius (height/4) (Point2D.xy (width/3) (height/3))
    
let square =
    BoundingBox2D.from 
        (Point2D.xy (width/2) (height/2))
        (Point2D.xy (width*2/3) (height*2/3))
        
/// TODO: Fix this one        
let triangle = 
    Triangle2D.from
        (Point2D.xy (width/2) (height/2))
        (Point2D.xy (width/2) (height/2))
        (Point2D.xy (width*2/3) (height*2/3))
```

Once we have our canvas, pens, and geometries, then we can put them all together.

```fsharp
Plotter.create [
    Layer.withPen red [ Plotter.circle circle ]
    Layer.withPen green [ Plotter.boundingBox square ]
    Layer.withPen blue [ Plotter.triangle triangle ]
]
```

# Development

To run this application just type

```sh
dotnet run
```