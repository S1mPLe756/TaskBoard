namespace CardService.Domain.Events;

public class CardUpdatedEmailSendEvent
{
    public List<string> Emails { get; set; }
    public string Message { get; set; }
}