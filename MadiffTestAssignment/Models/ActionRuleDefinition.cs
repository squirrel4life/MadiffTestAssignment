using System.ComponentModel.DataAnnotations;

namespace MadiffTestAssignment.Models;

public class ActionRuleDefinition
{
    [Required]
    public CardType CardType { get; set; }
    [Required]
    public CardStatus CardStatus { get; set; }
    [Required]
    [MinLength(1)]
    public List<string> Actions { get; set; } = [];
    public bool? RequirePin { get; set; }
}