using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace Test01
{

    public partial class Window2 : Window
    {
        string pathXML = Window1.parentDirPath() + "\\BBDD\\XMLbd.xml";
        string lastUser;
        public Window2()
        {
            InitializeComponent();
            sortXML();
            readXML();

        }
        public Window2(int score, string user) : this()
        {
            lastUser = user;

            InitializeComponent();

            writeXML(score, user);

            readXML();

        }
        //leer XML
        public void readXML()
        {
            //formato de lista de usuarios para recoger del XML
            List<Usuario> listUser = new List<Usuario>();
            XmlSerializer serial = new XmlSerializer(typeof(List<Usuario>));

            using (FileStream fs = new FileStream(pathXML, FileMode.Open, FileAccess.Read))
            {
                listUser = serial.Deserialize(fs) as List<Usuario>;
            }
            rowListSP.ItemsSource = listUser;
        }
        //Escribir en el XML
        public void writeXML(int score, string user)
        {
            System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Load(pathXML);
            System.Xml.Linq.XElement root = new System.Xml.Linq.XElement("Usuario");
            root.Add(new System.Xml.Linq.XElement("nick", user));
            root.Add(new System.Xml.Linq.XElement("score", score));
            doc.Element("ArrayOfUsuario").Add(root);
            doc.Save(pathXML);
        }

        //Boton para volver al Menu
        private void btnToMenu(object sender, RoutedEventArgs e)
        {
            Window1 mainWindw = new Window1(lastUser);
            mainWindw.Show();
            this.Close();
        }
        //Unicamente reordena el XML
        private void sortXML()
        {
            XElement root = XElement.Load(pathXML);
            var orderedtabs = root.Elements("Usuario")
                                  .OrderByDescending(xtab => (int)xtab.Element("score"))
                                  .ToArray();
            root.RemoveAll();
            foreach (XElement tab in orderedtabs)
                root.Add(tab);
            root.Save(pathXML);
        }
    }
}
