using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Source
{
    /// <summary>
    /// Wrapper for WC3 types to bypass type inspection.
    /// </summary>
    public class Wrapper<T>
    {
        public T Value { get; }

        public Wrapper(T value)
        {
#if DEBUG
            if (value == null)
            {
                throw new Exception("Cannot wrap a null value!");
            }
#endif
            Value = value;
        }
    }

}
