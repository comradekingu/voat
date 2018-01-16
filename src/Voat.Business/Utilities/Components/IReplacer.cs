using System;
using System.Collections.Generic;
using System.Text;

namespace Voat.Utilities.Components
{
    public interface IReplacer
    {
        string Replace(string content, object state);
    }
}
