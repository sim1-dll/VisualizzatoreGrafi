using QuickGraph;
using System.Diagnostics;
using System.ComponentModel;
using System;
using System.Windows.Media;

namespace VisualizzatoreGrafi
{
    /// <summary>
    /// A simple identifiable edge.
    /// </summary>
    [DebuggerDisplay("{Source.ID} -> {Target.ID}")]
    public class PocEdge : Edge<PocVertex>, INotifyPropertyChanged
    {
        private string id;

        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("ID");
            }
        }

        private Color color;
        public Color EdgeColor
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged("EdgeColor");
            }
        }

        public PocEdge(string id, PocVertex source, PocVertex target, bool colorato)
            : base(source, target)
        {
            ID = id;
            if (colorato)
            {
                EdgeColor = Colors.Red;
            }
            else
            {
                EdgeColor = Colors.Gray;
            }
        }


        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}