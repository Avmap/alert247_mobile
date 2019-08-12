using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public enum DialogType
    {
        Password = 0,
        /// <summary>
        /// A set of answers that are in the same Table
        /// </summary>
        Text = 1,
        Numeric = 2,
        Phone = 3
    }
}
