using System;

namespace Vault.Core
{
    public sealed class Session : IDisposable
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public byte[] Key { get; set; }

        #region Instance

        public bool IsValidInstance { get; private set; } = true;

        private static Session _instance;

        public static Session Instance
        {
            get
            {
                if (_instance == null || !_instance.IsValidInstance)
                {
                    _instance = new Session();
                }
                return _instance;
            }
        }

        #endregion


        private Session() => Clear();

        public void Dispose()
        {
            Clear();
            IsValidInstance = false;
        }

        public static void ClearInstance() => Instance.Dispose();

        public void Clear()
        {
            UserID = -1;
            Username = null;
            Key = null;
        }
    }
}
