using MadiffTestAssignment.Config;
using MadiffTestAssignment.Models;
using Microsoft.Extensions.Options;

namespace MadiffTestAssignment.Services;

public class CardActionRegistry : ICardActionRegistry
{
    private Dictionary<(CardType, CardStatus), List<ActionRule>> _rules = [];

    public CardActionRegistry(IOptionsMonitor<ActionRulesConfig> optionsMonitor)
    {
        LoadRules(optionsMonitor.CurrentValue);

        optionsMonitor.OnChange(config => LoadRules(config));
    }

    public List<string> GetActions(CardDetails card) => !_rules.TryGetValue((card.CardType, card.CardStatus), out var rules)
            ? (List<string>)([])
            : rules
            .Where(r => r.Condition(card))
            .Select(r => r.ActionName)
            .Distinct()
            .ToList();

    private void LoadRules(ActionRulesConfig config)
    {
        var newRules = new Dictionary<(CardType, CardStatus), List<ActionRule>>();

        foreach (var rule in config.Rules)
        {
            var key = (rule.CardType, rule.CardStatus);
            if (!newRules.ContainsKey(key))
                newRules[key] = [];

            foreach (var action in rule.Actions)
            {
                newRules[key].Add(new ActionRule(
                    action,
                    rule.RequirePin switch
                    {
                        true => c => c.IsPinSet,
                        false => c => !c.IsPinSet,
                        null => _ => true
                    }));
            }
        }

        _rules = newRules;
    }
}
