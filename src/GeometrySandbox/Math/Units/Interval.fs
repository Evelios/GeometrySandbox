module Math.Units.Interval

open Math.Units

let linspace (start: Quantity<'Units>) (stop: Quantity<'Units>) (n: int) : Quantity<'Units> seq =
    let step = (stop - start) / float (n - 1)
    seq { for i in 0 .. n - 1 -> start + float i * step }
