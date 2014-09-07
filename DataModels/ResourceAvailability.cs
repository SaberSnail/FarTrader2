namespace FarTrader.DataModels
{
	internal sealed class ResourceAvailability
	{
		public ResourceAvailability(ResourceKind kind)
		{
			m_kind = kind;
		}

		public ResourceKind ResourceKind
		{
			get { return m_kind; }
		}

		public double RawAccessibility { get; set; }

		public double Quantity { get; set; }

		public double GetEffectiveAccessibility(TechnologyKind technology)
		{
			return TechnologyModifiers.GetInstance(technology).GetModifiedResourceAccessibility(m_kind, RawAccessibility);
		}

		readonly ResourceKind m_kind;
	}
}
