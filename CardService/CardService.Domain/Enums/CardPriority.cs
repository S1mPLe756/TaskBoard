using System.ComponentModel;

namespace CardService.Domain.Enum;

public enum CardPriority
{
    [Description("Low")]
    Low,
    [Description("Normal")]
    Normal,
    [Description("High")]
    High,
}