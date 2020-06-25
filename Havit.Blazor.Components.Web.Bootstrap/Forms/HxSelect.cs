﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	/// <summary>
	/// Select.
	/// </summary>
	/// <typeparam name="TValueType">Type of value.</typeparam>
	/// <typeparam name="TItemType">Type of items.</typeparam>
	public class HxSelect<TValueType, TItemType> : HxInputBase<TValueType>
	{
		/// <summary>
		/// Indicates when null is a valid value.
		/// </summary>
		[Parameter] public bool? Nullable { get; set; }

		/// <summary>
		/// Indicates when null is a valid value.
		/// Uses (in order) to get effective value: Nullable property, RequiresAttribute on bounded property (false), Nullable type on bounded property (true), class (true), default (false).
		/// </summary>
		protected bool NullableEffective
		{
			get
			{
				if (Nullable != null)
				{
					return Nullable.Value;
				}

				if (GetValueAttribute<RequiredAttribute>() != null)
				{
					return false;
				}

				if (System.Nullable.GetUnderlyingType(typeof(TValueType)) != null)
				{
					return true;
				}

				if (typeof(TValueType).IsClass)
				{
					return true;
				}

				return true;
			}
		}

		/// <summary>
		/// Text to display for null value.
		/// </summary>
		[Parameter] public string NullText { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// </summary>
		[Parameter] public Func<TItemType, TValueType> ValueSelector { get; set; } // TODO: Pojmenování? Pozor na podobnost s Value, ValueChanged, ValueExpression.

		/// <summary>
		/// Items to display. 
		/// </summary>
		[Parameter] public IEnumerable<TItemType> Items { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> Text { get; set; } // TODO: Pojmenování? Vs. ValueSelector vs. Sort.

		/// <summary>
		/// Selects value to sort items. Uses Text property when not set.
		/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
		/// </summary>
		[Parameter] public Func<TItemType, IComparable> Sort { get; set; } // TODO: Neumíme zřetězení výrazů pro řazení, v takovém případě buď umělou vlastnost s IComparable nebo seřadit předem.

		/// <summary>
		/// When true, items are sorted before displaying in select.
		/// Default value is true.
		/// </summary>
		[Parameter] public bool AutoSort { get; set; } = true;

		private List<TItemType> itemsToRender;

		/// <inheritdoc/>
		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);

			itemsToRender = Items?.ToList() ?? new List<TItemType>();

			// AutoSort
			if (AutoSort && (itemsToRender.Count > 1))
			{
				if (Sort != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.Sort).ToList();
				}
				else if (Text != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.Text).ToList();
				}
			}
		}

		/// <inheritdoc/>
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "select");
			BuildRenderInput_AddCommonAttributes(builder, null);

			builder.AddAttribute(1000, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));

			IEqualityComparer<TValueType> comparer = EqualityComparer<TValueType>.Default;
			TItemType selectedItem = itemsToRender.FirstOrDefault(item => comparer.Equals(Value, GetValueFromItem(item)));

			if ((Value != null) && (selectedItem == null))
			{
				throw new InvalidOperationException($"Items does not contain item for current value '{Value}'.");
			}

			if (NullableEffective || (selectedItem == null))
			{
				builder.OpenElement(2000, "option");
				builder.AddAttribute(2001, "value", -1);
				builder.AddContent(2002, NullText);
				builder.CloseElement();
			}

			for (int i = 0; i < itemsToRender.Count; i++)
			{
				var item = itemsToRender[i];
				if (item != null)
				{
					builder.OpenElement(3000, "option");
					builder.AddAttribute(3001, "value", i.ToString());
					builder.AddAttribute(3002, "selected", comparer.Equals(Value, GetValueFromItem(item)));
					builder.AddContent(3003, Text?.Invoke(item) ?? item?.ToString() ?? String.Empty);
					builder.CloseElement();
				}
			}

			builder.CloseElement();
		}

		/// <inheritdoc/>
		protected override bool TryParseValueFromString(string value, out TValueType result, out string validationErrorMessage)
		{
			int index = int.Parse(value);
			result = (index == -1)
				? default(TValueType)
				: GetValueFromItem(itemsToRender[index]);

			validationErrorMessage = null;
			return true;
		}

		private TValueType GetValueFromItem(TItemType item)
		{
			if (ValueSelector != null)
			{
				return ValueSelector(item);
			}

			if (typeof(TValueType) == typeof(TItemType))
			{
				return (TValueType)(object)item;
			}

			throw new InvalidOperationException("ValueSelector property not set.");
		}
	}
}