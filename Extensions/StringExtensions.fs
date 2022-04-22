module GeometrySandbox.Extensions.String

/// Try parsing a string into an integer. Will return None on failure
let toInt (s: string) : int option =
    let i = 0

    if System.Int32.TryParse(s, ref i) then
        Some i
    else
        None

/// Try parsing a string into an integer. Will return None on failure
let toFloat (s: string) : float option =
    let f = 0.

    if System.Double.TryParse(s, ref f) then
        Some f
    else
        None
