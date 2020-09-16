using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vault
{
    public interface IMessageReceiver
    {
        void ReceiveMessage(string message, object obj);
    }
}
