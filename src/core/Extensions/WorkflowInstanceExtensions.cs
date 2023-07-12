namespace Synapse;

/// <summary>
/// Defines extensions for <see cref="WorkflowInstance"/>s
/// </summary>
public static class WorkflowInstanceExtensions
{

    /// <summary>
    /// Determines whether or not the <see cref="WorkflowInstance"/> is managed by the specified <see cref="WorkflowAgent"/>
    /// </summary>
    /// <param name="instance">The extended <see cref="WorkflowInstance"/></param>
    /// <param name="agentQualifiedName">The qualified name of the <see cref="WorkflowAgent"/> to check</param>
    /// <returns>A boolean indicating whether or not the <see cref="WorkflowInstance"/> is managed by the specified <see cref="WorkflowAgent"/></returns>
    public static bool IsManagedBy(this WorkflowInstance instance, string agentQualifiedName)
    {
        if (string.IsNullOrWhiteSpace(agentQualifiedName)) throw new ArgumentNullException(nameof(agentQualifiedName));
        return instance.Metadata.Labels != null && instance.Metadata.Labels.TryGetValue(WorkflowInstance.Labels.Agent, out var agentRef) && agentRef == agentQualifiedName;
    }

    /// <summary>
    /// Determines whether or not the <see cref="WorkflowInstance"/> is managed by the specified <see cref="WorkflowAgent"/>
    /// </summary>
    /// <param name="instance">The extended <see cref="WorkflowInstance"/></param>
    /// <param name="agent">The <see cref="WorkflowAgent"/> to check</param>
    /// <returns>A boolean indicating whether or not the <see cref="WorkflowInstance"/> is managed by the specified <see cref="WorkflowAgent"/></returns>
    public static bool IsManagedBy(this WorkflowInstance instance, WorkflowAgent agent) => instance.IsManagedBy(agent.GetQualifiedName());

}