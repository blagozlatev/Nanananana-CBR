namespace ComicBookReader.Nomenclatures
{    
    public static class Constants
    {
        public static class Image
        {           
            /// <summary>
            /// 
            /// </summary>
            public static double ScaleImageStep { get { return 0.2; } }            
            /// <summary>
            /// 
            /// </summary>
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
            /// <summary>
            /// The constant represents the pattern for the Regular Express matching.
            /// Current value is ""d*.png|d*.jpg|d*.jpeg|d*.tiff|d*.gif"
            /// </summary>
            public static string RegExPattern
            {
                get                    
                { return "d*.png|d*.jpg|d*.jpeg|d*.tiff|d*.gif"; }
            }

            /// <summary>
            /// 
            /// </summary>
            public static string DefaultOpenDialogExtension
            {
                get
                { return ".cbr"; }
            }

            /// <summary>
            /// 
            /// </summary>
            public static string OpenDialogFilter
            {
                get
                { return "Comic Book Archive (*.cbr, *.cbz, *.cbt, *.cba, *.cb7)|*.cbr;*.cbz;*.cbt;*.cba;*.cb7"; }
            }
        }
    }
}
