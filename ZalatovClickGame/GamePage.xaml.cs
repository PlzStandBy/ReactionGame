using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using System.Diagnostics;


namespace ZalatovClickGame
{
    public abstract class Generator
    {
        public abstract float Next();
        public abstract void SetSeed(string seed);
    }

    public class MidpointGenerator : Generator
    {
        string currentSeed;
        string prevSeed;
        public MidpointGenerator(string seed)
        {
            while (seed.Length != 8)
            {
                if (seed.Length > 8) seed = seed.Substring(0, 8);
                else if (seed.Length < 8) seed = "0" + seed;
            }
            currentSeed = seed.Substring(2, 4);
            prevSeed = "2728";
        }
        public MidpointGenerator()
        {
            currentSeed = Convert.ToInt32(DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            while (currentSeed.Length != 8)
            {
                if (currentSeed.Length > 8) currentSeed = currentSeed.Substring(0, 8);
                else if (currentSeed.Length < 8) currentSeed = "0" + currentSeed;
            }
            prevSeed = "2728";
        }
        public override void SetSeed(string seed)
        {
            while (seed.Length != 8)
            {
                if (seed.Length > 8) seed = seed.Substring(0, 8);
                else if (seed.Length < 8) seed = "0" + seed;
            }
            currentSeed = seed.Substring(2, 4);
        }
        public override float Next()
        {
            string tmp = currentSeed;
            currentSeed = (Convert.ToInt32(prevSeed) * Convert.ToInt32(currentSeed)).ToString();
            if (Convert.ToInt32(currentSeed) == 0) currentSeed = Convert.ToInt32(DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            prevSeed = tmp;
            while (currentSeed.Length < 8) currentSeed = "0" + currentSeed;
            currentSeed = currentSeed.Substring(2, 4);
            float result = Convert.ToInt32(currentSeed);
            while (Math.Floor(result) > 0)
            { result /= 10; }
            return result;

        }
    }


    public partial class GamePage : Page
    {
        int countdown = 3;
        static int seed = 23452345;
        private MidpointGenerator mpgen = new MidpointGenerator(seed.ToString());
        private const int fieldSize = 9;
        private Button[] gameButtons = new Button[fieldSize];
        private Timer timer;
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts;
        List<double> reactionTimes = new List<double>();

        public GamePage()
        {
            InitializeComponent();
            RestartButton.IsEnabled = false;
            gameButtons[0] = GameButton_0_0;
            gameButtons[1] = GameButton_0_1;
            gameButtons[2] = GameButton_0_2;
            gameButtons[3] = GameButton_1_0;
            gameButtons[4] = GameButton_1_1;
            gameButtons[5] = GameButton_1_2;
            gameButtons[6] = GameButton_2_0;
            gameButtons[7] = GameButton_2_1;
            gameButtons[8] = GameButton_2_2;
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnStartEvent;
            timer.Enabled = true;
        }

        private void HideAllGameButtons()
        {
            for(int i = 0; i < fieldSize; i++)
            {
                gameButtons[i].Visibility = Visibility.Hidden;
            }
        }

        private void ButtonClick()
        {
            HideAllGameButtons();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            stopWatch.Reset();
            reactionTimes.Add(ts.TotalMilliseconds);
            averageReactionTime.Text = "СРЕДНЕЕ ВРЕМЯ РЕАКЦИИ: " + Math.Round(reactionTimes.Average(),2).ToString() + " мс";
            lastReactionTime.Text = "ПОСЛЕДНЕЕ ВРЕМЯ РЕАКЦИИ: " + Math.Round(ts.TotalMilliseconds,2).ToString() + " мс";
        }


        private void ShowButtonByIndex(int index)
        {
            if(index >= 1 && index <= 9)
            {
                gameButtons[index - 1].Visibility = Visibility.Visible;
            }
        }

        private int CalculateIndexByValue(float value)
        {
            return Convert.ToInt32(Math.Floor((value * 10f)));
        }

        private void GameButton_0_0_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_0_1_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_0_2_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_1_0_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_1_1_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_1_2_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_2_0_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_2_1_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void GameButton_2_2_Push(object sender, RoutedEventArgs e)
        {
            ButtonClick();
        }

        private void OnStartEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() => RestartButton.IsEnabled = false);
            if (countdown > 0)
            {
                countdown--;
                Dispatcher.Invoke( () => { CountdownTextBlock.Text = countdown.ToString(); } );
            }
            else
            {
                OnTimedEvent(source, e);
                timer.Interval = 3000;
                timer.Elapsed -= OnStartEvent;
                timer.Elapsed += OnTimedEvent;
                Dispatcher.Invoke(() => { BeforeStartTextBlock.Text = ""; CountdownTextBlock.Text = ""; });
            }


        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            stopWatch.Reset();
            Dispatcher.Invoke(() => RestartButton.IsEnabled = true);
            Dispatcher.Invoke(() => ShowButtonByIndex(CalculateIndexByValue(mpgen.Next())));
            stopWatch.Start();
        }

        private void RestartButtonClick(object sender, RoutedEventArgs e)
        {
            countdown = 3;
            Dispatcher.Invoke(() => RestartButton.IsEnabled = false);
            reactionTimes.Clear();
            Dispatcher.Invoke( () =>
            {
                HideAllGameButtons();
                averageReactionTime.Text = "-СРЕДНЕЕ ВРЕМЯ РЕАКЦИИ-";
                lastReactionTime.Text = "-ПОСЛЕДНЕЕ ВРЕМЯ РЕАКЦИИ-";
                BeforeStartTextBlock.Text = "ДО СТАРТА:";
                CountdownTextBlock.Text = countdown.ToString();
            });
            timer.Interval = 1000;
            timer.Elapsed -= OnTimedEvent;
            timer.Elapsed += OnStartEvent;
        }
    }
}
