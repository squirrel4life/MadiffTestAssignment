using MadiffTestAssignment.Models;

namespace MadiffTestAssignment.Services
{
    public static class AllowedActionsGenerator
    {
        public static List<string> GenerateAllowedActions(CardDetails details)
        {
            var actions = new List<string>();

            actions.AddRange(["ACTION3", "ACTION4", "ACTION9"]);
            if (details.CardStatus == CardStatus.Active) actions.Add("ACTION1");
            if (details.CardStatus == CardStatus.Inactive) actions.Add("ACTION2");
            if (details.CardType == CardType.Credit) actions.Add("ACTION5");
            if (!details.CardStatus.In(CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed))
            {
                if (details.IsPinSet)
                {
                    actions.Add("ACTION6");
                    if (details.CardStatus == CardStatus.Blocked) actions.Add("ACTION7");
                }
                else if (details.CardStatus != CardStatus.Blocked) actions.Add("ACTION7");
                actions.Add("ACTION8");
            }
            if (details.CardStatus.In(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active))
            {
                actions.AddRange(["ACTION10", "ACTION12", "ACTION13"]);
                if (details.CardStatus != CardStatus.Ordered) actions.Add("ACTION11");
            }

            return actions;
        }

        private static bool In(this CardStatus cardStatus, params CardStatus[] cardStatuses)
        {
            foreach (var status in cardStatuses)
            {
                if(cardStatus == status) return true;
            }
            return false;
        }
    }
}
