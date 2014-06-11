using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisualizzatoreGrafi
{
    class MissioneTreno
    {
        internal MissioneTreno(string nomeTreno, List<int> cdbList)
        {
            NomeTreno = nomeTreno;
            CdbList = cdbList;
        }

        internal string NomeTreno { get; private set; }

        internal List<int> CdbList { get; private set; }

        internal List<int> Visitati { get; set; }
    }
}
