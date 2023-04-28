namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure the expiration policy for stored objects
/// </summary>
[DataContract]
public record ObjectExpirationPolicy
{

    /// <summary>
    /// Initializes a new <see cref="ObjectExpirationPolicy"/>
    /// </summary>
    public ObjectExpirationPolicy() { }

    /// <summary>
    /// Initializes a new <see cref="ObjectExpirationPolicy"/>
    /// </summary>
    /// <param name="strategy">The policy's expiration strategy. Supported values by default are: 'never', 'absolute', 'sliding'</param>
    /// <param name="expiresAt">The date and time at which the object should expire. Required 'type' is 'absolute'</param>
    /// <param name="expiresAfter">The duration represents the amount of time after which the object should expire. Required if type is 'sliding'</param>
    public ObjectExpirationPolicy(string strategy, DateTimeOffset? expiresAt, TimeSpan? expiresAfter)
    {
        if(string.IsNullOrWhiteSpace(strategy)) throw new ArgumentNullException(nameof(strategy));
        if (strategy.Length < 3 || strategy.Length > 22 || !strategy.IsAlphanumeric('-')) throw new ArgumentException($"The specified value '{strategy}' is not a valid object expiration strategy", nameof(strategy));
        this.Strategy = strategy;
        this.ExpiresAt = expiresAt;
        this.ExpiresAfter = expiresAfter;
    }

    /// <summary>
    /// Gets/sets the policy's expiration strategy. Supported values by default are: 'never', 'absolute', 'sliding'
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 1, Name = "strategy", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("strategy"), YamlMember(Order = 1, Alias = "strategy")]
    public virtual string Strategy { get; set; } = null!;

    /// <summary>
    /// Gets/sets the date and time at which the object should expire. Required 'type' is 'absolute'
    /// </summary>
    [DataMember(Order = 2, Name = "expiresAt"), JsonPropertyOrder(2), JsonPropertyName("expiresAt"), YamlMember(Order = 2, Alias = "expiresAt")]
    public virtual DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    /// Gets/sets the duration represents the amount of time after which the object should expire. Required if type is 'sliding'
    /// </summary>
    [DataMember(Order = 3, Name = "expiresAfter"), JsonPropertyOrder(3), JsonPropertyName("expiresAfter"), YamlMember(Order = 3, Alias = "expiresAfter")]
    public virtual TimeSpan? ExpiresAfter { get; set; }

}