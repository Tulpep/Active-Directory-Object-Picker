using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming -- those names are from Windows/COM
namespace Tulpep.ActiveDirectoryObjectPicker
{

	/// <summary>
	/// The object picker dialog box.
	/// </summary>
	[ComImport, Guid("17D6CCD8-3B7B-11D2-B9E0-00C04FD8DBF7")]
	internal class DSObjectPicker
	{
	}

	/// <summary>
	/// The IDsObjectPicker interface is used by an application to initialize and display an object picker dialog box. 
	/// </summary>
	[ComImport, Guid("0C87E64E-3B7A-11D2-B9E0-00C04FD8DBF7"),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IDsObjectPicker
	{
        [PreserveSig]
        int Initialize(ref DSOP_INIT_INFO pInitInfo);
        [PreserveSig]
		int InvokeDialog(IntPtr HWND, out IDataObject lpDataObject);
	}

	[ComImport, Guid("e2d3ec9b-d041-445a-8f16-4748de8fb1cf"), 
	 InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IDsObjectPickerCredentials
	{
		[PreserveSig]
		int Initialize(ref DSOP_INIT_INFO pInitInfo);
		[PreserveSig]
		int InvokeDialog(IntPtr HWND, out IDataObject lpDataObject);
		[PreserveSig]
		int SetCredentials(string userName, string password);
	}

	/// <summary>
	/// Interface to enable data transfers
	/// </summary>
	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
	Guid("0000010e-0000-0000-C000-000000000046")]
    internal interface IDataObject
	{
		[PreserveSig]
		int GetData(ref FORMATETC pFormatEtc, ref STGMEDIUM b);
		void GetDataHere(ref FORMATETC pFormatEtc, ref STGMEDIUM b);
		[PreserveSig]
		int QueryGetData(IntPtr a);
		[PreserveSig]
		int GetCanonicalFormatEtc(IntPtr a, IntPtr b);
		[PreserveSig]
		int SetData(IntPtr a, IntPtr b, int c);
		[PreserveSig]
		int EnumFormatEtc(uint a, IntPtr b);
		[PreserveSig]
		int DAdvise(IntPtr a, uint b, IntPtr c, ref uint d);
		[PreserveSig]
		int DUnadvise(uint a);
		[PreserveSig]
		int EnumDAdvise(IntPtr a);
	}

	[ComImport, Guid("9068270b-0939-11d1-8be1-00c04fd8d503")]
	internal interface IAdsLargeInteger
	{
		long HighPart { get; set; }
		long LowPart { get; set; }
	}

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
