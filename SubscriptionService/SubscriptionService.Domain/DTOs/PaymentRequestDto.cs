namespace SubscriptionService.Domain.DTOs;

using System.Text.Json.Serialization;

/// <summary>
/// DTO для создания платежа в YooKassa
/// </summary>
public class PaymentRequestDto
{
    [JsonPropertyName("amount")]
    public AmountDto? Amount { get; set; }

    [JsonPropertyName("payment_token")]
    public string? PaymentToken { get; set; }

    [JsonPropertyName("payment_method_id")]
    public string? PaymentMethodId { get; set; }

    [JsonPropertyName("payment_method_data")]
    public PaymentMethodDto? PaymentMethodData { get; set; }

    [JsonPropertyName("confirmation")]
    public ConfirmationDto? Confirmation { get; set; }

    [JsonPropertyName("capture")]
    public bool Capture { get; set; }

    /// <summary>
    /// Создание DTO с значениями по умолчанию
    /// </summary>
    public static PaymentRequestDto CreateDefault()
    {
        return new PaymentRequestDto
        {
            Capture = true
        };
    }
}

/// <summary>
/// Сумма платежа
/// </summary>
public class AmountDto
{
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Создание суммы в рублях
    /// </summary>
    public static AmountDto Rubles(decimal value)
    {
        return new AmountDto
        {
            Value = value,
            Currency = "RUB"
        };
    }
}

/// <summary>
/// Данные для подтверждения платежа
/// </summary>
public class ConfirmationDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("return_url")]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Создание подтверждения с редиректом
    /// </summary>
    public static ConfirmationDto Redirect(string returnUrl)
    {
        return new ConfirmationDto
        {
            Type = "redirect",
            ReturnUrl = returnUrl
        };
    }
}

/// <summary>
/// Данные способа оплаты
/// </summary>
public class PaymentMethodDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Банковская карта
    /// </summary>
    public static PaymentMethodDto BankCard()
    {
        return new PaymentMethodDto
        {
            Type = "bank_card"
        };
    }

    /// <summary>
    /// ЮMoney
    /// </summary>
    public static PaymentMethodDto YooMoney()
    {
        return new PaymentMethodDto
        {
            Type = "yoo_money"
        };
    }

    /// <summary>
    /// СБП
    /// </summary>
    public static PaymentMethodDto Sbp()
    {
        return new PaymentMethodDto
        {
            Type = "sbp"
        };
    }
}