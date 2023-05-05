namespace Synapse;

/// <summary>
/// Defines extensions for <see cref="WorkflowAgentProcessRule"/>s
/// </summary>
public static class WorkflowAgentProcessRuleExtensions
{

    /// <summary>
    /// Determines whether or not the <see cref="WorkflowAgentProcessRule"/> applies to the specified <see cref="WorkflowInstance"/>
    /// </summary>
    /// <param name="rule">The <see cref="WorkflowAgentProcessRule"/> to check</param>
    /// <param name="workflowVersion">The <see cref="WorkflowVersion"/> that defines the <see cref="WorkflowInstance"/> to check</param>
    /// <param name="instance">The <see cref="WorkflowInstance"/> to check</param>
    /// <returns>A boolean indicating whether or not the <see cref="WorkflowAgentProcessRule"/> applies to the specified <see cref="WorkflowInstance"/></returns>
    public static bool AppliesTo(this WorkflowAgentProcessRule rule, WorkflowVersion workflowVersion, WorkflowInstance instance)
    {
        if (rule == null) throw new ArgumentNullException(nameof(rule));
        if (workflowVersion == null) throw new ArgumentNullException(nameof(workflowVersion));
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (!rule.SupportedLanguages.Contains(workflowVersion.Name)) return false;
        if (rule.Selectors != null && rule.Selectors.Any()) return rule.Selectors.All(s => s.Selects(instance));
        return true;
    }

}
