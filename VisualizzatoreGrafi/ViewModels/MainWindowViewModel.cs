using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GraphSharp.Controls;
using System.Windows.Media;
using System.IO;



namespace VisualizzatoreGrafi
{
    public class PocGraphLayout : GraphLayout<PocVertex, PocEdge, PocGraph> { }



    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Data

        private string layoutAlgorithmType;
        private PocGraph graph;
        private List<String> layoutAlgorithmTypes = new List<string>();
        private int count;

        private List<MissioneTreno> Missioni { get; set; }
        private List<int> Posizioni { get; set; }

        #endregion
        
        #region Ctor
        public MainWindowViewModel()
        {
            Graph = new PocGraph(true);

            Missioni = new List<MissioneTreno>();
            Posizioni = new List<int>();


            //Add Layout Algorithm Types
            layoutAlgorithmTypes.Add("BoundedFR");
            layoutAlgorithmTypes.Add("Circular");
            layoutAlgorithmTypes.Add("CompoundFDP");
            layoutAlgorithmTypes.Add("EfficientSugiyama");
            layoutAlgorithmTypes.Add("FR");
            layoutAlgorithmTypes.Add("ISOM");
            layoutAlgorithmTypes.Add("KK");
            layoutAlgorithmTypes.Add("LinLog");
            layoutAlgorithmTypes.Add("Tree");

            //Pick a default Layout Algorithm Type
            LayoutAlgorithmType = "FR";

        }
        #endregion

        public void CaricaFile()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt|UMC Files (*.umc)|*.umc";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                Missioni = CaricaMissioni(filename);

                Refresh();

            }
        }

        /// <summary>
        /// Ritorna una lista di missioni (lista di ID di CDB) a partire da un file di testo
        /// </summary>
        private static List<MissioneTreno> CaricaMissioni(string nomefile)
        {
            List<MissioneTreno> missioni = new List<MissioneTreno>();

            if (!File.Exists(nomefile))
                return missioni;

            FileStream stream = null;
            StreamReader sr = null;
            try
            {
                stream = File.Open(nomefile, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(stream);

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    //Formato di una riga
                    // nometreno = [ x,y,z ]
                    if (string.IsNullOrEmpty(line) || line[0] == '#')
                        continue;

                    string[] tokens = line.Split('=');
                    if (tokens.Length > 1)
                    {
                        string nometreno = tokens[0].Trim();

                        string cdb = tokens[1].TrimStart(new[] { '[', ' ' });
                        cdb = cdb.TrimEnd(new[] { ']', ' ' });
                        cdb = cdb.Replace(" ", "");
                        List<string> cdbList = cdb.Split(',').ToList();
                        List<int> cdbListInt = cdbList.ConvertAll(Convert.ToInt32);

                        missioni.Add(new MissioneTreno(nometreno, cdbListInt));

                        Console.WriteLine("{0}= [{1}]", nometreno, cdb);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            return missioni;
        }

        public void CaricaPosizioni(string testo)
        {
            try
            {
                List<string> cdbList = testo.Split(',').ToList();
                Posizioni = cdbList.ConvertAll(Convert.ToInt32);
            }
            catch(Exception) {};

            if (Posizioni != null && Posizioni.Count == Missioni.Count)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
           



            //List<PocVertex> existingVertices = new List<PocVertex>();
            //existingVertices.Add(new PocVertex(String.Format("Barn Rubble{0}", count), Colors.Green)); //0
            //existingVertices.Add(new PocVertex(String.Format("Frank Zappa{0}", count), Colors.Red)); //1
            //existingVertices.Add(new PocVertex(String.Format("Gerty CrinckleBottom{0}", count), Colors.Orange)); //2


            //foreach (PocVertex vertex in existingVertices)
            //    Graph.AddVertex(vertex);


            ////add some edges to the graph
            //AddNewGraphEdge(existingVertices[0], existingVertices[1]);
            //AddNewGraphEdge(existingVertices[0], existingVertices[2]);


            //NotifyPropertyChanged("Graph");




            if (Missioni == null)
            {
                return;
            }

            graph = new PocGraph(true);
            count++;

            List<PocVertex> existingVertices = new List<PocVertex>();
            Dictionary<int, PocVertex> cdbInseriti = new Dictionary<int, PocVertex>();
            foreach (MissioneTreno missione in Missioni)
            {
                foreach (int cdb in missione.CdbList)
                {
                    if (!cdbInseriti.ContainsKey(cdb))
                    {
                        string txt = cdb.ToString();
                        Color vertexColor = Colors.Black;
                        if (Posizioni.Contains(cdb))
                        {
                            vertexColor = Colors.Red;

                            int idx = Posizioni.IndexOf(cdb);
                            MissioneTreno m = Missioni[idx];
                            txt = txt + "(" + m.NomeTreno + ")";
                        }

                        PocVertex v = new PocVertex(txt, vertexColor);
                        existingVertices.Add(v);
                        cdbInseriti.Add(cdb, v);
                    }
                }
            }

            foreach (PocVertex vertex in existingVertices)
                Graph.AddVertex(vertex);

            Dictionary<KeyValuePair<int, int>, PocEdge> edgeInseriti = new Dictionary<KeyValuePair<int, int>, PocEdge>();

            for (int i = 0; i < Missioni.Count; i++)
            {
                MissioneTreno missione = Missioni[i];
                int cdbprec = -1;
                int posTreno = -1;
                if (Posizioni.Count == Missioni.Count)
                {
                    posTreno = Posizioni[i];
                }
                

                foreach (int cdb in missione.CdbList)
                {
                    if (cdbprec != -1)
                    {
                        bool colorato = posTreno == cdbprec;

                        string edgeString = string.Format("{0}-{1} Connected", cdbInseriti[cdbprec].ID, cdbInseriti[cdb].ID);
                        PocEdge newEdge = new PocEdge(edgeString, cdbInseriti[cdbprec], cdbInseriti[cdb], colorato);

                        KeyValuePair<int, int> pair = new KeyValuePair<int, int>(cdbprec, cdb);

                        //se un edge c'è già ma io lo voglio colorato, tolgo quello che c'è già
                        if (colorato && edgeInseriti.ContainsKey(pair))
                        {
                            edgeInseriti.Remove(pair);
                        }

                        if (!edgeInseriti.ContainsKey(pair))
                        {
                            edgeInseriti.Add(pair, newEdge);
                        }
                    }
                    cdbprec = cdb;
                }
            }

            foreach (PocEdge edge in edgeInseriti.Values)
                Graph.AddEdge(edge);

            NotifyPropertyChanged("Graph");
        }

        #region Public Properties

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }


        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }



        public PocGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
