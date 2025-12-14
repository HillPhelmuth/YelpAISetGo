namespace YelpAIDemo.YelpComponents.AIChat;

public sealed class ChatMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public ChatRole Role { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
}
