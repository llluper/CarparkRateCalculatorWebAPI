using System;
namespace carparkRateCalculator.Models
{
    public class TicketMachine
    {
        public DateTime entry { get; set; }
        public DateTime exit { get; set; }
        public RateType entryType { get; set; }
        public RateType exitType { get; set; }
        public Rate rate;
        public enum RateType { Early, Night, Weekend, Standard };

        public TicketMachine(Ticket ticket)
        {
            this.entry = ticket.entry;
            this.exit = ticket.exit;
            this.entryType = RateType.Standard;
            this.exitType = RateType.Standard;
            rate = new Rate();
        }

        public bool IsValid()
        {
            return this.entry.CompareTo(exit) < 0;
        }

        public bool IsWeekend(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }

        public void ProcessTicket()
        {
            if (IsValid())
            {
                CheckEntryCondition();
                if (entryType != RateType.Standard)
                {
                    CheckExitCondition();
                }
                CalculateRate();
            }
        }

        public void CheckEntryCondition()
        {
            if (IsWeekend(entry.DayOfWeek))
            {
                entryType = RateType.Weekend;
            }
            else
            {
                if (entry.TimeOfDay.Hours >= 6 && entry.TimeOfDay.Hours <= 8)
                {
                    entryType = RateType.Early;
                } 
                else if (entry.TimeOfDay.Hours >= 18 && entry.TimeOfDay.Hours <= 23)
                {
                    entryType = RateType.Night;
                }
            }
        }

        public void CheckExitCondition()
        {
            bool sameDay = entry.Date == exit.Date;
            bool nextDay = entry.Date.AddDays(1) == exit.Date;

            if (entryType == RateType.Night || entryType == RateType.Early)
            {
                if ((exit.TimeOfDay.Hours > 15 && exit.TimeOfDay.Hours < 23) || 
                    ((exit.TimeOfDay.Hours == 15 && exit.TimeOfDay.Minutes >= 30) || (exit.TimeOfDay.Hours == 23 && exit.TimeOfDay.Minutes < 31)))
                {
                    if (nextDay)
                    {
                        exitType = RateType.Night;
                    }
                    else if (sameDay)
                    {
                        exitType = RateType.Early;
                    }
                }
                else
                {
                    exitType = RateType.Standard;
                }
            }
            else if (entryType == RateType.Weekend)
            {
                if (IsWeekend(exit.DayOfWeek))
                {
                    if (sameDay || nextDay)
                    {
                        exitType = RateType.Weekend;
                    }
                }
            }
        }

        public void CalculateRate()
        {
            TimeSpan difference = exit - entry;
            bool sameDay = entry.Date == exit.Date;

            if (entryType == exitType && entryType != RateType.Standard)
            {
                switch (entryType)
                {
                    case RateType.Early:
                        {
                            rate.name = "Early";
                            rate.cost = 130.00M;
                            break;
                        }
                    case RateType.Night:
                        {
                            rate.name = "Night";
                            rate.cost = 6.50M;
                            break;
                        }
                    case RateType.Weekend:
                        {
                            rate.name = "Weekend";
                            rate.cost = 10.00M;
                            break;
                        }
                    default:
                        rate.name = "Standard";
                        rate.cost = 0;
                        break;
                }
            }
            else
            {
                rate.name = "Standard";
                if (sameDay)
                {
                    if (entry.TimeOfDay.TotalMinutes > exit.TimeOfDay.TotalMinutes - 60)
                    {
                        rate.cost = 5.00M;
                    }
                    else if (entry.TimeOfDay.TotalMinutes > exit.TimeOfDay.TotalMinutes - 120)
                    {
                        rate.cost = 10.00M;
                    }
                    else if (entry.TimeOfDay.TotalMinutes > exit.TimeOfDay.TotalMinutes - 180)
                    {
                        rate.cost = 15.00M;
                    }
                    else
                    {
                        rate.cost = 20.00M;
                    }
                }
                else
                {
                    rate.cost = (difference.Days + 1) * 20;
                }
            }
        }

        public Rate GetRate()
        {
            return rate;
        }
    }
}
