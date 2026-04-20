namespace EventBus.Messages.Events;
public class BaseIntegrationEvent
{
    public string CorrelationId { get; protected set; }
    public DateTime OccurredOn { get; protected set; }
    public BaseIntegrationEvent()
    {
        CorrelationId = Guid.NewGuid().ToString();
        OccurredOn = DateTime.UtcNow;
    }
    public BaseIntegrationEvent(Guid correlationId, DateTime occurredOn)
    {
        CorrelationId = correlationId.ToString();
        OccurredOn = occurredOn;
    }

}
