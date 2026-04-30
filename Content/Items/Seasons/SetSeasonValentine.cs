using ChangedSpecialMod.Common.Systems;

namespace ChangedSpecialMod.Content.Items.Seasons
{
    public class SetSeasonValentine : BaseSetSeason
    {
        public override string EventName => "Valentine";
        public override SeasonalEvent Event => SeasonalEvent.Valentine;
    }
}
