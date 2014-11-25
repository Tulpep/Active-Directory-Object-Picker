using System;

namespace Tulpep.ActiveDirectoryObjectPicker
{
	/// <summary>
	/// Indicates the ADsPaths provider type of the DirectoryObjectPickerDialog.
	/// This provider affects the contents of the ADPath returned
	/// </summary>
	[Flags]
	public enum ADsPathsProviders
	{
		Default = 0,
		
		/// <summary>
		/// The ADsPaths are converted to use the WinNT provider.
		/// 
		/// The ADsPath string for the ADSI WinNT provider can be one of the following forms:
		/// 
		/// <code>
		/// WinNT:
		/// WinNT://<domain name>
		/// WinNT://<domain name>/<server>
		/// WinNT://<domain name>/<path>
		/// WinNT://<domain name>/<object name>
		/// WinNT://<domain name>/<object name>,<object class>
		/// WinNT://<server>
		/// WinNT://<server>/<object name>
		/// WinNT://<server>/<object name>,<object class>
		/// </code>
		/// 
		/// The domain name can be either a NETBIOS name or a DNS name.
		/// The server is the name of a specific server within the domain.
		/// The path is the path of on object, such as "printserver1/printer2".
		/// The object name is the name of a specific object.
		/// The object class is the class name of the named object. One example of this usage would be "WinNT://MyServer/JeffSmith,user". Specifying a class name can improve the performance of the bind operation.
		/// </summary>
		WinNT = 0x00000002,

		/// <summary>
		/// The ADsPaths are converted to use the LDAP provider.
		/// 
		/// The Microsoft LDAP provider ADsPath requires the following format.
		/// 
		/// <code>
		/// LDAP://HostName[:PortNumber][/DistinguishedName]
		/// </code>
		/// 
		/// Further info, see <see cref="http://msdn.microsoft.com/en-us/library/aa746384(v=VS.85).aspx"/>.
		/// </summary>
		LDAP = 0x00000004,

		/// <summary>
		/// The ADsPaths for objects selected from this scope are converted to use the GC provider.
		/// </summary>
		GC = 0x00000008,

		/// <summary>
		/// The ADsPaths having an objectSid attribute are converted to the form 
		/// 
		/// <code>
		/// LDAP://<SID=x>
		/// </code>
		/// 
		/// where x represents the hexadecimal digits of the objectSid attribute value.
		/// </summary>
		SIDPath = 0x00000010,

		/// <summary>
		/// The ADsPaths for down-level, well-known SID objects are an empty string unless this flag is specified (For example; DSOP_DOWNLEVEL_FILTER_INTERACTIVE). If this flag is specified, the paths have the form: 
		/// 
		/// <code>
		/// WinNT://NT AUTHORITY/Interactive
		/// </code>
		/// 
		/// or:
		/// 
		/// <code>
		/// WinNT://Creator owner
		/// </code>
		/// </summary>
		DownlevelBuildinPath = 0x00000020
	}
}
