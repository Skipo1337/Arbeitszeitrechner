using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Arbeitszeitrechner
{

    public class CalculateTimes
    {
        private TimeOnly vonTime;
        private TimeOnly bisTime;
        private TimeOnly pause;
        private TimeOnly wochenArbeitsZeit;




        public TimeSpan CalculateDailyBalance(TimeOnly vonTime, TimeOnly bisTime, TimeSpan pause, TimeSpan wochenArbeitsZeit)
        {

            var tagesLeistungMitPause = CalculateDailyWorkedHours(vonTime, bisTime, pause);
            TimeSpan mindestensTagesleistung = wochenArbeitsZeit / 5;
            var tagesleistungSollIst = tagesLeistungMitPause - mindestensTagesleistung;
            return tagesleistungSollIst;
        }

        public TimeSpan CalculateDailyWorkedHours(TimeOnly vonTime, TimeOnly bisTime, TimeSpan pause)
        {
            if (BisTime < VonTime)
                throw new InvalidOperationException("'Bis' muss größer sein als 'Von'.");
            var tagesLeistungOhnePause = bisTime - vonTime;
            var tagesLeistungMitPause = tagesLeistungOhnePause - pause;
            return tagesLeistungMitPause;
        }

        public TimeOnly GetShouldGo(TimeOnly dailyWorked, TimeSpan wochenArbeitsZeit, int daysWorked, TimeSpan pauseMinuten)
        {
            TimeSpan dailyTimeToWork = wochenArbeitsZeit / daysWorked;

            return dailyWorked.Add(dailyTimeToWork + pauseMinuten);
        }

        public TimeOnly VonTime { get => vonTime; set => vonTime = value; }
        public TimeOnly BisTime { get => bisTime; set => bisTime = value; }
        public TimeOnly Pause { get => pause; set => pause = value; }
        public TimeOnly WochenArbeitsZeit { get => wochenArbeitsZeit; set => wochenArbeitsZeit = value; }
    }
}
