using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arbeitszeitrechner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        private void Check_TimeFormat(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
            
            if (DateTime.TryParseExact(tb.Text, "HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _))
            {
                tb.Background = Brushes.White;
            }
            else if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Background = Brushes.White;
            }
            else
            {
                tb.Background = Brushes.LightCoral;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }



        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            ResettDefaultTime.ResettDefaultTimeMethod();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CalculateTimes arbeitszeitrechner = new();
            TimeSpan wochenArbeitszeit = TimeSpan.Parse(Properties.Settings.Default.Wochenstunden);
            TimeSpan pausenMinuten;
            TimeSpan weekWorked = TimeSpan.Zero;
            var daysWorked = 5; //Properties.Settings.Default.AnzahlArbeitstage;

            if (int.TryParse(Properties.Settings.Default.Pausenzeit, out int minutenAlsZahl))
            {
                Debug.WriteLine("TryParse");
                pausenMinuten = TimeSpan.FromMinutes(minutenAlsZahl);
            }
            else
            {
                pausenMinuten = TimeSpan.FromMinutes(0);
            }

            if (int.TryParse(Properties.Settings.Default.Wochenstunden, out int stundenAlsZahl))
            {
                Debug.WriteLine("TryParse");
                wochenArbeitszeit = TimeSpan.FromHours(stundenAlsZahl);
            }
            else
            {
                wochenArbeitszeit = TimeSpan.FromHours(0);
            }

            var tagesEingaben = new List<(DayOfWeek Wochentag, TextBox Von, TextBox Bis, Label aktuellerTag)>
            {
                (DayOfWeek.Monday,    TextBox_VonMontag,     TextBox_BisMontag,     MontagLabel),
                (DayOfWeek.Tuesday,   TextBox_VonDienstag,   TextBox_BisDienstag,   DienstagLabel),
                (DayOfWeek.Wednesday, TextBox_VonMittwoch,   TextBox_BisMittwoch,   MittwochLabel),
                (DayOfWeek.Thursday,  TextBox_VonDonnerstag, TextBox_BisDonnerstag, DonnerstagLabel),
                (DayOfWeek.Friday,    TextBox_VonFreitag,    TextBox_BisFreitag,    FreitagLabel),
                (DayOfWeek.Saturday,  TextBox_VonSamstag,    TextBox_BisSamstag,    SamstagLabel),
                (DayOfWeek.Sunday,    TextBox_VonSonntag,    TextBox_BisSonntag,    SonntagLabel)
            };
            try
            {
                foreach (var tag in tagesEingaben)
                {
                TimeOnly vonTime = TimeOnly.Parse(tag.Von.Text);
                TimeOnly bisTime = TimeOnly.Parse(tag.Bis.Text);
                    if (!string.IsNullOrEmpty(tag.Von.Text) && !string.IsNullOrWhiteSpace(tag.Bis.Text))
                    {
                    
                    var dayliWorked = arbeitszeitrechner.CalculateDailyBalance(vonTime, bisTime, pausenMinuten, wochenArbeitszeit);
                    tag.aktuellerTag.Content = dayliWorked.ToString(@"hh\:mm");
                    
                        weekWorked += arbeitszeitrechner.CalculateDailyWorkedHours(vonTime, bisTime, pausenMinuten);
                    
                    }
                    if (tag.Wochentag == DateTime.Today.DayOfWeek)
                    {
                        TextBlock_EmpfohlenesGehen.Text = arbeitszeitrechner.GetShouldGo(vonTime, wochenArbeitszeit, daysWorked, pausenMinuten).ToString();
                    }
                }
            }
            catch { }


            string eingabe = Properties.Settings.Default.Wochenstunden;
            TimeSpan sollArbeitszeit;
            if (double.TryParse(eingabe, out double wochenstundenAlsZahl))
            {
                sollArbeitszeit = TimeSpan.FromHours(wochenstundenAlsZahl);
            }
            else if (TimeSpan.TryParse(eingabe, out sollArbeitszeit))
            {
                //wir in else if (TimeSpan.TryParse(eingabe, out sollArbeitszeit)) behandelt
            }
            else
            {
                sollArbeitszeit = TimeSpan.Zero;
            }
            weekWorked = weekWorked - sollArbeitszeit;
            var converter = new System.Windows.Media.BrushConverter();
            if (weekWorked < TimeSpan.Zero)
            {
                GesamtzeitWocheBorder.Background = (Brush)converter.ConvertFrom("#FFEBEE");
                GesamtWocheUberschrift.Foreground = (Brush)converter.ConvertFrom("#D32F2F");
                GesamtWocheUberschrift.Text = "MINUSSTUNDEN";
                GesamtWocheStundenText.Foreground = (Brush)converter.ConvertFrom("#B71C1C");
            }
            else if (weekWorked > TimeSpan.Zero)
            {
                GesamtzeitWocheBorder.Background = (Brush)converter.ConvertFrom("#E8F5E9");
                GesamtWocheUberschrift.Foreground = (Brush)converter.ConvertFrom("#2E7D32");
                GesamtWocheUberschrift.Text = "ÜBERSTUNDEN";

                GesamtWocheStundenText.Foreground = (Brush)converter.ConvertFrom("#1B5E20");
            }
            else
            {
                GesamtzeitWocheBorder.Background = (Brush)converter.ConvertFrom("#F0F4FF");
                GesamtWocheUberschrift.Foreground = (Brush)converter.ConvertFrom("#4A90E2");
                GesamtWocheUberschrift.Text = "WOCHE";

                GesamtWocheStundenText.Foreground = Brushes.Gray;
            }
            string sign = weekWorked < TimeSpan.Zero ? "-" : "";
            TextBlock_GesamtzeitWoche.Text = sign + weekWorked.Duration().ToString(@"hh\:mm");


        }
    }
}