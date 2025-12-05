using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbeitszeitrechner
{

    public class CalculateWorkedTime
    {
        private TimeOnly vonTime;
        private TimeOnly bisTime;
        private TimeOnly pause;
        private TimeOnly wochenArbeitsZeit;



        public TimeSpan GetWorkedTime(TimeOnly vonTime, TimeOnly bisTime, TimeSpan pause, TimeSpan wochenArbeitsZeit)
        {
            
            if (BisTime < VonTime)
                throw new InvalidOperationException("'Bis' muss größer sein als 'Von'.");
            var tagesLeistungOhnePause = bisTime - vonTime;
            Debug.WriteLine("tagesLeistungOhnePause: " + tagesLeistungOhnePause);
            var tagesLeistungMitPause = tagesLeistungOhnePause - pause;
            Debug.WriteLine("tagesLeistungMitPause: " + tagesLeistungMitPause);
            TimeSpan mindestensTagesleistung = wochenArbeitsZeit / 5;
            Debug.WriteLine("mindestensTagesleistung: " + mindestensTagesleistung);
            var tagesleistungSollIst = tagesLeistungMitPause - mindestensTagesleistung;
            Debug.WriteLine("tagesleistungSollIst: " + tagesleistungSollIst);
            return tagesleistungSollIst;

        }


        public TimeOnly VonTime { get => vonTime; set => vonTime = value; }
        public TimeOnly BisTime { get => bisTime; set => bisTime = value; }
        public TimeOnly Pause { get => pause; set => pause = value; }
        public TimeOnly WochenArbeitsZeit { get => wochenArbeitsZeit; set => wochenArbeitsZeit = value; }
    }
}
