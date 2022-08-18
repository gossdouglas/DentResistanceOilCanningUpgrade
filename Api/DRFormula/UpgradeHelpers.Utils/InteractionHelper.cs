using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace UpgradeHelpers.Helpers
{
	public static class InteractionHelper
	{
        /// <summary>
        /// Key location in Windows registry to save values.
        /// </summary>
        private const string RegisterWindowsVBSettings = @"Software\VB and VBA Program Settings\";

        /// <summary>
        /// Obtains the name and value of an environment variable based on a list of environment variables ordered alphabetically.
        /// </summary>
        /// <param name="position">The zero-based position of the variable within the list of environment variables.</param>
        /// <returns>The name and value of the variables, separated by an equal sign (=).  If no value is found, returns an empty string.</returns>
        public static string GetEnvironmentVariableByIndex(int position)
		{
			var variables = System.Environment.GetEnvironmentVariables();
			if (position < 0 || position > variables.Count) return String.Empty;
			string x = variables.Keys.Cast<string>().ToArray().OrderBy(index => index).ElementAt(position);
			return String.Format("{0}={1}",  x, System.Environment.GetEnvironmentVariable(x));
		}

        /// <summary>
        /// Saves an application entry in the Windows registry.
        /// </summary>
        /// <param name="appName">String that contains the name of the application or project.</param>
        /// <param name="section">String that contains the name of the section to save the key.</param>
        /// <param name="key">String that contains the name of the key to be saved.</param>
        /// <param name="setting">String that contains the value to assign the key.</param>
        public static void SaveSettingRegistryKey(string appName, string section, string key, string setting)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey(RegisterWindowsVBSettings + appName + "\\" + section);
            rk.SetValue(key, setting);
        }

        /// <summary>
        /// Obtains the value (setting) from the registry through key.
        /// </summary>
        /// <param name="appName">String that contains the name of the application or project.</param>
        /// <param name="section">String that contains the name of the section where the key is located.</param>
        /// <param name="key">String that contains the name of the key to be returned.</param>
        /// <param name="sDefault">Optional. String that contains the value to return based on the key.</param>
        /// <returns>The setting value from an a</returns>
        public static string GetSettingRegistryKey(string appName, string section, string key, string sDefault = "")
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(RegisterWindowsVBSettings + appName + "\\" + section);
            if (rk != null) return rk.GetValue(key).ToString();
            return sDefault;
        }
	}
}
