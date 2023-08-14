namespace PenPlotter

type Plotter = { Canvas: Canvas; Layers: Layer seq }

module Plotter =
    let create (canvas: Canvas) (layers: Layer seq) : Plotter = { Canvas = canvas; Layers = layers }

    module Svg =
        open SharpVG
        open Math.Geometry
        open Math.Units

        let conversion (len: Math.Units.Length) : float = Length.inCssPixels len

        /// Create an SVG style based on the pen that is being used
        let styleFromPen (pen: PenPlotter.Pen) : Style =
            let strokeWidth = SharpVG.Length.ofFloat (conversion pen.Thickness)
            let svgPen = SharpVG.Pen.createWithWidth (Color.ofName Colors.Black) strokeWidth
            Style.empty |> Style.withStrokePen svgPen

        /// Create an SVG based on a pen and the input geometry
        let fromGeometry (pen: PenPlotter.Pen) (geometry: IGeometry) : Element =
            let style = styleFromPen pen
            
            match geometry with
            | :? Line2D<Meters, Cartesian> as line ->
                let start = Point.ofFloats (conversion line.Start.X, conversion line.Start.Y)
                let finish = Point.ofFloats (conversion line.Finish.X, conversion line.Finish.Y)
                Line.create start finish |> Element.createWithStyle style 

            | _ -> failwith $"Unable to create SVG from geometry:\n{geometry}"

    let toSvg (plotter: Plotter): string = ""
