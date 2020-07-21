using System;
using System.Globalization;

namespace FitnessVictoryCollector.DataModels
{
    public class VictoryPost
    {
        public string Id { get; set; }
        public DateTime PostDate { get; set; }
        public int PostWeekNumber
        {
            get
            {
                return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(PostDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            }
        }
        public int VictoryCommentCount { get; set; }
    }
}
