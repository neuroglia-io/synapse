namespace Synapse;

/// <summary>
/// Exposes helpers to handle Hylo embedded resources
/// </summary>
public static class EmbeddedResources
{

    static readonly string Prefix = $"{typeof(EmbeddedResources).Namespace}.";

    /// <summary>
    /// Reads to end the stream of the specified embedded resource
    /// </summary>
    /// <param name="resourceName">The name of the embedded resource to read to end</param>
    /// <returns>The specified embedded resource's content</returns>
    public static string ReadToEnd(string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName)) throw new ArgumentNullException(nameof(resourceName));
        using var stream = typeof(EmbeddedResources).Assembly.GetManifestResourceStream(resourceName)!;
        using var streamReader = new StreamReader(stream);
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Exposes Hylo embedded assets
    /// </summary>
    public static class Assets
    {

        static readonly string Prefix = $"{EmbeddedResources.Prefix}Assets.";

        /// <summary>
        /// Exposes Hylo embedded resource definitions
        /// </summary>
        public static class Definitions
        {

            static readonly string Prefix = $"{Assets.Prefix}Definitions.";

            /// <summary>
            /// Gets the definition of workflow DSLs
            /// </summary>
            public static readonly string WorkflowDsl = $"{Prefix}workflow-dsl.yaml";
            /// <summary>
            /// Gets the definition of workflow DSL extensions
            /// </summary>
            public static readonly string WorkflowDslExtension = $"{Prefix}workflow-dsl-extension.yaml";
            /// <summary>
            /// Gets the definition of workflows
            /// </summary>
            public static readonly string Workflow = $"{Prefix}workflow.yaml";
            /// <summary>
            /// Gets the definition of workflow processes
            /// </summary>
            public static readonly string WorkflowProcess = $"{Prefix}workflow-process.yaml";
            /// <summary>
            /// Gets the definition of workflow runners
            /// </summary>
            public static readonly string WorkflowRunner = $"{Prefix}workflow-runner.yaml";

        }

    }

}