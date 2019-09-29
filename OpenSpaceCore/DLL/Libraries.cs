using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace OpenSpaceCore.DLL
{
    public static class Libraries
    {
        public static string[] GetDllList(string path) =>
            Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

        public static T LoadDll<T>(string path)
        {
            try
            {
                var dll = Assembly.LoadFile(path);
                var dllType = dll.GetExportedTypes().First(t => typeof(T).IsAssignableFrom(t));

                return (T)Activator.CreateInstance(dllType);
            }
            catch (BadImageFormatException)
            {
                MessageBox.Show("Cannot load selected file: Incorrect format.");
            }
            catch (MissingMethodException)
            {
                MessageBox.Show("Cannot load selected file: Class not found.");
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot load selected file.");
            }

            return default;
        }
    }
}