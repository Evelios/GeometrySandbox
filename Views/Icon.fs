// Icons can be found here https://materialdesignicons.com/
module GeometrySandbox.Views.Icon

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Controls.Shapes
open Avalonia.Media

type Size =
    | Small
    | Medium
    | Large

// ---- Sizes ----

let small = Small
let medium = Medium
let large = Large


// ---- Builders ----

let private icon (data: string) (size: Size) (color: string) =
    let baseSize = 24.

    let sizePx =
        match size with
        | Small -> 16.
        | Medium -> 20.
        | Large -> 24.

    ViewBox.create [
        Viewbox.stretch Stretch.Uniform
        Viewbox.width sizePx
        Viewbox.height sizePx
        Viewbox.child (
            Canvas.create [
                Canvas.width baseSize
                Canvas.height baseSize
                Canvas.children [
                    Path.create [ Path.fill color; Path.data data ]
                ]
            ]
        )
    ]


// ---- Actions ----

let save =
    icon
        "M15,9H5V5H15M12,19A3,3 0 0,1 9,16A3,3 0 0,1 12,13A3,3 0 0,1 15,16A3,3 0 0,1 12,19M17,3H5C3.89,3 3,3.9 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V7L17,3Z"

let resize =
    icon "M10.59,12L14.59,8H11V6H18V13H16V9.41L12,13.41V16H20V4H8V12H10.59M22,2V18H12V22H2V12H6V2H22M10,14H4V20H10V14Z"

// ---- Types ----

let orientation =
    icon
        "M8.5,13.5L11,16.5L14.5,12L19,18H5M21,19V5C21,3.89 20.1,3 19,3H5A2,2 0 0,0 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19Z"

let portrait =
    icon
        "M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"

let landscape =
    icon "M14,6L10.25,11L13.1,14.8L11.5,16C9.81,13.75 7,10 7,10L1,18H23L14,6Z"

let portraitPage =
    icon "M17,19H7V5H17M17,3H7A2,2 0 0,0 5,5V19A2,2 0 0,0 7,21H17A2,2 0 0,0 19,19V5C19,3.89 18.1,3 17,3Z"

let landscapePage =
    icon "M19,17H5V7H19M19,5H5A2,2 0 0,0 3,7V17A2,2 0 0,0 5,19H19A2,2 0 0,0 21,17V7C21,5.89 20.1,5 19,5Z"

// ---- Window States ----

let multiple =
    icon "M3 11H11V3H3M5 5H9V9H5M13 21H21V13H13M15 15H19V19H15M3 21H11V13H3M5 15H9V19H5M13 3V11H21V3M19 9H15V5H19Z"

let frame =
    icon
        "M10,14.29L6.5,19H17.46L14.75,15.46L12.78,17.8L10,14.29M5,21V7H18.96V21H5M12,2.4L14.61,5.03H9.37L12,2.4M5,5.03C4.5,5.03 4,5.22 3.61,5.61C3.2,6 3,6.46 3,7V21C3,21.5 3.2,22 3.61,22.39C4,22.8 4.5,23 5,23H18.96C19.5,23 19.96,22.8 20.37,22.39C20.77,22 21,21.5 21,21V7C21,6.46 20.77,6 20.37,5.61C19.96,5.22 19.5,5.03 18.96,5.03H16L12,1L7.96,5.03H5Z"

let carousel =
    icon "M2 6H6V17H2V6M7 19H17V4H7V19M9 6H15V17H9V6M18 6H22V17H18V6Z"

let fullscreen =
    icon "M5,5H10V7H7V10H5V5M14,5H19V10H17V7H14V5M17,14H19V19H14V17H17V14M10,17V19H5V14H7V17H10Z"

let fullscreenExit =
    icon "M14,14H19V16H16V19H14V14M5,14H10V19H8V16H5V14M8,5H10V10H5V8H8V5M19,8V10H14V5H16V8H19Z"

// ---- Tools ----

let rulerSquare =
    icon
        "M3,5V21H9V19.5H7V18H9V16.5H5V15H9V13.5H7V12H9V10.5H5V9H9V5H10.5V9H12V7H13.5V9H15V5H16.5V9H18V7H19.5V9H21V3H5A2,2 0 0,0 3,5M6,7A1,1 0 0,1 5,6A1,1 0 0,1 6,5A1,1 0 0,1 7,6A1,1 0 0,1 6,7Z"

let lockClosed =
    icon
        "M12,17C10.89,17 10,16.1 10,15C10,13.89 10.89,13 12,13A2,2 0 0,1 14,15A2,2 0 0,1 12,17M18,20V10H6V20H18M18,8A2,2 0 0,1 20,10V20A2,2 0 0,1 18,22H6C4.89,22 4,21.1 4,20V10C4,8.89 4.89,8 6,8H7V6A5,5 0 0,1 12,1A5,5 0 0,1 17,6V8H18M12,3A3,3 0 0,0 9,6V8H15V6A3,3 0 0,0 12,3Z"

let lockOpen =
    icon
        "M18,20V10H6V20H18M18,8A2,2 0 0,1 20,10V20A2,2 0 0,1 18,22H6C4.89,22 4,21.1 4,20V10A2,2 0 0,1 6,8H15V6A3,3 0 0,0 12,3A3,3 0 0,0 9,6H7A5,5 0 0,1 12,1A5,5 0 0,1 17,6V8H18M12,17A2,2 0 0,1 10,15A2,2 0 0,1 12,13A2,2 0 0,1 14,15A2,2 0 0,1 12,17Z"
