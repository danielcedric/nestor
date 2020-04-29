using Nestor.Tools.Languages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nestor.Tools.Helpers
{
    /// <summary>
    /// Fournit des méthodes permettant de formatter des dates dans différents formats
    /// </summary>
    public static class DateTimeHelper
    {
        // Dictionnaire qui stock pour chaque année les jours fériés Français
        private static readonly Dictionary<int, Dictionary<DateTime, FrenchHolidayType>> frenchHolidays = new Dictionary<int, Dictionary<DateTime, FrenchHolidayType>>();

        /// <summary>
        /// Type de jour férié en France
        /// </summary>
        public enum FrenchHolidayType
        {
            NewYearsDay,
            EasterMonday,
            LaborDay,
            AlliedVictoryDay,
            AscensionDay,
            DayWhitDay,
            NationalHoliday,
            AssumptionDay,
            HalloweenDay,
            ArmisticeDay,
            ChristmasDay
        }

        /// <summary>
        /// Formate un objet DateTime dans un format 01/01/2000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDate(this DateTime date)
        {
            return string.Format("{0:dd/MM/yyyy}", date);
        }

        /// <summary>
        /// Formate un objet DateTime dans un format US 2000-01-01
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatDateUS(this DateTime date)
        {
            return string.Format("{0:yyyy-MM-dd}", date);
        }

        /// <summary>
        /// Formate un objet DateTime dans un format "heure, minute, seconde"
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatHeure(this DateTime heure)
        {
            return string.Format("{0:HH:mm:ss}", heure);
        }

        ///<summary>
        /// Formate une heure dans le format universel
        /// </summary>
        public static string FormatHeureUT(this DateTime heure)
        {
            return heure.ToString("s");
        }

        /// <summary>
        /// Converti un Timestamp en DateTime
        /// </summary>
        /// <param name="timestamp">Timestamp au format unix</param>
        public static DateTime FromUnixTimestamp(int unixTimestamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimestamp).ToLocalTime();
            return dt;
        }

        /// <summary>
        /// Formate une heure selon le format donné
        /// </summary>
        /// <param name="heure"></param>
        /// <param name="formatHeure"></param>
        /// <returns></returns>
        public static string FormatHeure(this DateTime heure, string formatHeure)
        {
            return string.Format(formatHeure, heure);
        }

        /// <summary>
        /// Formate une date au format YYYY-MM-dd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ParseDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        // format de date paramétrable
        public static DateTime ParseDate(string date, string formatDate)
        {
            return DateTime.ParseExact(date, formatDate, CultureInfo.InvariantCulture);
        }

        public static int GetFormatDateIndex(string date, IList<string> LstFormatDate)
        {
            int index = -1;
            DateTime dateRetour = DateTime.MinValue;
            foreach (string formatDate in LstFormatDate)
            {
                if (DateTime.TryParseExact(date, formatDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateRetour))
                {
                    index = LstFormatDate.IndexOf(formatDate);
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heure"></param>
        /// <param name="LstFormatHeure"></param>
        /// <returns></returns>
        public static int GetFormatHeureIndex(string heure, List<string> LstFormatHeure)
        {
            int index = -1;
            DateTime heureRetour = DateTime.MinValue;
            foreach (string formatHeure in LstFormatHeure)
            {
                if (DateTime.TryParseExact(heure, formatHeure, CultureInfo.InvariantCulture, DateTimeStyles.None, out heureRetour))
                {
                    index = LstFormatHeure.IndexOf(formatHeure);
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// format de l'heure fixe HH:mm:ss
        /// </summary>
        /// <param name="dateHeure"></param>
        /// <returns></returns>
        public static DateTime ParseDateHeure(string dateHeure)
        {
            return DateTime.Parse(dateHeure); //ParseExact(dateHeure, "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// format de la date et de l'heure paramétrable
        /// </summary>
        /// <param name="dateHeure"></param>
        /// <param name="formatDateHeure"></param>
        /// <returns></returns>
        public static DateTime ParseDateHeure(string dateHeure, string formatDateHeure)
        {
            return DateTime.ParseExact(dateHeure, formatDateHeure, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Obtient la prochaine date du jour passé en paramètre, la date de départ est aujourd'hui
        /// </summary>
        /// <param name="target">Prochain jour recherché</param>
        /// <returns></returns>
        public static DateTime GetNextDay(DayOfWeek target)
        {
            return GetNextDay(DateTime.Now, target);
        }

        /// <summary>
        /// Obtient la prochaine date du jour passé en paramètre
        /// </summary>
        /// <param name="start">Date de départ</param>
        /// <param name="target">Prochain jour recherché</param>
        /// <returns></returns>
        public static DateTime GetNextDay(DateTime start,  DayOfWeek target)
        {
            return start.AddDays(GetNextDayDelta(start, target));
        }

        /// <summary>
        /// Obtient l'écart entre la date passée en paramètre et le prochain jour ciblé
        /// </summary>
        /// <param name="start">Date de départ</param>
        /// <param name="target">Prochain jour recherché</param>
        /// <returns></returns>
        public static int GetNextDayDelta(DateTime start, DayOfWeek target)
        {
            int delta = target - start.DayOfWeek;
            if (delta <= 0)
                delta = 7 + delta;

            return delta;
        }


        public static string TimeToNow(DateTime dt)
        {
            if (dt < DateTime.Now)
                return "about sometime ago";
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                return String.Format("il y a {0} an{1}", years, years == 1 ? string.Empty : "s");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                return String.Format("il y a {0} mois", months);
            }
            if (span.Days > 0)
                return String.Format("il y a {0} jour{1}", span.Days, span.Days == 1 ? string.Empty : "s");
            if (span.Hours > 0)
                return String.Format("il y a {0} heure{1}", span.Hours, span.Hours == 1 ? string.Empty : "s");
            if (span.Minutes > 0)
                return String.Format("il y a {0} minute{1}", span.Minutes, span.Minutes == 1 ? string.Empty : "s");
            if (span.Seconds > 5)
                return String.Format("il y a {0} secondes", span.Seconds);
            if (span.Seconds == 0)
                return "il y a quelques secondes";
            return string.Empty;
        }

        public static string TimeFromNow(DateTime dt)
        {
            TimeSpan span = dt - DateTime.Now;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                return String.Format("dans {0} an{1}", years, years == 1 ? string.Empty : "s");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                return String.Format("dans {0} mois", months);
            }
            if (span.Days > 0)
                return String.Format("dans {0} jour{1}", span.Days, span.Days == 1 ? string.Empty : "s");
            if (span.Hours > 0)
                return String.Format("dans {0} heure{1}", span.Hours, span.Hours == 1 ? string.Empty : "s");
            if (span.Minutes > 0)
                return String.Format("dans {0} minute{1}", span.Minutes, span.Minutes == 1 ? string.Empty : "s");
            if (span.Seconds > 5)
                return String.Format("dans {0} secondes", span.Seconds);
            if (span.Seconds == 0)
                return "dans quelques secondes";
            return string.Empty;
        }

        /// <summary>
        /// Obtient la date "humanizée"
        /// </summary>
        /// <param name="dt">Date</param>
        /// <param name="todayString">Aujourd'hui</param>
        /// <param name="tomorrowString">Demain</param>
        /// <returns></returns>
        public static string HumanizeDate(DateTime dt, string todayString, string tomorrowString)
        {
            if (dt.Date == DateTime.Today.Date)
                return todayString;
            else if (dt.Date == DateTime.Today.AddDays(1).Date)
                return tomorrowString;
            else
                return dt.ToString("dddd dd MMMM");
        }

        /// <summary>
        /// Obtient la liste des 11 jours fériés en France
        /// </summary>
        /// <param name="year">Année à générer</param>
        /// <returns></returns>
        public static Dictionary<DateTime, FrenchHolidayType> GetFrenchHolidays(int year)
        {
            if (year < 1970)
                throw new InvalidOperationException("L'année ne peut-être inférieure à 1970");

            if (frenchHolidays.ContainsKey(year))
                return frenchHolidays[year];

            // Jour de Pâques
            var easterDate = GetEasterDate(year);

            var easterMonday = easterDate.AddDays(1);
            var ascensionDay = easterDate.AddDays(39);
            var dayWhitMonday = easterDate.AddDays(50);
            
            frenchHolidays.Add(year, new Dictionary<DateTime, FrenchHolidayType>(11)
            {
                {  new DateTime(year,1,1), FrenchHolidayType.NewYearsDay},      // 1er janvier
                { new DateTime(year,5,1), FrenchHolidayType.LaborDay},          // Fête du travail (1er mai)
                { new DateTime(year,5,8), FrenchHolidayType.AlliedVictoryDay},  // Victoire des alliés (8 mai)
                { new DateTime(year,7,14), FrenchHolidayType.NationalHoliday},  // Fête nationale (14 juillet)
                { new DateTime(year,8,15), FrenchHolidayType.AssumptionDay},    // Assomption (15 août)
                { new DateTime(year,11,1), FrenchHolidayType.HalloweenDay},     // Toussaint (1er novembre)
                { new DateTime(year,11,11),FrenchHolidayType.ArmisticeDay },    // Armistice (11 novembre)
                { new DateTime(year,12,25), FrenchHolidayType.ChristmasDay},    // Noël (25 décembre)
                { new DateTime(year,easterMonday.Month,easterMonday.Day), FrenchHolidayType.EasterMonday},    // Lundi de Pâques
                { new DateTime(year,dayWhitMonday.Month,dayWhitMonday.Day),FrenchHolidayType.DayWhitDay },    // Lundi de Pentecôte
                { new DateTime(year,ascensionDay.Month,ascensionDay.Day),FrenchHolidayType.AscensionDay}     // Jeud de l'ascension
            });
            
            frenchHolidays[year].OrderBy(holiday => holiday.Key);

            return frenchHolidays[year];
        }

        /// <summary>
        /// Obtient un booléen qui indique s'il s'agit d'un jour férié
        /// </summary>
        /// <param name="date">date à tester</param>
        /// <returns>Vrai s'il s'agit d'un jour férié</returns>
        public static bool IsPublicHoliday(DateTime date)
        {
            return GetFrenchHolidays(date.Year).Keys.Contains(date);
        }

        /// <summary>
        /// Obtient le type de jour férié
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static FrenchHolidayType? GetFrenchHolidayType(DateTime date)
        {
            var holidays = GetFrenchHolidays(date.Year);

            // Test si le jour est férié ou non
            if (!holidays.Keys.Contains(date))
                return null;

            // Il s'agit d'un jour férié
            return holidays[date];
        }

        /// <summary>
        /// Obtient le jour du dimanche de Pâques
        /// </summary>
        /// <param name="year">Année à calculer</param>
        /// <returns></returns>
        public static DateTime GetEasterDate(int year)
        {
            int month = 3;

            // Determine the Golden number:
            int G = year % 19 + 1;

            // Determine the century number:
            int C = year / 100 + 1;

            // Correct for the years who are not leap years:
            int X = (3 * C) / 4 - 12;

            // Mooncorrection:
            int Y = (8 * C + 5) / 25 - 5;

            // Find sunday:
            int Z = (5 * year) / 4 - X - 10;

            // Determine epact(age of moon on 1 januari of that year(follows a cycle of 19 years):
            int E = (11 * G + 20 + Y - X) % 30;
            if (E == 24) { E++; }
            if ((E == 25) && (G > 11)) { E++; }

            // Get the full moon:
            int N = 44 - E;
            if (N < 21) { N = N + 30; }

            // Up to sunday:
            int P = (N + 7) - ((Z + N) % 7);

            // Easterdate: 
            if (P > 31)
            {
                P = P - 31;
                month = 4;
            }
            return new DateTime(year, month, P);
        }

        /// <summary>
        /// Obtient la validité au format chaine de caractère 
        /// </summary>
        /// <param name="dFrom">date de départ</param>
        /// <param name="dTo">date d'arrivée</param>
        /// <returns></returns>
        public static string HumanizeValidity(DateTime dFrom, DateTime dTo)
        {
            if (dFrom.Year == dTo.Year)
            {
                if (dFrom.Month == dTo.Month)
                    return string.Format(Sentences.HumanizeValiditySameYearSameMonth, dFrom, dTo); // Du 01 au 31 mars 2016
                else
                    return string.Format(Sentences.HumanizeValiditySameYearDifferentMonth, dFrom, dTo); // Du 10 mars au 10 avril 2016
            }
            else
                return string.Format(Sentences.HumanizeValidityDifferentYear, dFrom, dTo); // Du 12 décembre 2015 au 12 janvier 2016
        }

        /// <summary>
        /// Obtient le nombre totales de minutes d'un timespan
        /// </summary>
        /// <param name="time">représente l'interval de temps</param>
        /// <returns></returns>
        public static int ConvertToMinutes(this TimeSpan time)
        {
            return (time.Days * 1440) + (time.Hours * 60) + time.Minutes;
        }

        /// <summary>
        /// Obtient le premier jour de la semaine de la date passée en paramètre
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetMondayOfWeek(DateTime date)
        {
            // lastMonday is always the Monday before nextSunday.
            // When today is a Sunday, lastMonday will be tomorrow.     
            int offset = date.DayOfWeek != DayOfWeek.Sunday ? date.DayOfWeek - DayOfWeek.Monday : 6;
            return date.AddDays(-offset);
        }

        /// <summary>
        /// Obtient le dernier jour de la semaine de la date passée en paramètre
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetSundayDayOfWeek(DateTime date)
        {
            return GetMondayOfWeek(date).AddDays(6);
        }

        /// <summary>
        /// Obtient un booléen qui indique si la date passée en paramètre est dans la plage
        /// </summary>
        /// <param name="dt">Date à tester</param>
        /// <param name="start">Date de début de la période</param>
        /// <param name="end">Date de fin de la période</param>
        /// <returns></returns>
        public static bool In(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }

        /// <summary>
        /// Obtient un booléen qui indique si la date passée en paramètre n'est pas dans la plage
        /// </summary>
        /// <param name="dt">Date à tester</param>
        /// <param name="start">Date de début de la période</param>
        /// <param name="end">Date de fin de la période</param>
        /// <returns></returns>
        public static bool NotIn(this DateTime dt, DateTime start, DateTime end)
        {
            return !In(dt, start, end);
        }
    }
}
