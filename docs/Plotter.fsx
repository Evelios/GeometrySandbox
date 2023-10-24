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
