using UnityEngine;
using RPG.Utility.UI;
using RPG.Quests;

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
            QuestStatus questStatus = GetComponent<QuestRowUI>().QuestStatus;
            tooltip.GetComponent<QuestTooltip>().SetUp(questStatus);
        }
        #endregion
    }
}
