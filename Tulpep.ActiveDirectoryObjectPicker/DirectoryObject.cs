
namespace Tulpep.ActiveDirectoryObjectPicker
{
	/// <summary>
	/// Details of a directory object selected in the DirectoryObjectPickerDialog.
	/// </summary>
	public class DirectoryObject
    {
	    public DirectoryObject(string name, string path, string schemaClass, string upn, object[] fetchedAttributes)
        {
            this.Name = name;
            this.Path = path;
            this.SchemaClassName = schemaClass;
            this.Upn = upn;
            this.FetchedAttributes = fetchedAttributes;
        }

        /// <summary>
        /// Gets the Active Directory path for this directory object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The format of this string depends on the options specified in the DirectoryObjectPickerDialog
        /// from which this object was selected.
        /// </para>
        /// </remarks>
        public string Path { get; private set; }

	    /// <summary>
        /// Gets the name of the schema class for this directory object (objectClass attribute).
        /// </summary>
		public string SchemaClassName { get; private set; }

	    /// <summary>
        /// Gets the directory object's relative distinguished name (RDN).
        /// </summary>
		public string Name { get; private set; }

	    /// <summary>
        /// Gets the objects user principal name (userPrincipalName attribute).
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the object does not have a userPrincipalName value, this property is an empty string. 
        /// </para>
        /// </remarks>
		public string Upn { get; private set; }

	    /// <summary>
        /// Gets attributes retrieved by the object picker as it makes the selection.
        /// </summary>
        public object[] FetchedAttributes { get; private set; }
    }
}
