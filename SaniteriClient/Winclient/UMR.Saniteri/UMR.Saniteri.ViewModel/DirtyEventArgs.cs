using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.ViewModel
{
    public class DirtyEventArgs : EventArgs
    {
        public DirtyEventArgs(bool _dirty)
        {
            m_IsDirty = _dirty;
        }
        bool m_IsDirty;

        public bool IsDirty
        {
            get { return m_IsDirty; }
            set { m_IsDirty = value; }
        }
    }
}
