/*
 * Copyright © 2022-Present The Synapse Authors
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System.Diagnostics.Metrics;

namespace Synapse.Application
{

    /// <summary>
    /// Exposes constants about Synapse's telemetry
    /// </summary>
    public static class Telemetry
    {

        /// <summary>
        /// Exposes constants about Synapse's telemetry service
        /// </summary>
        public static class Service
        {

            /// <summary>
            /// Gets the name of Synapse's telemetry service
            /// </summary>
            public const string Name = "Synapse";
            /// <summary>
            /// Gets the version of Synapse's telemetry service
            /// </summary>
            public static string Version = typeof(Telemetry).Assembly.GetName().Version!.ToString(3);

        }

        /// <summary>
        /// Exposes constants about Synapse's 
        /// </summary>
        public static class ActivitySource
        {

            /// <summary>
            /// Gets the name of Synapse's activity source
            /// </summary>
            public const string Name = "synapse";
            /// <summary>
            /// Gets the version of Synapse's activity source
            /// </summary>
            public static string Version = typeof(Telemetry).Assembly.GetName().Version!.ToString(3);

            internal static System.Diagnostics.ActivitySource Current = new(Name, Version);

        }

        internal static class Metrics
        {

            internal static Meter Meter = new("synapse");
            internal const string Prefix = "synapse-";

            internal static class Counters
            {

                public static Counter<int> Workflows = Meter.CreateCounter<int>(Prefix + "workflows");

                public static Counter<int> WorkflowInstances = Meter.CreateCounter<int>(Prefix + "workflow-instances");

                public static Counter<int> FaultedWorkflowInstances = Meter.CreateCounter<int>(Prefix + "workflow-instances-faulted");

                public static Counter<int> CancelledWorkflowInstances = Meter.CreateCounter<int>(Prefix + "workflow-instances-cancelled");

                public static Counter<int> CompletedWorkflowInstances = Meter.CreateCounter<int>(Prefix + "workflow-instances-completed");

                public static Counter<int> WorkflowActivities = Meter.CreateCounter<int>(Prefix + "workflow-activities");

                public static Counter<int> FaultedWorkflowActivities = Meter.CreateCounter<int>(Prefix + "workflow-activities-faulted");

                public static Counter<int> CancelledWorkflowActivities= Meter.CreateCounter<int>(Prefix + "workflow-activities-cancelled");

                public static Counter<int> CompletedWorkflowActivities = Meter.CreateCounter<int>(Prefix + "workflow-activities-completed");

            }

            internal static class Histograms
            {

                public static Histogram<double> WorkflowInstanceTime = Meter.CreateHistogram<double>(Prefix + "workflow-instance-time", unit: "ms");

                public static Histogram<double> WorkflowActivityTime = Meter.CreateHistogram<double>(Prefix + "workflow-activity-time", unit: "ms");

            }

            internal static KeyValuePair<string, object?>[] GetTagsFor(V1Workflow workflow)
            {
                if (workflow == null)
                    throw new ArgumentNullException(nameof(workflow));
                return Array.Empty<KeyValuePair<string, object?>>();
            }

            internal static KeyValuePair<string, object?>[] GetTagsFor(V1WorkflowInstance instance)
            {
                if (instance == null)
                    throw new ArgumentNullException(nameof(instance));
                return new KeyValuePair<string, object?>[]
                {
                    new("workflow-id", instance.WorkflowId)
                };
            }

            internal static KeyValuePair<string, object?>[] GetTagsFor(V1WorkflowActivity activity)
            {
                if (activity == null)
                    throw new ArgumentNullException(nameof(activity));
                return new KeyValuePair<string, object?>[]
                {
                    new("workflow-activity-type", activity.Type)
                };
            }

        }

    }

}
