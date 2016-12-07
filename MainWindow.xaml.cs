using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookSplitter
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileInfo fichero;
        GestorFicheros gestor = new GestorFicheros();
        public List<string> lineasLibro = new List<string>();
        private bool ficheroSeleccionado;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void reset()
        {
            this.labelLibro.Content = "";
            ficheroSeleccionado = false;
            fichero = null;
            lineasLibro.Clear();
            textBoxExp.Clear();
        }

        private void ButtonSeleccionarLibro_Click(object sender, RoutedEventArgs e)
        {
            reset();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (!String.IsNullOrEmpty(ofd.FileName))
            {
                ficheroSeleccionado = true;
                fichero = new FileInfo(ofd.FileName);
                this.labelLibro.Content = fichero.Name;
                gestor.nombreFichero = fichero.FullName;
                lineasLibro = gestor.leerLineas();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ficheroSeleccionado)
            {
                MessageBox.Show("Fichero no seleccionado");
            }
            else
            {
                string exp = textBoxExp.Text;
                int max;
                int.TryParse(textBoxMaxChar.Text, out max);
                if (max == 0) max = 50;
                if (!String.IsNullOrEmpty(exp))
                {
                    Regex regex = new Regex(exp);
                    for (int i = 0; i < lineasLibro.Count; i++)
                    {
                        string linea = lineasLibro[i];

                        if (regex.IsMatch(linea) && linea.Length<=max)
                        {
                            lineasLibro.Insert(i, "---");
                            i++;
                        }

                    }
                    string fileName = fichero.Directory.ToString() + @"\splitted_" + fichero.Name;
                    gestor.writeFile(fileName, lineasLibro);
                    Process.Start(fileName);


                }
            }
        }
    }
}
