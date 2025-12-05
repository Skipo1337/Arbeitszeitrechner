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
            


            // Prüfen, ob Eingabe eine gültige Zeit im Format HH:mm ist
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
            CalculateWorkedTime arbeitszeitrechner = new CalculateWorkedTime();
            TimeSpan wochenArbeitszeit = TimeSpan.Parse(Properties.Settings.Default.Wochenstunden);
            TimeSpan pausenMinuten;

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

            var tagesEingaben = new List<(TextBox Von, TextBox Bis, Label aktuellerTag)>
            {
                (TextBox_VonMontag, TextBox_BisMontag, MontagLabel),
                (TextBox_VonDienstag, TextBox_BisDienstag, DienstagLabel),
                (TextBox_VonMittwoch, TextBox_BisMittwoch, MittwochLabel),
                (TextBox_VonDonnerstag, TextBox_BisDonnerstag, DonnerstagLabel),
                (TextBox_VonFreitag, TextBox_BisFreitag, FreitagLabel),
                (TextBox_VonSamstag, TextBox_BisSamstag, SamstagLabel),
                (TextBox_VonSonntag, TextBox_BisSonntag, SonntagLabel)
            };

            foreach (var tag in tagesEingaben)
            {
                if (!string.IsNullOrEmpty(tag.Von.Text) && !string.IsNullOrWhiteSpace(tag.Bis.Text))
                {
                    TimeOnly vonTime = TimeOnly.Parse(tag.Von.Text);
                    TimeOnly bisTime = TimeOnly.Parse(tag.Bis.Text);
                    tag.aktuellerTag.Content = arbeitszeitrechner.GetWorkedTime(vonTime, bisTime, pausenMinuten, wochenArbeitszeit).ToString();
                }
            }

         

        }
    }
}