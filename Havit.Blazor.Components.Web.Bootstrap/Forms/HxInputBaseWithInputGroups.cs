﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// A base class for form input components. This base class automatically integrates
	/// with an Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
	/// as a cascading parameter.
	/// Extends <seealso cref="HxInputBase{TValue}" /> class.
	/// Adds support for input groups, <a href="https://v5.getbootstrap.com/docs/5.0/forms/input-group/" />
	/// </summary>
	public abstract class HxInputBaseWithInputGroups<TValue> : HxInputBase<TValue>, IFormValueComponentWithInputGroups
	{
		/// <summary>
		/// Input-group at the beginning of the input.
		/// </summary>
		[Parameter] public string InputGroupStartText { get; set; }

		/// <summary>
		/// Input-group at the beginning of the input.
		/// </summary>
		[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

		/// <summary>
		/// Input-group at the end of the input.
		/// </summary>
		[Parameter] public string InputGroupEndText { get; set; }

		/// <summary>
		/// Input-group at the end of the input.
		/// </summary>
		[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }
	}
}