namespace ComicBookReader.Nomenclatures
{    
    public static class Constants
    {
        public static class Image
        {
            public static int AngleRotation { get { return 90; } }
            public static int FullRotation { get { return 360; } }
            public static int NullRotation { get { return 0; } }
            public static double ScaleImageStep { get { return 0.2; } }
            public static int DivisorForCenterOfImage { get { return 2; } }
            public static int MinimumZoomPercentage { get { return 30; } }
            public static int MaximumZoomPercentage { get { return 400; } }
        }

        public static class General
        {
            public static int IntZero { get { return 0; } }
            public static int IntOne { get { return 1; } }
            public static int IntHundred { get { return 100; } }
            public static string StringZero { get { return "0"; } }
            public static string StringOne { get { return "1"; } }
        }

        public static class Web
        {
            public static string MethodPost { get { return "POST"; } }
            public static string MethodGet { get { return "GET"; } }
            public static string ContentText { get { return "text/plain"; } }
            public static string ContentBinaryFormData { get { return "multipart/form-data"; } }
        }

        public static class Links
        {
            public static string GetAllBottles { get { return "http://bottlewebapp.apphb.com/Serialized/"; } }
            public static string GetBottle { get { return "http://bottlewebapp.apphb.com/Serialized/Index/"; } }
            public static string GetImageBase64 { get { return "http://bottlewebapp.apphb.com/Serialized/GetImageBase/"; } }
            public static string SendBottle { get { return "http://bottlewebapp.apphb.com/Serialized/Post"; } }
            public static string SendImageBase64 { get { return "http://bottlewebapp.apphb.com/Serialized/PostImage/"; } }
        }
    }
}
