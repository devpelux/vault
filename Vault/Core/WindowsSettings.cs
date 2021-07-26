using System;
using System.Windows;

namespace Vault.Core
{
    public record WindowParams(double Height, double Width, double Top, double Left)
    {
        public static readonly WindowParams INVALID = new(-1, -1, -1, -1);
    }

    public sealed class WindowsSettings : IDisposable
    {
        #region Instance

        public bool IsValidInstance { get; private set; } = true;

        private static WindowsSettings _instance;

        public static WindowsSettings Instance
        {
            get
            {
                if (_instance == null || !_instance.IsValidInstance)
                {
                    _instance = new WindowsSettings();
                }
                return _instance;
            }
        }

        #endregion


        private WindowsSettings() { }

        public void Dispose()
        {
            Save();
            IsValidInstance = false;
        }

        public void Save() => Settings.Instance.Save();

        public WindowParams GetWindowParams(string window, WindowParams defaultParam)
        {
            int userID = Session.Instance.UserID;

            double height = Settings.Instance.GetSetting($"{userID}_{window}_HEIGHT") as double? ?? defaultParam.Height;
            double width = Settings.Instance.GetSetting($"{userID}_{window}_WIDTH") as double? ?? defaultParam.Width;
            double top = Settings.Instance.GetSetting($"{userID}_{window}_TOP") as double? ?? defaultParam.Top;
            double left = Settings.Instance.GetSetting($"{userID}_{window}_LEFT") as double? ?? defaultParam.Left;

            return new WindowParams(height, width, top, left);
        }

        public void SetWindowParams(string window, WindowParams param)
        {
            int userID = Session.Instance.UserID;

            Settings.Instance.SetSetting($"{userID}_{window}_HEIGHT", param.Height);
            Settings.Instance.SetSetting($"{userID}_{window}_WIDTH", param.Width);
            Settings.Instance.SetSetting($"{userID}_{window}_TOP", param.Top);
            Settings.Instance.SetSetting($"{userID}_{window}_LEFT", param.Left);
        }

        public WindowState GetWindowState(string window, WindowState defaultState)
        {
            return Settings.Instance.GetSetting($"{Session.Instance.UserID}_{window}_WINDOWSTATE") as WindowState? ?? defaultState;
        }

        public void SetWindowState(string window, WindowState state)
        {
            Settings.Instance.SetSetting($"{Session.Instance.UserID}_{window}_WINDOWSTATE", state);
        }
    }
}
