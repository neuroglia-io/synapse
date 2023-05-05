namespace Synapse;

/// <summary>
/// Defines extensions for <see cref="WorkflowVersion"/>s
/// </summary>
public static class WorkflowVersionExtensions
{

    /// <summary>
    /// Attempts to get the <see cref="WorkflowVersion"/> with the specified name, if any
    /// </summary>
    /// <param name="versions">The extended <see cref="WorkflowVersion"/></param>
    /// <param name="name">The name of the <see cref="WorkflowVersion"/> to get</param>
    /// <param name="version">The <see cref="WorkflowVersion"/> with the specified name, if any</param>
    /// <returns>A boolean indicating whether or not the specified version could be found</returns>
    public static bool TryGetVersion(this IEnumerable<WorkflowVersion> versions, string name, out WorkflowVersion? version)
    {
        if(versions == null) throw new ArgumentNullException(nameof(versions));
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        version = versions.FirstOrDefault(v => v.Name == name);
        return version != null;
    }

}
