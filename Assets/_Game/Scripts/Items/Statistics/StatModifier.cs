// refactored from https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/

using UnityEngine;

namespace Game.Items.Statistics
{
	public enum StatModType
	{
		Flat = 100,
		PercentAdd = 200,
		PercentMult = 300,
	}

	[System.Serializable]
	public class StatModifier
	{
		public float Value => _value;
		public StatModType Type => _type;
		public int Order => _order;
		public object Source => _source;

		[SerializeField] float _value;
		[SerializeField] StatModType _type;
		[SerializeField] int _order;
		object _source;

		public StatModifier(float value, StatModType type, int order, object source = null)
		{
			_value = value;
			_type = type;
			_order = order;
			_source = source;
		}
		
		public StatModifier() : this(1, StatModType.Flat) { }
		public StatModifier(float value, StatModType type) : this(value, type, (int)type) { }
		public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
	}
}
