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
using System.Windows.Threading;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Drawing;
using WpfAnimatedGif;
using System.Threading;

namespace Test01
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int posXcar = 10;
        int score = 0;
        int carSpeed = 50;
        int speed = 10;
        string firstImg;
        string user;
        private int countdown = 5;
        //private DispatcherTimer countDownTimer;

        DispatcherTimer gameTimer = new DispatcherTimer();
        MediaPlayer explosionSound = new MediaPlayer();
        public Window1()
        {
            InitializeComponent();

            IniGifPath(parentDirPath() + "\\img\\Explosion.gif", explosionGif);
            //iniGifPath(parentDirPath() + "\\img\\portal1.gif", portalGif); //TODO mejorar
            
            scorePoints.Content = score;
            gameCanvas.Focus();
            resetAnimation(portalGif, Canvas.GetLeft(rctImg), 500);
            ImageBrush imgRand = new ImageBrush();
            firstImg = GetrandomFile();
            imgRand.ImageSource = new BitmapImage(new Uri(firstImg));
            rctImg.Fill = imgRand;
            //TODO countDownTimer = new DispatcherTimer();
            //TODO countDownTimer.Interval = new TimeSpan(0, 0, 1);
            //TODO countDownTimer.Tick += counDownAnimation;

            //Thread del movimiento
            gameTimer.Tick += gameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(carSpeed);
            gameTimer.Start();
        }
        public Window1(string userName) : this()
        {
            user = userName;
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            //OPTIONAL dejar el clear para que no se vea el texto al reiniciar
            //inputTB.Clear();

            Canvas.SetLeft(rctImg, Canvas.GetLeft(rctImg) - speed);

           if(Canvas.GetLeft(rctImg) < 5 || Canvas.GetLeft(rctImg) + (rctImg.Width * 2) > Application.Current.MainWindow.Width)
            {
                speed = -speed;
            }

            if (Canvas.GetLeft(rctImg) > (Application.Current.MainWindow.Width - rctImg.Width*2))
            {
                gameTimer.Stop();
                
                Window2 scoreWin = new Window2(score, user);
                scoreWin.Show();
                this.Close();
            }
        }

        //Generar Imagen aleatoria
        public static string GetrandomFile()
        {
            var rand = new Random();
            var files = Directory.GetFiles(parentDirPath() + "\\randomImg", "*.jpg");
            return files[rand.Next(files.Length)];
        }

        //TODO V2.0 animación antes de empezar
        public void counDownAnimation(object sender, EventArgs e)
        {
            if(countdown > 0)
            {
                countdown--;
                //TBCountDown
            }
        }
        //get path del fichero padre
        public static string parentDirPath()
        {
            string dirName = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo fileInfo = new FileInfo(dirName);
            DirectoryInfo parentDir = fileInfo.Directory.Parent.Parent;
            string parentDirName = parentDir.FullName;
            return parentDirName;
        }

        private async void resetAnimation(Image img, double pos, int time = 1000)
        {
            //TODO v2.0 creación y reset de animación
            Canvas.SetLeft(img, pos);
            Canvas.SetTop(img, 339);
            img.Visibility = Visibility.Visible;
            rctImg.Visibility = Visibility.Hidden;
            gameTimer.Stop();
            await Task.Delay(time).ConfigureAwait(true);
            img.Visibility = Visibility.Hidden;
            rctImg.Visibility = Visibility.Visible;
            gameTimer.Start();
        }

        public static void IniGifPath(string path, Image img)
        {
            var portal = new BitmapImage();
            portal.BeginInit();
            portal.UriSource = new Uri(path);
            portal.EndInit();
            ImageBehavior.SetAnimatedSource(img, portal);
        }

        private void inputTB_key(object sender, KeyEventArgs e)
        {
            //get key for each img
            var result = firstImg.Substring(GetrandomFile().Length - 5);
            //get char in textBox
            string key = e.Key.ToString().ToLower();

            if (key.Equals(value: result[0].ToString()))
            {
                //TODO Call reset animation para la v2.0 
                double imgPosX = Canvas.GetLeft(rctImg);
                Canvas.SetLeft(rctImg, posXcar);
                if(carSpeed > 10)
                {
                    carSpeed -= 2;
                    score = score + (int)((1000 - (int)Canvas.GetLeft(rctImg)) / 4) + 1;
                }
                else if (carSpeed >= 2) //cambiar al 1 para mas emoción
                {
                    carSpeed--;
                    score = score + ((1000 - (int)Canvas.GetLeft(rctImg)) / 2) + 1;
                }
                else
                {
                    //incrementar la posicion al llegar al cap de velocidad
                    Canvas.SetLeft(rctImg, posXcar += 10);
                    score = score + (1000 - (int)Canvas.GetLeft(rctImg)) + 1;
                }
                //incremetar el score y la velocidad del coche
                gameTimer.Interval = TimeSpan.FromMilliseconds(carSpeed);
                scorePoints.Content = score;
                explosionSound.Play();
                resetAnimation(explosionGif, imgPosX);
                explosionSound.Stop();
                //cambiar la imagen
                ImageBrush imgRand = new ImageBrush();
                firstImg = GetrandomFile();
                imgRand.ImageSource = new BitmapImage(new Uri(firstImg));
                gameTimer.Stop();
                rctImg.Fill = imgRand;
                inputTB.Clear();
            }
        }
    }
}
