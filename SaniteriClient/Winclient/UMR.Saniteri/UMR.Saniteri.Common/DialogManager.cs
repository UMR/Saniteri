using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UMR.Saniteri.Common
{
    public static class DialogManager
    {
        public static Action<string> popup;
        public static Func<string, string, bool> confirm;
        public static Func<string, string> openFile;
    }
}
