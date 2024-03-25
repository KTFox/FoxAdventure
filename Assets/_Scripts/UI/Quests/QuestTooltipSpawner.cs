using UnityEngine;
using RPG.Utility.UI;

namespace RPG.UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        #region TooltipSpawner implements
        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
        }
        #endregion
    }
}
