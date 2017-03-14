using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming -- those names are from Windows/COM
namespace ADPickerTester
{
	[ComImport, Guid("274fae1f-3626-11d1-a3a4-00c04fb950dc")]
	internal class NameTranslate
	{
	}

	[Guid("B1B272A3-3625-11D1-A3A4-00C04FB950DC"),
	InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	internal interface IADsNameTranslate
	{
		[DispId(1)]
		int ChaseReferral { set; }

		[DispId(5)]
		string Get(int lnFormatType);
		[DispId(7)]
		object GetEx(int lnFormatType);
		[DispId(2)]
		void Init(int lnSetType, string bstrADsPath);
		[DispId(3)]
		void InitEx(int lnSetType, string bstrADsPath, string bstrUserID, string bstrDomain, string bstrPassword);
		[DispId(4)]
		void Set(int lnSetType, string bstrADsPath);
		[DispId(6)]
		void SetEx(int lnFormatType, object pVar);
	}
}
// ReSharper restore InconsistentNaming
