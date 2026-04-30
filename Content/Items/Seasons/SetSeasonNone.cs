using ChangedSpecialMod.Common.Systems;

namespace ChangedSpecialMod.Content.Items.Seasons
{
    public class SetSeasonNone : BaseSetSeason
    {
        public override string EventName => "None";
        public override SeasonalEvent Event => SeasonalEvent.None;
    }
}
