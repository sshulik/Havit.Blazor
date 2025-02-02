﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Input for entering tags.
	/// Does not allow duplicate tags.
	/// </summary>
	public class HxInputTags : HxInputBase<List<string>>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
	{
		// TODO Chips?
		// TODO Do we want spinner? Configurable?

		/// <summary>
		/// Application-wide defaults for the <see cref="HxInputTags"/>.
		/// </summary>
		public static InputTagsDefaults Defaults { get; } = new InputTagsDefaults();

		/// <summary>
		/// Indicates whether you are restricted to suggested items only (<c>false</c>).
		/// Default is <c>true</c> (you can type your own tags).
		/// </summary>
		[Parameter] public bool AllowCustomTags { get; set; } = true;

		/// <summary>
		/// Set to method providing data for tags' suggestions.
		/// </summary>
		[Parameter] public InputTagsDataProviderDelegate DataProvider { get; set; }

		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		[Parameter] public int? SuggestMinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int? SuggestDelay { get; set; } = 300;

		/// <summary>
		/// Characters, when typed, divide the current input into separate tags.
		/// Default is comma, semicolon and space.
		/// </summary>
		[Parameter] public List<char> Delimiters { get; set; }

		/// <summary>
		/// Indicates whether the add-icon (+) should be displayed.
		/// Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool? ShowAddButton { get; set; }

		/// <summary>
		/// Optional text for the add-button.
		/// Displayed only when there are no tags (the <c>Value</c> is empty).
		/// Default is <c>null</c> (none).
		/// </summary>
		[Parameter] public string AddButtonText { get; set; }

		/// <summary>
		/// Indicates whether a "naked" variant should be displayed (no border).
		/// Default is <c>false</c>.
		/// Consider enabling <see cref="ShowAddButton"/> when using <c>Naked</c>.
		/// </summary>
		[Parameter] public bool Naked { get; set; } = false;

		/// <summary>
		/// Short hint displayed in the input field before the user enters a value.
		/// </summary>
		[Parameter] public string Placeholder { get; set; }

		/// <summary>
		/// Background color of the tag (also used for the AddButton).
		/// Default is <see cref="ThemeColor.Light"/>.
		/// </summary>
		[Parameter] public ThemeColor? TagBackgroundColor { get; set; }

		/// <summary>
		/// Color of the tag text (also used for the AddButtonText and icons).
		/// Default is <see cref="ThemeColor.Dark"/>.
		/// </summary>
		[Parameter] public ThemeColor? TagTextColor { get; set; }

		/// <inheritdoc cref="HxInputBase{TValue}" />
		[Parameter] public LabelType? LabelType { get; set; }

		/// <inheritdoc cref="Bootstrap.InputSize" />
		[Parameter] public InputSize? InputSize { get; set; }

		/// <summary>
		/// Return <see cref="HxInputTags"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual InputTagsDefaults GetDefaults() => HxInputTags.Defaults;
		IInputDefaultsWithSize IInputWithSize.GetDefaults() => GetDefaults(); // might be replaced with C# vNext convariant return types on interfaces

		protected override LabelValueRenderOrder RenderOrder => (LabelType == Bootstrap.LabelType.Floating) ? LabelValueRenderOrder.ValueOnly /* renderování labelu zajistí HxInputTagsInternal */ : LabelValueRenderOrder.LabelValue;
		private protected override string CoreCssClass => "hx-input-tags position-relative";

		private HxInputTagsInternal hxInputTagsInternalComponent;

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			var defaults = this.GetDefaults();

			LabelType labelTypeEffective = (this as IInputWithLabelType).LabelTypeEffective;

			builder.OpenComponent<HxInputTagsInternal>(1);
			builder.AddAttribute(1000, nameof(HxInputTagsInternal.Value), Value);
			builder.AddAttribute(1001, nameof(HxInputTagsInternal.ValueChanged), EventCallback.Factory.Create<List<string>>(this, HandleValueChanged));
			builder.AddAttribute(1002, nameof(HxInputTagsInternal.DataProvider), DataProvider);
			builder.AddAttribute(1005, nameof(HxInputTagsInternal.SuggestMinimumLength), SuggestMinimumLength ?? defaults.SuggestMinimumLength);
			builder.AddAttribute(1006, nameof(HxInputTagsInternal.SuggestDelay), SuggestDelay ?? defaults.SuggestDelay);
			builder.AddAttribute(1007, nameof(HxInputTagsInternal.InputId), InputId);
			builder.AddAttribute(1008, nameof(HxInputTagsInternal.CoreFormControlCssClass), GetInputCssClassToRender()); // we want to shift original input-classes to the wrapping .form-control container
			builder.AddAttribute(1009, nameof(HxInputTagsInternal.EnabledEffective), EnabledEffective);
			builder.AddAttribute(1011, nameof(HxInputTagsInternal.Placeholder), (labelTypeEffective == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating) ? "placeholder" : Placeholder);
			builder.AddAttribute(1012, nameof(HxInputTagsInternal.LabelTypeEffective), labelTypeEffective);
			builder.AddAttribute(1013, nameof(HxInputTagsInternal.AllowCustomTags), AllowCustomTags);
			builder.AddAttribute(1014, nameof(HxInputTagsInternal.Delimiters), Delimiters ?? defaults.Delimiters);
			builder.AddAttribute(1015, nameof(HxInputTagsInternal.InputSizeEffective), ((IInputWithSize)this).InputSizeEffective);
			builder.AddAttribute(1016, nameof(HxInputTagsInternal.ShowAddButtonEffective), ShowAddButton ?? defaults.ShowAddButton);
			builder.AddAttribute(1017, nameof(HxInputTagsInternal.Naked), Naked);
			builder.AddAttribute(1018, nameof(HxInputTagsInternal.CssClass), CssClassHelper.Combine(this.CssClass, IsValueInvalid() ? InvalidCssClass : null));
			builder.AddAttribute(1019, nameof(HxInputTagsInternal.AddButtonText), this.AddButtonText);
			builder.AddAttribute(1020, nameof(HxInputTagsInternal.TagBackgroundColor), this.TagBackgroundColor ?? defaults.TagBackgroundColor);
			builder.AddAttribute(1021, nameof(HxInputTagsInternal.TagTextColor), this.TagTextColor ?? defaults.TagTextColor);
			builder.AddComponentReferenceCapture(1100, component => hxInputTagsInternalComponent = (HxInputTagsInternal)component);
			builder.CloseComponent();
		}

		private void HandleValueChanged(List<string> newValue)
		{
			CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out List<string> result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		public override async ValueTask FocusAsync()
		{
			if (hxInputTagsInternalComponent == null)
			{
				throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
			}

			await hxInputTagsInternalComponent.FocusAsync();
		}

		/// <inheritdoc />
		protected override void RenderChipGenerator(RenderTreeBuilder builder)
		{
			if (Value?.Any() ?? false)
			{
				base.RenderChipGenerator(builder);
			}
		}

		/// <inheritdoc />
		protected override void RenderChipValue(RenderTreeBuilder builder)
		{
			builder.AddContent(0, String.Join(", ", Value));
		}
	}
}
