using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;

namespace VisualizzatoreGrafi
{
    /// <summary>
    /// A simple identifiable vertex.
    /// </summary>
    [DebuggerDisplay("{ID}")]
    public class PocVertex
    {
        public string ID { get; private set; }
        public Color Color { get; private set; }

        public PocVertex(string id, Color color)
        {
            ID = id;
            Color = color;
        }

        public override string ToString()
        {
            return string.Format("{0}", ID);
        }
    }
}
