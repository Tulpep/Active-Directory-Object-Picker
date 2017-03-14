// ReSharper disable InconsistentNaming -- those names are from Windows/COM
namespace ADPickerTester
{
	/// <summary>
	/// Directory name types for use with IADsNameTranslate
	/// </summary>
	enum ADS_NAME_TYPE_ENUM
	{
		ADS_NAME_TYPE_1779 = 1,
		ADS_NAME_TYPE_CANONICAL = 2,
		ADS_NAME_TYPE_NT4 = 3,
		ADS_NAME_TYPE_DISPLAY = 4,
		ADS_NAME_TYPE_DOMAIN_SIMPLE = 5,
		ADS_NAME_TYPE_ENTERPRISE_SIMPLE = 6,
		ADS_NAME_TYPE_GUID = 7,
		ADS_NAME_TYPE_UNKNOWN = 8,
		ADS_NAME_TYPE_USER_PRINCIPAL_NAME = 9,
		ADS_NAME_TYPE_CANONICAL_EX = 10,
		ADS_NAME_TYPE_SERVICE_PRINCIPAL_NAME = 11,
		ADS_NAME_TYPE_SID_OR_SID_HISTORY_NAME = 12,
	}
}
// ReSharper restore InconsistentNaming
