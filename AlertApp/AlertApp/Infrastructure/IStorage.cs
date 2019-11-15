using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface IStorage
    {
        void SaveFile(string filename, byte[] data);
    }
}
