using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeitrechner
{
    class ResettDefaultTime
    {
        internal static void ResettDefaultTimeMethod()
        {
            // Die Werte aus den Standard-Feldern holen
            string stdVon = Properties.Settings.Default.StandardVon;
            string stdBis = Properties.Settings.Default.StandardBis;

            // Auf Montag bis Freitag übertragen
            // Montag
            Properties.Settings.Default.MontagVon = stdVon;
            Properties.Settings.Default.MontagBis = stdBis;

            // Dienstag
            Properties.Settings.Default.DienstagVon = stdVon;
            Properties.Settings.Default.DienstagBis = stdBis;

            // Mittwoch
            Properties.Settings.Default.MittwochVon = stdVon;
            Properties.Settings.Default.MittwochBis = stdBis;

            // Donnerstag
            Properties.Settings.Default.DonnerstagVon = stdVon;
            Properties.Settings.Default.DonnerstagBis = stdBis;

            // Freitag
            Properties.Settings.Default.FreitagVon = stdVon;
            Properties.Settings.Default.FreitagBis = stdBis;

            // Samstag
            Properties.Settings.Default.SamstagVon = string.Empty;
            Properties.Settings.Default.SamstagBis = string.Empty;

            // Sonntag
            Properties.Settings.Default.SonntagVon = string.Empty;
            Properties.Settings.Default.SonntagBis = string.Empty;
        }
    }
}
