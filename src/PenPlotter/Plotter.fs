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

    /// Map over all the geometry objects within the plotter drawing.
    /// This can be geometric operations and coordinate conversion operations.
    let mapGeometry (map: IGeometry<'ACoords> -> IGeometry<'BCoords>) (plotter: Plotter<'ACoords>) : Plotter<'BCoords> =
        { Canvas = Canvas.mapGeometry map plotter.Canvas
          Layers = Seq.map (Layer.mapGeometry map) plotter.Layers }

    /// Get the plotter object relative to a reference frame. This
    let relativeTo (frame: Frame2D<Meters, 'ACoords, 'BCoords>) (plotter: Plotter<'ACoords>) =
        { Canvas = Canvas.relativeTo frame plotter.Canvas
          Layers = Seq.map (Layer.relativeTo frame) plotter.Layers }
