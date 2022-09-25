using System;
namespace Megahard.Science.Units
{

	public class VolumetricFlow : BaseQuantity
	{
		static VolumetricFlow()
		{
			s_Instance = new VolumetricFlow();
		}
		VolumetricFlow() : base("Volumetric Flow")
		{
			_SCCM = new PhysicalUnit("sccm", "standard cubic centimeters per minute", this, 1);
			_SLM = new PhysicalUnit("slm", "standard liter per minute", this, 1000);
			RegisterUnits(_SCCM, _SLM);
		}

		public static PhysicalQuantity AsSCCM(double val)
		{
			return new PhysicalQuantity(val, SCCM);
		}

		public static PhysicalQuantity AsSLM(double val)
		{
			return new PhysicalQuantity(val, SLM);
		}
		public static PhysicalUnit SCCM
		{
			get { return s_Instance._SCCM; }
		}
		public static PhysicalUnit SLM
		{
			get { return s_Instance._SLM; }
		}

		static readonly VolumetricFlow s_Instance;
		readonly PhysicalUnit _SCCM;
		readonly PhysicalUnit _SLM;
	}

}
