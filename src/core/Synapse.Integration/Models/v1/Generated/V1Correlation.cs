﻿
/*
 * Copyright © 2022-Present The Synapse Authors
 * <p>
 * Licensed under the Apache License, Version 2.0(the "License");
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
 */

/* -----------------------------------------------------------------------
 * This file has been automatically generated by a tool
 * -----------------------------------------------------------------------
 */

namespace Synapse.Integration.Models
{

	/// <summary>
	/// Represents an event correlation
	/// </summary>
	[DataContract]
	[Queryable]
	public partial class V1Correlation
		: Entity
	{

		/// <summary>
		/// The V1Correlation's lifetime
		/// </summary>
		[DataMember(Name = "Lifetime", Order = 1)]
		[Description("The V1Correlation's lifetime")]
		public virtual V1CorrelationLifetime Lifetime { get; set; }

		/// <summary>
		/// A value determining the type of the V1Correlation's V1CorrelationCondition evaluation
		/// </summary>
		[DataMember(Name = "ConditionType", Order = 2)]
		[Description("A value determining the type of the V1Correlation's V1CorrelationCondition evaluation")]
		public virtual V1CorrelationConditionType ConditionType { get; set; }

		/// <summary>
		/// The outcome of the V1Correlation
		/// </summary>
		[DataMember(Name = "Outcome", Order = 3)]
		[Description("The outcome of the V1Correlation")]
		public virtual V1CorrelationOutcome Outcome { get; set; }

    }

}
