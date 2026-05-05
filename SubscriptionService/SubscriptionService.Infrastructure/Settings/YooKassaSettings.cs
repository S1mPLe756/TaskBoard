namespace SubscriptionService.Infrastructure.Settings;

public class YooKassaSettings
{
    public string ShopId { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.yookassa.ru/v3";
}