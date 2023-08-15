namespace PenPlotter

type Plotter = { Canvas: Canvas; Layers: Layer seq }


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Plotter =
    let create (canvas: Canvas) (layers: Layer seq) : Plotter = { Canvas = canvas; Layers = layers }

    let toSvg (plotter: Plotter): string = ""
