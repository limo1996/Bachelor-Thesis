﻿namespace PiStudio.Win10.Data
{
    public class PiCanvasContentChangedEventArgs
    {
        public ContentChangedType Type { get; set; }
        public SVGCurve Curve { get; set; }
    }

    public enum ContentChangedType
    {
        Cleared,
        Added,
        Removed
    }
}
