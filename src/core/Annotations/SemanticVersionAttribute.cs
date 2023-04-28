namespace Synapse;

/// <summary>
/// Represents a <see cref="ValidationAttribute"/> used to validate semantic versions
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class SemanticVersionAttribute
    : RegularExpressionAttribute
{

    /// <inheritdoc/>
    public SemanticVersionAttribute() 
        : base(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$")
    {

    }

}
