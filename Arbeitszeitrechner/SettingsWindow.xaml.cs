using System.Windows;
using Arbeitszeitrechner.Properties;

namespace Arbeitszeitrechner
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            CheckBox_Overwrite.IsChecked = Settings.Default.UeberschreibenAktiv;
            Box_Von.Text = Settings.Default.StandardVon;
            Box_Bis.Text = Settings.Default.StandardBis;
            Box_Wochenstunden.Text = Settings.Default.Wochenstunden;
            Box_Arbeitstage.Text = Settings.Default.AnzahlArbeitstage;
            Box_PausenDauer.Text = Settings.Default.Pausenzeit;
            Box_PausenBeginn.Text = Settings.Default.PausenBeginn;
            //Check_Benachrichtigung.IsChecked = Settings.Default.BenachrichtigungAktiv;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            Settings.Default.UeberschreibenAktiv = CheckBox_Overwrite.IsChecked == true;
            Settings.Default.StandardVon = Box_Von.Text;
            Settings.Default.StandardBis = Box_Bis.Text;
            Settings.Default.Wochenstunden = Box_Wochenstunden.Text;
            Settings.Default.AnzahlArbeitstage = Box_Arbeitstage.Text;
            Settings.Default.Pausenzeit = Box_PausenDauer.Text;
            Settings.Default.PausenBeginn = Box_PausenBeginn.Text;
            //Settings.Default.BenachrichtigungAktiv = Check_Benachrichtigung.IsChecked == true;
            if (CheckBox_Overwrite.IsChecked == true)
            {
                ResettDefaultTime.ResettDefaultTimeMethod();
            }
            Settings.Default.Save();
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult != true)
            {
                Properties.Settings.Default.Reload();
            }
        }
    }
}