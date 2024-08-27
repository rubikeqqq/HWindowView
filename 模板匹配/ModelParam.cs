using HalconDotNet;

namespace 模板匹配
{
    public class ModelParam
    {
        public HTuple NumLevels = 5;
        public double AngleStart = -45;
        public double AngleExtent = 90;
        public HTuple AngleStep = "auto";
        public double ScaleMin = 0.7;
        public double ScaleMax = 1.3;
        public HTuple ScaleStep = "auto";
        public HTuple Optimization = "none";
        public string Metric = "ignore_global_polarity";
        public HTuple Contrast = 40;
        public HTuple MinContrast = 10;
    }

    public class FindModelParam
    {
        public double AngleStart = -45;
        public double AngleExtent = 90;
        public double ScaleMin = 0.7;
        public double ScaleMax = 1.3;
        public double MinScore = 0.5;
        public int NumMatches = 0;
        public double MaxOverlap = 0.5;
        public HTuple SubPixel = "least_squares";
        public HTuple NumLevels = 5;
        public double Greediness = 0.9;
        public HTuple Row = 0;
        public HTuple Column = 0;
        public HTuple Angle = 0;
        public HTuple Scale = 0;
        public HTuple Score = 0;
    }
}
