﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Placement of the tooltip for <see cref="HxTooltip"/>.
	/// </summary>
	public enum TooltipPlacement
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3,

		/// <summary>
		/// When is specified, it will dynamically reorient the tooltip.
		/// </summary>
		Auto = 4
	}
}
