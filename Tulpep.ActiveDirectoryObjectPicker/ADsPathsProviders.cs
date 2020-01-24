using System;

namespace Tulpep.ActiveDirectoryObjectPicker
{
	/// <summary>
	/// Indicates the ADsPaths provider type of the DirectoryObjectPickerDialog. This provider affects the contents of the ADPath returned
	/// </summary>
	[Flags]
	public enum ADsPathsProviders
	{
		/// <summary>Default provider.</summary>
		Default = 0,

		/// <summary>
		/// The ADsPaths are converted to use the WinNT provider.
		///
		/// The ADsPath string for the ADSI WinNT provider can be one of the following forms:
		/// <code>
		///WinNT:
		///WinNT://&lt;domain name&gt;
		///WinNT://&lt;domain name&gt;/&lt;server&gt;
		///WinNT://&lt;domain name&gt;/&lt;path&gt;
		///WinNT://&lt;domain name&gt;/&lt;object name&gt;
		///WinNT://&lt;domain name&gt;/&lt;object name&gt;,&lt;object class&gt;
		///WinNT://&lt;server&gt;
		///WinNT://&lt;server&gt;/&lt;object name&gt;
		///WinNT://&lt;server&gt;/&lt;object name&gt;,&lt;object class&gt;
		/// </code>
		/// The domain name can be either a NETBIOS name or a DNS name. The server is the name of a specific server within the domain. The
		/// path is the path of on object, such as "printserver1/printer2". The object name is the name of a specific object. The object
		/// class is the class name of the named object. One example of this usage would be "WinNT://MyServer/JeffSmith,user". Specifying a
		/// class name can improve the performance of the bind operation.
		/// </summary>
		WinNT = 0x00000002,

		/// <summary>
		/// The ADsPaths are converted to use the LDAP provider.
		/// <para>The Microsoft LDAP provider ADsPath requires the following format.</para>
		/// <para>LDAP://HostName[:PortNumber][/DistinguishedName]</para>
		/// <para>Further info, see <a href="http://msdn.microsoft.com/en-us/library/aa746384(v=VS.85).aspx">http://msdn.microsoft.com/en-us/library/aa746384(v=VS.85).aspx</a>.</para>
		/// </summary>
		LDAP = 0x00000004,

		/// <summary>The ADsPaths for objects selected from this scope are converted to use the GC provider.</summary>
		GC = 0x00000008,

		/// <summary>
		/// The ADsPaths having an objectSid attribute are converted to the form
		/// <code>
		///LDAP://&lt;SID=x&gt;
		/// </code>
		/// where x represents the hexadecimal digits of the objectSid attribute value.
		/// </summary>
		SIDPath = 0x00000010,

		/// <summary>
		/// The ADsPaths for down-level, well-known SID objects are an empty string unless this flag is specified (For example;
		/// DSOP_DOWNLEVEL_FILTER_INTERACTIVE). If this flag is specified, the paths have the form:
		/// <para><c>WinNT://NT AUTHORITY/Interactive</c> or <c>WinNT://Creator owner</c>.</para>
		/// </summary>
		DownlevelBuiltinPath = 0x00000020,

		/// <summary>Use DownlevelBuiltinPath instead.</summary>
		[Obsolete("Use DownlevelBuiltinPath instead.")]
		DownlevelBuildinPath = 0x00000020
	}
}