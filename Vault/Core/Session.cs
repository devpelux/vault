using System;
using System.Collections.Generic;

namespace Vault.Core
{
    public sealed class Session : IDisposable
    {
        private static Session _instance = null;
        public static Session Instance
        {
            get
            {
                if (_instance == null) _instance = new Session();
                return _instance;
            }
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public byte[] Key { get; set; }

        public WindowsData CategoriesWindowsData { get; set; }


        private Session() => Clear();

        public void Clear()
        {
            UserID = -1;
            Username = null;
            Key = null;
            CategoriesWindowsData = null;
        }

        public void LoadWindowsDatas()
        {
            List<WindowsData> windowsDatas = VaultDB.Instance.WindowsDatas.GetRecords(UserID);
            CategoriesWindowsData = windowsDatas.Find(d => d.Window == CategoriesWindow.WindowID);
        }

        public void SaveWindowsDatas()
        {
            SaveWindowsData(CategoriesWindowsData);
        }

        private static void SaveWindowsData(WindowsData windowsData)
        {
            if (windowsData != null)
            {
                if (windowsData.ID == WindowsDatas.NewID) VaultDB.Instance.WindowsDatas.AddRecord(windowsData);
                else VaultDB.Instance.WindowsDatas.UpdateRecord(windowsData);
            }
        }

        public void Dispose()
        {
            SaveWindowsDatas();
            Clear();
        }
    }
}
