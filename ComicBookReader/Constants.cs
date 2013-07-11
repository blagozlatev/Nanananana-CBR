namespace ComicBookReader.Nomenclatures
{    
    public static class Constants
    {
        public static class Image
        {           
            public static double ScaleImageStep { get { return 0.2; } }
            public static int DivisorForCenterOfImage { get { return 2; } }
            public static int MinimumZoomPercentage { get { return 30; } }
            public static int MaximumZoomPercentage { get { return 400; } }
            public static double MinimumScaleXYValue { get { return 0.6; } }
        }

        public static class General
        {
            public static int IntZero { get { return 0; } }
            public static int IntOne { get { return 1; } }
            public static int IntHundred { get { return 100; } }
            public static string StringZero { get { return "0"; } }
            public static string StringOne { get { return "1"; } }
        }

        public static class Strings
        {
            public static string RegExPattern
            {
                get                    
                { return "d*.png|d*.jpg|d*.jpeg|d*.tiff|d*.gif"; }
            }
        }
    }
}
