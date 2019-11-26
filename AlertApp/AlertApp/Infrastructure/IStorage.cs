using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface IStorage
    {
        string SaveFile(string filename, byte[] data);
    }
}
