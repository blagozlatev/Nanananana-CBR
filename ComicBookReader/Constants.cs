namespace ComicBookReader.Nomenclatures
{    
    public static class Constants
    {
        public static class Image
        {           
            /// <summary>
            /// The step for incrementing/decrementing ScaleTransform and TranslateTransform
            /// </summary>
            public static double ScaleImageStep { get { return 0.2; } }            
            /// <summary>
            /// The minimum value for the ScaleTransform
            /// </summary>
            public static double MinimumScaleXYValue { get { return 0.7; } }
        }

        public static class General
        {
            /// <summary>
            /// Integer constant for 0
            /// </summary>
            public static int IntZero { get { return 0; } }
            /// <summary>
            /// Integer constant for 1
            /// </summary>
            public static int IntOne { get { return 1; } }
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
            /// The constant represents the default extension for the dialog open.
            /// Current value is ".cbr"
            /// </summary>
            public static string DefaultOpenDialogExtension
            {
                get
                { return ".cbr"; }
            }

            /// <summary>
            /// The constant represents the filter for the open file dialog.
            /// Current value is "Comic Book Archive (*.cbr, *.cbz, *.cbt, *.cba, *.cb7)|*.cbr;*.cbz;*.cbt;*.cba;*.cb7"
            /// </summary>
            public static string OpenDialogFilter
            {
                get
                { return "Comic Book Archive (*.cbr, *.cbz, *.cbt, *.cba, *.cb7)|*.cbr;*.cbz;*.cbt;*.cba;*.cb7"; }
            }

            /// <summary>
            ///This constant is used to get the directory of the 7z dynamic-link library.
            /// Current value is "7z.dll"
            /// </summary>
            public static string SevenZipLibrary { get { return "7z.dll"; } }
        }
    }
}
