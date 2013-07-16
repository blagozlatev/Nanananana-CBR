using Microsoft.Win32;

namespace NananananaCBR
{
    class KeyHandler
    {
        static RegistryKey rk = Registry.CurrentUser.CreateSubKey(Constants.Strings.RegistryKeyName);

        public static void setLibraryDirectory(string Directory)
        {
            rk.SetValue(Constants.Strings.RegistryKeyValue, Directory);
        }

        public static string getLibraryDirectory()
        {
            if (rk.GetValue(Constants.Strings.RegistryKeyValue) == null)
            {
                rk.SetValue(Constants.Strings.RegistryKeyValue, Constants.Strings.RegistryInitialDirectory);
                return (string)rk.GetValue(Constants.Strings.RegistryKeyValue);
            }
            return (string)rk.GetValue(Constants.Strings.RegistryKeyValue);
        }
    }
}
