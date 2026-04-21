using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ChangedSpecialMod.Common.Systems
{
    public enum SeasonalEvent
    {
        Invalid = -1,
        None = 0,
        Birthday,
        Valentine,
        Easter,
        Oktoberfest,
        Halloween,
        XMas
    }

    public enum Month
    {
        Invalid = -1,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class SeasonalEventObject
    {
        public SeasonalEvent EventType;
        public DateTime StartDate;
        public DateTime EndDate;

        public SeasonalEventObject(SeasonalEvent eventType, DateTime startDate, DateTime endDate)
        {
            EventType = eventType;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsActive()
        {
            var today = DateTime.Today;
            return today >= StartDate && today <= EndDate;
        }
    }

    public class SeasonSystem : ModSystem
    {
        public static SeasonalEvent season = SeasonalEvent.None;

        // Original
        // - Valentine        1 february      29 february
        // - Easter           1 april         30 april
        // - Oktoberfest      27 september    31 october
        // - Halloween        10 october      1 november
        // - Thanksgiving     1 november      30 november
        // - Xmas             15 december     31 december

        // In this mod
        // - Valentine        10 february     28 february
        // - Easter           1 april         30 april
        // - Oktoberfest      20 september    9 october
        // - Halloween        10 october      1 november
        // - Thanksgiving     10 november     30 november
        // - Xmas             15 december     31 december

        public override void PostSetupContent()
        {
            SetSeason();
            base.PostSetupContent();
        }

        /// <summary>
        /// Set the season. This will happen after loading the mod or by using certain items.
        /// </summary>
        /// <param name="overWriteSeason"></param>
        public static void SetSeason(SeasonalEvent overWriteSeason = SeasonalEvent.Invalid)
        {
            // Overwriting the season
            if (overWriteSeason != SeasonalEvent.Invalid)
            {
                season = overWriteSeason;
                if (season == SeasonalEvent.Halloween)
                {
                    Main.halloween = true;
                    Main.xMas = false;
                }
                else if (season == SeasonalEvent.XMas)
                {
                    Main.halloween = false;
                    Main.xMas = true;
                }
                return;
            }

            // Getting the season normally
            if (Main.halloween)
                season = SeasonalEvent.Halloween;
            else if (Main.xMas)
                season = SeasonalEvent.XMas;
            else
            {
                DateTime today = DateTime.Today;
                List<SeasonalEventObject> seasonalEvents = new List<SeasonalEventObject>();

                // Me and DragonSnow have the same birthday
                seasonalEvents.Add(new SeasonalEventObject(
                    SeasonalEvent.Birthday,
                    new DateTime(today.Year, (int)Month.January, 14),
                    new DateTime(today.Year, (int)Month.January, 14)
                    )
                );

                seasonalEvents.Add(new SeasonalEventObject(
                    SeasonalEvent.Valentine,
                    new DateTime(today.Year, (int)Month.February, 1),
                    new DateTime(today.Year, (int)Month.February, 28)
                    )
                );

                seasonalEvents.Add(new SeasonalEventObject(
                    SeasonalEvent.Easter,
                    new DateTime(today.Year, (int)Month.April, 1),
                    new DateTime(today.Year, (int)Month.April, 30)
                    )
                );

                seasonalEvents.Add(new SeasonalEventObject(
                    SeasonalEvent.Oktoberfest,
                    new DateTime(today.Year, (int)Month.September, 20),
                    new DateTime(today.Year, (int)Month.October, 9)
                    )
                );

                foreach (var seasonalEvent in seasonalEvents)
                {
                    if (seasonalEvent.IsActive())
                    {
                        season = seasonalEvent.EventType;
                        break;
                    }
                }
            }
        }
    }
}
