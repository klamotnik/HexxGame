using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class ListRow : Actor
    {
        public string Key { get; protected set; }

        public ListRow(Rectangle rectangle, string key) : base(rectangle)
        {
            Key = key;
            NeedRefresh = true;
        }
    }
}
