namespace SubscriptionService.Domain.DTOs;

using System.Text.Json.Serialization;
using System;

[JsonSerializable(typeof(PaymentInfoResponseDto))]
public partial class PaymentInfoResponseJsonContext : JsonSerializerContext { }

public class PaymentInfoResponseDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public AmountDto? Amount { get; set; }

    [JsonPropertyName("recipient")]
    public RecipientDto? Recipient { get; set; }

    [JsonPropertyName("payment_method")]
    public PaymentMethodDto? PaymentMethod { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("confirmation")]
    public ConfirmationDto? Confirmation { get; set; }

    [JsonPropertyName("test")]
    public bool Test { get; set; }

    [JsonPropertyName("paid")]
    public bool Paid { get; set; }

    [JsonPropertyName("refundable")]
    public bool Refundable { get; set; }

    [JsonPropertyName("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    // --- вложенные классы ---

    public class AmountDto
    {
        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
    }

    public class RecipientDto
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; } = string.Empty;

        [JsonPropertyName("gateway_id")]
        public string GatewayId { get; set; } = string.Empty;
    }

    public class PaymentMethodDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("saved")]
        public bool Saved { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("card")]
        public CardDto? Card { get; set; }
    }

    public class CardDto
    {
        [JsonPropertyName("first6")]
        public string First6 { get; set; } = string.Empty;

        [JsonPropertyName("last4")]
        public string Last4 { get; set; } = string.Empty;

        [JsonPropertyName("expiry_year")]
        public int ExpiryYear { get; set; }

        [JsonPropertyName("expiry_month")]
        public int ExpiryMonth { get; set; }

        [JsonPropertyName("card_type")]
        public string CardType { get; set; } = string.Empty;

        [JsonPropertyName("card_product")]
        public CardProductDto? CardProduct { get; set; }

        [JsonPropertyName("issuer_country")]
        public string IssuerCountry { get; set; } = string.Empty;

        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; } = string.Empty;
    }

    public class CardProductDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
    }

    public class ConfirmationDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("confirmation_url")]
        public string ConfirmationUrl { get; set; } = string.Empty;
    }
}