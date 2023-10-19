namespace PenPlotter

open Math.Geometry
open Math.Units
open SharpVG
open System

open PenPlotter

type Plotter<'Coordinates> =
    { Canvas: Canvas<'Coordinates>
      Layers: Layer<'Coordinates> seq }


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Plotter =
    let create (canvas: Canvas<'Coordinates>) (layers: Layer<'Coordinates> seq) : Plotter<'Coordinates> =
        { Canvas = canvas; Layers = layers }
