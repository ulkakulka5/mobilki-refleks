using Microsoft.Maui.Controls.Shapes;
using System.Timers;
using System;

namespace MauiApp3
{

    public partial class MainPage : ContentPage
    {
        int punkty = 0;
        int czas = 60;
        Random los = new Random();
        System.Timers.Timer zegar = new System.Timers.Timer(1000);
        bool gra = false;

        public MainPage()
        {
            InitializeComponent();
            zegar.Elapsed += Tik;
            Loaded += (s, e) => Start();
        }

        void Start()
        {
            punkty = 0;
            czas = 60;
            ScoreLabel.Text = "Wynik: 0";
            TimeLabel.Text = "Czas: 60";
            GameArea.Children.Clear();
            gra = true;
            zegar.Start();
            Kolko();
        }

        void Tik(object s, System.Timers.ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                czas--;
                TimeLabel.Text = "Czas: " + czas;
                if (czas <= 0)
                {
                    gra = false;
                    zegar.Stop();
                    GameArea.Children.Clear();
                    DisplayAlert("Koniec", "Wynik: " + punkty, "OK");
                }
            });
        }

        void Kolko()
        {
            if (!gra) return;

            int rozmiar = los.Next(30, 100);
            double x = los.NextDouble() * (GameArea.Width - rozmiar);
            double y = los.NextDouble() * (GameArea.Height - rozmiar);

            Ellipse kolko = new Ellipse
            {
                WidthRequest = rozmiar,
                HeightRequest = rozmiar,
                Fill = SolidColorBrush.Pink
            };

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) =>
            {
                if (gra)
                {
                    punkty += (100 - rozmiar) / 10 + 1;
                    ScoreLabel.Text = "Wynik: " + punkty;
                    GameArea.Children.Remove(kolko);
                    Kolko();
                }
            };
            kolko.GestureRecognizers.Add(tap);

            AbsoluteLayout.SetLayoutBounds(kolko, new Rect(x, y, rozmiar, rozmiar));
            GameArea.Children.Add(kolko);
        }

        void OnResetClicked(object s, EventArgs e)
        {
            zegar.Stop();
            Start();
        }
    }
}
