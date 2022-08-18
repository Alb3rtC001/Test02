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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test01
{

    public partial class MainWindow : Window
    {
        MediaPlayer iniMusic = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            
            iniMusic.Open(new Uri(Window1.parentDirPath() + "\\Music\\RegresoAlFuturo.mp3"));
            iniMusic.Position = TimeSpan.FromMilliseconds(6000);
            //TODO poner la musica en loop
            iniMusic.Play();
        }

        //Evento boton inicio Game
        private void startGame(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(userTxtBox.Text) || userTxtBox.Text == "User" || userTxtBox.Text.Contains(" "))
            {
                MessageBox.Show("El nombre introducido no es válido, \nno puede estar vacío ni contener espacios en blanco");
            }
            else
            {
                MessageBox.Show("Hola! ayuda a proteger el coche de Doc! El juego consiste en teclear la primera letra de las imagenes " +
                    "que irán apareciendo por pantalla antes de que se choque contra el coche de Doc que se ha parado un momento a por un refigerio," +
                    " en cuanto antes aciertes mas puntos tendrás y a su vez a más velocidad irá. Buena suerte!");
                iniMusic.Pause();
                iniMusic.Stop();
                Window1 gameWind = new Window1(userTxtBox.Text);
                gameWind.Show();
                this.Close();
            }
        }

        //Abrir scoreWindows
        private void goToScoreWindow(object sender, RoutedEventArgs e)
        {
            iniMusic.Pause();
            iniMusic.Stop();
            Window2 scoreWind = new Window2();
            scoreWind.Show();
            this.Close();
        }

        //Cambiar shadow text
        private void UserName_TB(object sender, MouseEventArgs e)
        {
            if (userTxtBox.Text == "User")
            {
                userTxtBox.Text = "";
                userTxtBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        //Cambios en el shadow Text
        private void UserName_TB_leave(object sender, DragEventArgs e)
        {
            if (userTxtBox.Text == "")
            {
                userTxtBox.Text = "User";
                userTxtBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
