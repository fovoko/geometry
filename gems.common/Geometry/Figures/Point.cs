﻿namespace gems.common.Geometry.Figures
{

    public struct Point(double x, double y)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
    }
}
