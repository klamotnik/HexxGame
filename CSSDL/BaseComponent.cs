using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public abstract class BaseComponent : IDisposable
    {
        public IntPtr handle;

        public BaseComponent(IntPtr handle)
        {
            this.handle = handle;
        }

        public void Dispose()
        {
            CloseHandle();
            GC.SuppressFinalize(this);
        }

        public static implicit operator IntPtr(BaseComponent baseComponent)
        {
            return baseComponent.handle;
        }

        public abstract void CloseHandle();
    }
}
