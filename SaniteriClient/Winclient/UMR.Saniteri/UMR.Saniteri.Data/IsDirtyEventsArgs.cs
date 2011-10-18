using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Data
{
    public class IsDirtyEventsArgs: EventArgs
    {
        private bool _isDirty;

        public bool IsDirty
        {
            get { return _isDirty; }
        }

        public IsDirtyEventsArgs(bool _dirty)
        {
            _isDirty = _dirty;
        }
    }
}
