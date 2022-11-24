// refactored from https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Game.Play.Items.Statistics
{
	[Serializable]
	public class Stat
	{
		public Action<Stat> OnChange;
		
		public float BaseValue = 1;

		protected bool isDirty = true;
		protected float lastBaseValue;

		[SerializeField, ReadOnly] protected float _value;
		public virtual float Value
		{
			get
			{
				// exit, no recalculation is necessary
				if (!isDirty && lastBaseValue == BaseValue) return _value;

				lastBaseValue = BaseValue;
				_value = CalculateFinalValue();
				isDirty = false;
				return _value;
			}
		}

		protected readonly List<StatModifier> statModifiers;
		public readonly ReadOnlyCollection<StatModifier> StatModifiers;

		public Stat()
		{
			statModifiers = new List<StatModifier>();
			StatModifiers = statModifiers.AsReadOnly();
		}

		public Stat(float baseValue) : this()
		{
			BaseValue = baseValue;
		}

		public virtual void AddModifier(StatModifier modifier)
		{
			isDirty = true;
			statModifiers.Add(modifier);
			
			OnChange?.Invoke(this);
		}

		public virtual bool RemoveModifier(StatModifier modifier)
		{
			// exit, modifier is not applies to this stat
			if (!statModifiers.Remove(modifier)) return false;

			isDirty = true;
			OnChange?.Invoke(this);
			return true;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = statModifiers.RemoveAll(mod => mod.Source == source);

			// exit, nothing was removed
			if (numRemovals <= 0) return false;

			isDirty = true;
			OnChange?.Invoke(this);
			return true;
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
			{
				return -1;
			}

			if (a.Order > b.Order)
			{
				return 1;
			}
			
			return 0; //if (a.Order == b.Order)
		}
		
		protected virtual float CalculateFinalValue()
		{
			float finalValue = BaseValue;
			float sumPercentAdd = 0;

			statModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < statModifiers.Count; i++)
			{
				StatModifier mod = statModifiers[i];

				switch (mod.Type)
				{
					case StatModType.Flat:
						finalValue += mod.Value;
						continue;
					
					case StatModType.PercentAdd:
					{
						sumPercentAdd += mod.Value;

						if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
						{
							finalValue *= 1 + sumPercentAdd;
							sumPercentAdd = 0;
						}

						continue;
					}
					case StatModType.PercentMult:
						finalValue *= 1 + mod.Value;
						break;
				}
			}

			// Workaround for float calculation errors, like displaying 12.00001 instead of 12
			return (float)Math.Round(finalValue, 4);
		}
	}
}
