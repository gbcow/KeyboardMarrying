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
using System.Windows.Threading;
namespace KeyboardMarrying
{
    public partial class MainWindow : Window
    {
        static BitmapImage openDoorSource = new BitmapImage(new Uri("Images/phone.png", UriKind.Relative));
        static BitmapImage closeDoorSource = new BitmapImage(new Uri("Images/gray.png", UriKind.Relative));
        static BitmapImage add500 = new BitmapImage(new Uri("Images/animation/add500.png", UriKind.Relative));
        static BitmapImage add2500 = new BitmapImage(new Uri("Images/animation/add2500.png", UriKind.Relative));
        static BitmapImage minus250 = new BitmapImage(new Uri("Images/animation/minus250.png", UriKind.Relative));
        static BitmapImage minusLive = new BitmapImage(new Uri("Images/animation/minusLive.png", UriKind.Relative));
        static double [] imageShowTime =new double [] {2.0,3.0,4.0,5.0,6.0,7.0};
        bool[] isOpened = new bool[6] { false, false, false, false, false, false };
        int [] whatKindOfPeople = new int [6];//0= good , 1 = very good ,2 = bad;
        int score = 0;
        int lives = 3;
        int level = 0;
        int max = 2;
        int min = 1;
        int countTime = 2;
        int state;//0=start 1=selected mode 2= playing 3 = over 
        DispatcherTimer timer;
        Random random = new Random();
        DispatcherTimer [] t;
        DispatcherTimer gameoverCountDown;
        DispatcherTimer updatelevel;
        DispatcherTimer showCount1;
        DispatcherTimer showCount2;
        DispatcherTimer showCount3;
        DispatcherTimer showCount4;
        DispatcherTimer showCount5;
        DispatcherTimer showCount6;
        DispatcherTimer show2500;
                    //music
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        System.Media.SoundPlayer gameoverPlayer = new System.Media.SoundPlayer();

        public MainWindow()
        {
            InitializeComponent();
            //timer
            t = new DispatcherTimer[6];
            gameoverCountDown = new DispatcherTimer();
            timer = new DispatcherTimer();
            t[0] = new DispatcherTimer();
            t[1] = new DispatcherTimer();
            t[2] = new DispatcherTimer();
            t[3] = new DispatcherTimer();
            t[4] = new DispatcherTimer();
            t[5] = new DispatcherTimer();
            t[0].Tick += new EventHandler(Close_door1);
            t[1].Tick += new EventHandler(Close_door2);
            t[2].Tick += new EventHandler(Close_door3);
            t[3].Tick += new EventHandler(Close_door4);
            t[4].Tick += new EventHandler(Close_door5);
            t[5].Tick += new EventHandler(Close_door6);
            timer.Interval = TimeSpan.FromSeconds(1.0);
            timer.Tick += new EventHandler(timer1_Tick);
            gameoverCountDown = new DispatcherTimer();
            gameoverCountDown.Interval = TimeSpan.FromSeconds(8.0);
            gameoverCountDown.Tick += new EventHandler(gameover_Tick);
            updatelevel = new DispatcherTimer();
            updatelevel.Interval = TimeSpan.FromSeconds(5.0);
            updatelevel.Tick += new EventHandler(updatelevel_Tick);

            showCount1 = new DispatcherTimer();
            showCount1.Interval = TimeSpan.FromSeconds(0.5);
            showCount1.Tick += new EventHandler(showCount1_Tick);
            showCount2 = new DispatcherTimer();
            showCount2.Interval = TimeSpan.FromSeconds(0.5);
            showCount2.Tick += new EventHandler(showCount2_Tick);
            showCount3 = new DispatcherTimer();
            showCount3.Interval = TimeSpan.FromSeconds(0.5);
            showCount3.Tick += new EventHandler(showCount3_Tick);
            showCount4 = new DispatcherTimer();
            showCount4.Interval = TimeSpan.FromSeconds(0.5);
            showCount4.Tick += new EventHandler(showCount4_Tick);
            showCount5 = new DispatcherTimer();
            showCount5.Interval = TimeSpan.FromSeconds(0.5);
            showCount5.Tick += new EventHandler(showCount5_Tick);
            showCount6 = new DispatcherTimer();
            showCount6.Interval = TimeSpan.FromSeconds(0.5);
            showCount6.Tick += new EventHandler(showCount6_Tick);
            show2500 = new DispatcherTimer();
            show2500.Interval = TimeSpan.FromSeconds(1.0);
            show2500.Tick += new EventHandler(show2500_Tick);
            //music
            player.Stream = Properties.Resources.iphone;
            gameoverPlayer.Stream = Properties.Resources.bad_end;

            lblTimer.Content = "000";
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            img3.Visibility = Visibility.Hidden;
            img4.Visibility = Visibility.Hidden;
            img5.Visibility = Visibility.Hidden;
            img6.Visibility = Visibility.Hidden;
            img1_2.Visibility = Visibility.Hidden;
            img2_2.Visibility = Visibility.Hidden;
            img3_2.Visibility = Visibility.Hidden;
            img4_2.Visibility = Visibility.Hidden;
            img5_2.Visibility = Visibility.Hidden;
            img6_2.Visibility = Visibility.Hidden;
            gameoverLabel.Visibility = Visibility.Hidden;
            state = 0;
        }


        private void GameStart() {
            state = 2;
            updatelevel.Start();
            PlayAudioInStart();
            visibleKeys();
            timer.Start();
        }
        private void Reset() {

            score = 0;
            lives = 3;
            Score.Content = score.ToString();
            Lives.Content = lives.ToString();

        }
        private void GameOver()
        {
            player.Stop();
            gameoverLabel.Visibility = Visibility.Hidden;
            gameoverLabel.Visibility = Visibility.Visible;
            hiddenKeys();
            timer.Stop();
            updatelevel.Stop();
            GameoverPlayAudioInStart();
            gameoverCountDown.Start();
            state = 3;
        }
        private void gameover_Tick(object sender, EventArgs e)
        {
            gameoverPlayer.Stop();
            Reset();
            startBtn.Visibility = Visibility.Visible;
            aboutBtn.Visibility = Visibility.Visible;
            exitBtn.Visibility = Visibility.Visible;
            instructiontBtn.Visibility = Visibility.Visible;
            TitleImage.Visibility = Visibility.Visible;
            gameoverLabel.Visibility = Visibility.Hidden;
            gameoverCountDown.Stop();
            state = 0;
            level = 0;
            Level.Content = level.ToString();
        }

        private void updatelevel_Tick(object sender, EventArgs e)
        {
            if (level == 1)
            {
                min = 2;
                max = 4;
                level = 2;
                Level.Content = level.ToString();
            }
            else if (level == 2)
            {
                min = 3;
                max = 6;
                level = 3;
                Level.Content = level.ToString();
            }
            updatelevel.Stop();
            if (level < 3) {
                updatelevel.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int door;
            int willOpen;
            int times = random.Next(min,max);
            int kind;
            String ss;
            BitmapImage img;
            for (; times > 0; times--)
            {
                willOpen = random.Next(2);
                countTime = random.Next(2, 4);
                door =  random.Next(6);
                if (willOpen == 0 && !isOpened[door])
                {
                    kind = random.Next(0, 7);
                    if (kind == 0 || kind == 1)
                        whatKindOfPeople[door] = 2;
                    else if (kind < 6)
                        whatKindOfPeople[door] = 0;
                    else
                        whatKindOfPeople[door] = 1;
                    if (whatKindOfPeople[door] == 0)
                    {
                        ss = "images/beauty/beauty (" + random.Next(37, 74).ToString() + ").jpg";
                        img = new BitmapImage(new Uri(ss, UriKind.Relative));
                    }
                    else if (whatKindOfPeople[door] == 1)
                    {
                        ss = "images/sexy/sexy (" + random.Next(1, 24).ToString() + ").jpg";
                        img = new BitmapImage(new Uri(ss, UriKind.Relative));
                    }
                    else
                    {
                        ss = "images/handsome/handsome (" + random.Next(1, 13).ToString() + ").jpg";
                        img = new BitmapImage(new Uri(ss, UriKind.Relative));
                    }
                    switch (door)
                    {
                        case 0:
                            img1.Source = img;
                            isOpened[door] = true;
                            t[door].Interval = TimeSpan.FromSeconds(countTime);
                            t[door].Start();
                            break;
                        case 1:
                            img2.Source = img;
                            isOpened[door] = true;
                            t[door].Interval = TimeSpan.FromSeconds(countTime);
                            t[door].Start();
                            break;
                        case 2:
                            img3.Source = img;
                            isOpened[door] = true;
                            t[door].Interval = TimeSpan.FromSeconds(countTime);
                            t[door].Start();
                            break;
                        case 3:
                            img4.Source = img;
                            isOpened[door] = true;
                            t[door].Interval = TimeSpan.FromSeconds(countTime);
                            t[door].Start();
                            break;
                        case 4:
                            img5.Source = img;
                            isOpened[door] = true;
                            t[door].Interval = TimeSpan.FromSeconds(countTime);
                            t[door].Start();
                            break;
                        case 5:
                            img6.Source = img;
                            isOpened[door] = true;
                            t[door].Interval = TimeSpan.FromSeconds(countTime);
                            t[door].Start();
                            break;
                    }
                }
            }
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (state == 0) {
                switch (e.Key) { 
                    case Key.A:
                        CreateAbout();
                        break;
                    case Key.S:
                        chooseMode();
                        break;
                    case Key.I:
                        showInstruction();
                        break;
                    case Key.E:
                        this.Close();
                        break;

                }
            }
            else if (state == 1) {
                switch (e.Key) { 
                    case Key.E:
                        chooseEasy();
                        break;
                    case Key.M:
                        chooseMedium();
                        break;
                    case Key.H:
                        chooseHard();
                        break;
                }
            }
            else if (state == 2)
            {
                switch (e.Key)
                {
                    //first 
                    case Key.W:
                        Wkey2.Visibility = Visibility.Visible;
                        if (isOpened[0])
                        {
                            img1.Source = closeDoorSource;
                            isOpened[0] = false;
                            t[0].Stop();
                            if (whatKindOfPeople[0] == 0)
                            {
                                score += 500;
                                Count1.Source = add500;
                                showCount1.Start();
                            }
                            else if (whatKindOfPeople[0] == 2)
                            {
                                Count1.Source = minusLive;
                                showCount1.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    case Key.S:
                        Skey2.Visibility = Visibility.Visible;
                        if (isOpened[0])
                        {
                            img1.Source = closeDoorSource;
                            isOpened[0] = false;
                            t[0].Stop();
                            if (whatKindOfPeople[0] == 0)
                            {
                                score -= 250;
                            }
                            Score.Content = score.ToString();
                        }
                        break;
                    case Key.X:
                        Xkey2.Visibility = Visibility.Visible;
                        if (isOpened[0])
                        {
                            img1.Source = closeDoorSource;
                            isOpened[0] = false;
                            t[0].Stop();
                            Score.Content.ToString();
                            if (whatKindOfPeople[0] == 0)
                            {
                                score -= 250;
                            }
                            else if (whatKindOfPeople[0] == 1)
                            {
                                Bonus.Source = add2500;
                                show2500.Start();
                                score += 2500;
                            }
                            else
                            {
                                Count1.Source = minusLive;
                                showCount1.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    //second
                    case Key.E:
                        Ekey2.Visibility = Visibility.Visible;
                        if (isOpened[1])
                        {
                            img2.Source = closeDoorSource;
                            isOpened[1] = false;
                            t[1].Stop();
                            if (whatKindOfPeople[1] == 0)
                            {
                                Count2.Source = add500;
                                showCount2.Start();
                                score += 500;
                            }
                            else if (whatKindOfPeople[1] == 2)
                            {
                                Count2.Source = minusLive;
                                showCount2.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    case Key.D:
                        Dkey2.Visibility = Visibility.Visible;
                        if (isOpened[1])
                        {
                            img2.Source = closeDoorSource;
                            isOpened[1] = false;
                            t[1].Stop();
                            if (whatKindOfPeople[1] == 0)
                            {
                                score -= 250;
                            }
                            Score.Content = score.ToString();
                        }
                        break;
                    case Key.C:
                        Ckey2.Visibility = Visibility.Visible;
                        if (isOpened[1])
                        {
                            img2.Source = closeDoorSource;
                            isOpened[1] = false;
                            t[1].Stop();
                            Score.Content.ToString();
                            if (whatKindOfPeople[1] == 0)
                            {
                                score -= 250;
                            }
                            else if (whatKindOfPeople[1] == 1)
                            {
                                Bonus.Source = add2500;
                                show2500.Start();
                                score += 2500;
                            }
                            else
                            {
                                Count2.Source = minusLive;
                                showCount2.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;

                    //third
                    case Key.R:
                        Rkey2.Visibility = Visibility.Visible;
                        if (isOpened[2])
                        {
                            img3.Source = closeDoorSource;
                            isOpened[2] = false;
                            t[2].Stop();
                            if (whatKindOfPeople[2] == 0)
                            {
                                Count3.Source = add500;
                                showCount3.Start();
                                score += 500;
                            }
                            else if (whatKindOfPeople[2] == 2)
                            {
                                Count3.Source = minusLive;
                                showCount3.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    case Key.F:
                        Fkey2.Visibility = Visibility.Visible;
                        if (isOpened[2])
                        {
                            img3.Source = closeDoorSource;
                            isOpened[2] = false;
                            t[2].Stop();
                            if (whatKindOfPeople[2] == 0)
                            {
                                score -= 250;
                            }
                            Score.Content = score.ToString();
                        }
                        break;
                    case Key.V:
                        Vkey2.Visibility = Visibility.Visible;
                        if (isOpened[2])
                        {
                            img3.Source = closeDoorSource;
                            isOpened[2] = false;
                            t[2].Stop();
                            Score.Content.ToString();
                            if (whatKindOfPeople[2] == 0)
                            {
                                score -= 250;
                            }
                            else if (whatKindOfPeople[2] == 1)
                            {
                                Bonus.Source = add2500;
                                show2500.Start();
                                score += 2500;
                            }
                            else
                            {
                                Count3.Source = minusLive;
                                showCount3.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    //forth
                    case Key.U:
                        Ukey2.Visibility = Visibility.Visible;
                        if (isOpened[3])
                        {
                            img4.Source = closeDoorSource;
                            isOpened[3] = false;
                            t[3].Stop();
                            if (whatKindOfPeople[3] == 0)
                            {
                                score += 500;
                                Count4.Source = add500;
                                showCount4.Start();
                            }
                            else if (whatKindOfPeople[3] == 2)
                            {
                                Count4.Source = minusLive;
                                showCount4.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    case Key.J:
                        Jkey2.Visibility = Visibility.Visible;
                        if (isOpened[3])
                        {
                            img4.Source = closeDoorSource;
                            isOpened[3] = false;
                            t[3].Stop();
                            if (whatKindOfPeople[3] == 0)
                            {
                                score -= 250;
                            }
                            Score.Content = score.ToString();
                        }
                        break;
                    case Key.M:
                        Mkey2.Visibility = Visibility.Visible;
                        if (isOpened[3])
                        {
                            img4.Source = closeDoorSource;
                            isOpened[3] = false;
                            t[3].Stop();
                            Score.Content.ToString();
                            if (whatKindOfPeople[3] == 0)
                            {
                                score -= 250;
                            }
                            else if (whatKindOfPeople[3] == 1)
                            {
                                Bonus.Source = add2500;
                                show2500.Start();
                                score += 2500;
                            }
                            else
                            {
                                Count4.Source = minusLive;
                                showCount4.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    //fifth
                    case Key.I:
                        Ikey2.Visibility = Visibility.Visible;
                        if (isOpened[4])
                        {
                            img5.Source = closeDoorSource;
                            isOpened[4] = false;
                            t[4].Stop();
                            if (whatKindOfPeople[4] == 0)
                            {
                                score += 500;
                                Count5.Source = add500;
                                showCount5.Start();
                            }
                            else if (whatKindOfPeople[4] == 2)
                            {
                                Count5.Source = minusLive;
                                showCount5.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    case Key.K:
                        Kkey2.Visibility = Visibility.Visible;
                        if (isOpened[4])
                        {
                            img5.Source = closeDoorSource;
                            isOpened[4] = false;
                            t[4].Stop();
                            if (whatKindOfPeople[4] == 0)
                            {
                                score -= 250;
                            }
                            Score.Content = score.ToString();
                        }
                        break;
                    case Key.OemComma:
                        commakey2.Visibility = Visibility.Visible;
                        if (isOpened[4])
                        {
                            img5.Source = closeDoorSource;
                            isOpened[4] = false;
                            t[4].Stop();
                            Score.Content.ToString();
                            if (whatKindOfPeople[4] == 0)
                            {
                                score -= 250;
                            }
                            else if (whatKindOfPeople[4] == 1)
                            {
                                Bonus.Source = add2500;
                                show2500.Start();
                                score += 2500;
                            }
                            else if (whatKindOfPeople[4] == 2)
                            {
                                Count5.Source = minusLive;
                                showCount5.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    //sixth
                    case Key.O:
                        Okey2.Visibility = Visibility.Visible;
                        if (isOpened[5])
                        {
                            img6.Source = closeDoorSource;
                            isOpened[5] = false;
                            t[5].Stop();
                            if (whatKindOfPeople[5] == 0)
                            {
                                score += 500;
                                Count6.Source = add500;
                                showCount6.Start();
                            }
                            else if (whatKindOfPeople[5] == 2)
                            {
                                Count6.Source = minusLive;
                                showCount6.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                    case Key.L:
                        Lkey2.Visibility = Visibility.Visible;
                        if (isOpened[5])
                        {
                            img6.Source = closeDoorSource;
                            isOpened[5] = false;
                            t[5].Stop();
                            if (whatKindOfPeople[5] == 0)
                            {
                                score -= 250;
                            }
                            Score.Content = score.ToString();
                        }
                        break;
                    case Key.OemPeriod:
                        periodkey2.Visibility = Visibility.Visible;
                        if (isOpened[5])
                        {
                            img6.Source = closeDoorSource;
                            isOpened[5] = false;
                            t[5].Stop();
                            Score.Content.ToString();
                            if (whatKindOfPeople[5] == 0)
                            {
                                score -= 250;
                            }
                            else if (whatKindOfPeople[5] == 1)
                            {
                                Bonus.Source = add2500;
                                show2500.Start();
                                score += 2500;
                            }
                            else
                            {
                                Count6.Source = minusLive;
                                showCount6.Start();
                                lives -= 1;
                            }
                            Score.Content = score.ToString();
                            Lives.Content = lives.ToString();
                        }
                        break;
                }
                if (lives == 0)
                    GameOver();
                e.Handled = true;
            }
            else if (state == 3) { 
                
            }
            
        }
        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (state == 2) {
                switch (e.Key)
                {
                    //first 
                    case Key.W:
                        Wkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.S:
                        Skey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.X:
                        Xkey2.Visibility = Visibility.Hidden;
                        break;
                    //second
                    case Key.E:
                        Ekey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.D:
                        Dkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.C:
                        Ckey2.Visibility = Visibility.Hidden;
                        break;

                    //third
                    case Key.R:
                        Rkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.F:
                        Fkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.V:
                        Vkey2.Visibility = Visibility.Hidden;
                        break;
                    //forth
                    case Key.U:
                        Ukey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.J:
                        Jkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.M:
                        Mkey2.Visibility = Visibility.Hidden;
                        break;
                    //fifth
                    case Key.I:
                        Ikey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.K:
                        Kkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.OemComma:
                        commakey2.Visibility = Visibility.Hidden;
                        break;
                    //sixth
                    case Key.O:
                        Okey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.L:
                        Lkey2.Visibility = Visibility.Hidden;
                        break;
                    case Key.OemPeriod:
                        periodkey2.Visibility = Visibility.Hidden;
                        break;
                }
            }

        }

        private void PlayAudioInStart(){
            player.Load();
            player.PlayLooping();
        }
        private void GameoverPlayAudioInStart()
        {
            gameoverPlayer.Load();
            gameoverPlayer.PlayLooping();
        }
        private void Close_door1(object sender, EventArgs e) {
            img1.Source = closeDoorSource;
            isOpened[0] = false;
            t[0].Stop();
            if(whatKindOfPeople[0] == 2 ){
                Count1.Source = minusLive;
                showCount1.Start();
                lives--;
                Lives.Content = lives.ToString();
            }
            else if (whatKindOfPeople[0] == 0) {
                score -= 250;
                Score.Content = score.ToString();
            }
            t[0].Stop();
            if (lives == 0)
                GameOver();
        }
        private void Close_door2(object sender, EventArgs e)
        {
            img2.Source = closeDoorSource;
            isOpened[1] = false;
            t[1].Stop();
            if (whatKindOfPeople[1] == 2)
            {
                Count2.Source = minusLive;
                showCount2.Start();
                lives--;
                Lives.Content = lives.ToString();
            }
            else if (whatKindOfPeople[1] == 0)
            {
                score -= 250;
                Score.Content = score.ToString();
            }
            t[1].Stop();
            if (lives == 0)
                GameOver();

        }
        private void Close_door3(object sender, EventArgs e)
        {
            img3.Source = closeDoorSource;
            isOpened[2] = false;
            t[2].Stop();
            if (whatKindOfPeople[2] == 2)
            {
                Count3.Source = minusLive;
                showCount3.Start();
                lives--;
                Lives.Content = lives.ToString();
            }
            else if (whatKindOfPeople[2] == 0)
            {
                score -= 250;
                Score.Content = score.ToString();
            }
            t[2].Stop();
            if (lives == 0)
                GameOver();
        }
        private void Close_door4(object sender, EventArgs e)
        {
            img4.Source = closeDoorSource;
            isOpened[3] = false;
            t[3].Stop();
            if (whatKindOfPeople[3] == 2)
            {
                Count4.Source = minusLive;
                showCount4.Start();
                lives--;
                Lives.Content = lives.ToString();
            }
            else if (whatKindOfPeople[3] == 0)
            {
                score -= 250;
                Score.Content = score.ToString();
            }
            t[3].Stop();
            if (lives == 0)
                GameOver();
        }
        private void Close_door5(object sender, EventArgs e)
        {
            img5.Source = closeDoorSource;
            isOpened[4] = false;
            t[4].Stop();
            if (whatKindOfPeople[4] == 2)
            {
                Count5.Source = minusLive;
                showCount5.Start();
                lives--;
                Lives.Content = lives.ToString();
            }
            else if (whatKindOfPeople[4] == 0)
            {
                score -= 250;
                Score.Content = score.ToString();
            }
            t[4].Stop();
            if (lives == 0)
                GameOver();
        }
        private void Close_door6(object sender, EventArgs e)
        {
            img6.Source = closeDoorSource;
            isOpened[5] = false;
            t[5].Stop();
            if (whatKindOfPeople[5] == 2)
            {
                Count6.Source = minusLive;
                showCount6.Start();
                lives--;
                Lives.Content = lives.ToString();
            }
            else if (whatKindOfPeople[5] == 0)
            {
                score -= 250;
                Score.Content = score.ToString();
            }
            t[5].Stop();
            if (lives == 0)
                GameOver();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chooseMode();
        }
        private void chooseMode() {
            state = 1;
            startBtn.Visibility = Visibility.Hidden;
            aboutBtn.Visibility = Visibility.Hidden;
            exitBtn.Visibility = Visibility.Hidden;
            instructiontBtn.Visibility = Visibility.Hidden;
            TitleImage.Visibility = Visibility.Hidden;
            easyBtn.Visibility = Visibility.Visible;
            mediumBtn.Visibility = Visibility.Visible;
            hardBtn.Visibility = Visibility.Visible;
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            CreateAbout();
        }

        private void CreateAbout() {
            AboutBox1 aboutbox1 = new AboutBox1();
            aboutbox1.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void hard_Click(object sender, RoutedEventArgs e)
        {
            chooseHard();
        }
        private void chooseHard() {
            min = 3;
            max = 6;
            level = 3;
            Level.Content = level.ToString();
            easyBtn.Visibility = Visibility.Hidden;
            mediumBtn.Visibility = Visibility.Hidden;
            hardBtn.Visibility = Visibility.Hidden;
            GameStart();
        }

        private void medium_Btn(object sender, RoutedEventArgs e)
        {
            chooseMedium();
        }
        private void chooseMedium() {
            min = 2;
            max = 4;
            level = 2;
            Level.Content = level.ToString();
            easyBtn.Visibility = Visibility.Hidden;
            mediumBtn.Visibility = Visibility.Hidden;
            hardBtn.Visibility = Visibility.Hidden;
            GameStart();
        }
        private void easy_Click(object sender, RoutedEventArgs e)
        {
            chooseEasy();
        }
        private void chooseEasy() {
            min = 1;
            max = 2;
            level = 1;
            Level.Content = level.ToString();
            easyBtn.Visibility = Visibility.Hidden;
            mediumBtn.Visibility = Visibility.Hidden;
            hardBtn.Visibility = Visibility.Hidden;
            GameStart();
        }

        private void hiddenKeys() {
            periodkey.Visibility = Visibility.Hidden;
            Wkey2.Visibility = Visibility.Hidden;
            Skey2.Visibility = Visibility.Hidden;
            Xkey2.Visibility = Visibility.Hidden;
            Ekey2.Visibility = Visibility.Hidden;
            Dkey2.Visibility = Visibility.Hidden;
            Ckey2.Visibility = Visibility.Hidden;
            Rkey2.Visibility = Visibility.Hidden;
            Fkey2.Visibility = Visibility.Hidden;
            Vkey2.Visibility = Visibility.Hidden;
            Ukey2.Visibility = Visibility.Hidden;
            Jkey2.Visibility = Visibility.Hidden;
            Mkey2.Visibility = Visibility.Hidden;
            Ikey2.Visibility = Visibility.Hidden;
            Kkey2.Visibility = Visibility.Hidden;
            commakey2.Visibility = Visibility.Hidden;
            Okey2.Visibility = Visibility.Hidden;
            Lkey2.Visibility = Visibility.Hidden;
            Wkey.Visibility = Visibility.Hidden;
            Skey.Visibility = Visibility.Hidden;
            Xkey.Visibility = Visibility.Hidden;
            Ekey.Visibility = Visibility.Hidden;
            Dkey.Visibility = Visibility.Hidden;
            Ckey.Visibility = Visibility.Hidden;
            Rkey.Visibility = Visibility.Hidden;
            Fkey.Visibility = Visibility.Hidden;
            Vkey.Visibility = Visibility.Hidden;
            Ukey.Visibility = Visibility.Hidden;
            Jkey.Visibility = Visibility.Hidden;
            Mkey.Visibility = Visibility.Hidden;
            Ikey.Visibility = Visibility.Hidden;
            Kkey.Visibility = Visibility.Hidden;
            commakey.Visibility = Visibility.Hidden;
            Okey.Visibility = Visibility.Hidden;
            Lkey.Visibility = Visibility.Hidden;
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            img3.Visibility = Visibility.Hidden;
            img4.Visibility = Visibility.Hidden;
            img5.Visibility = Visibility.Hidden;
            img6.Visibility = Visibility.Hidden;
            img1_2.Visibility = Visibility.Hidden;
            img2_2.Visibility = Visibility.Hidden;
            img3_2.Visibility = Visibility.Hidden;
            img4_2.Visibility = Visibility.Hidden;
            img5_2.Visibility = Visibility.Hidden;
            img6_2.Visibility = Visibility.Hidden;
        }
        private void visibleKeys() {
            img1.Visibility = Visibility.Visible;
            img2.Visibility = Visibility.Visible;
            img3.Visibility = Visibility.Visible;
            img4.Visibility = Visibility.Visible;
            img5.Visibility = Visibility.Visible;
            img6.Visibility = Visibility.Visible;
            img1_2.Visibility = Visibility.Visible;
            img2_2.Visibility = Visibility.Visible;
            img3_2.Visibility = Visibility.Visible;
            img4_2.Visibility = Visibility.Visible;
            img5_2.Visibility = Visibility.Visible;
            img6_2.Visibility = Visibility.Visible;
            periodkey.Visibility = Visibility.Visible;
            Wkey.Visibility = Visibility.Visible;
            Skey.Visibility = Visibility.Visible;
            Xkey.Visibility = Visibility.Visible;
            Ekey.Visibility = Visibility.Visible;
            Dkey.Visibility = Visibility.Visible;
            Ckey.Visibility = Visibility.Visible;
            Rkey.Visibility = Visibility.Visible;
            Fkey.Visibility = Visibility.Visible;
            Vkey.Visibility = Visibility.Visible;
            Ukey.Visibility = Visibility.Visible;
            Jkey.Visibility = Visibility.Visible;
            Mkey.Visibility = Visibility.Visible;
            Ikey.Visibility = Visibility.Visible;
            Kkey.Visibility = Visibility.Visible;
            commakey.Visibility = Visibility.Visible;
            Okey.Visibility = Visibility.Visible;
            Lkey.Visibility = Visibility.Visible;
        }

        private void instructiontBtn_Click(object sender, RoutedEventArgs e)
        {
            showInstruction();
        }
        private void showInstruction() {
            InstructionWindow w = new InstructionWindow();
            w.ShowDialog();
        }

        private void showCount1_Tick(object sender, EventArgs e)
        {
            Count1.Source = null;
            showCount1.Stop();
        }
        private void showCount2_Tick(object sender, EventArgs e)
        {
            Count2.Source = null;
            showCount2.Stop();
        }
        private void showCount3_Tick(object sender, EventArgs e)
        {
            Count3.Source = null;
            showCount3.Stop();
        }
        private void showCount4_Tick(object sender, EventArgs e)
        {
            Count4.Source = null;
            showCount4.Stop();
        }
        private void showCount5_Tick(object sender, EventArgs e)
        {
            Count5.Source = null;
            showCount5.Stop();
        }
        private void showCount6_Tick(object sender, EventArgs e)
        {
            Count6.Source = null;
            showCount6.Stop();
        }
        private void show2500_Tick(object sender, EventArgs e)
        {
            Bonus.Source = null;
            show2500.Stop();
        }
        
    }
}
