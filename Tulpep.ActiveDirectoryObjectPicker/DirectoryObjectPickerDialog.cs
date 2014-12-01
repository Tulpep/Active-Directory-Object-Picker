// Copyright 2004 Armand du Plessis <armand@dotnet.org.za>
// http://dotnet.org.za/armand/articles/2453.aspx
// Thanks to Joe Cataford and Friedrich Brunzema.
// Also see MSDN: http://msdn2.microsoft.com/en-us/library/ms675899.aspx
// Enhancements by sgryphon@computer.org (PACOM):
// - Integrated with the CommonDialog API, e.g. returns a DialogResult, changed namespace, etc.
// - Marked COM interop code as internal; only the main dialog (and related classes) are public.
// - Added basic scope (location) and filter (object type) control, plus multi-select flag.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tulpep.ActiveDirectoryObjectPicker
{
    /// <summary>
    /// Represents a common dialog that allows a user to select directory objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The directory object picker dialog box enables a user to select one or more objects 
    /// from either the global catalog, a Microsoft Windows 2000 domain or computer, 
    /// a Microsoft Windows NT 4.0 domain or computer, or a workgroup. The object types 
    /// from which a user can select include user, contact, group, and computer objects.
    /// </para>
    /// <para>
    /// This managed class wraps the Directory Object Picker common dialog from 
    /// the Active Directory UI.
    /// </para>
    /// <para>
    /// It simplifies the scope (Locations) and filter (ObjectTypes) selection by allowing a single filter to be
    /// specified which applies to all scopes (translating to both up-level and down-level
    /// filter flags as necessary).
    /// </para>
    /// <para>
    /// The object type filter is also simplified by combining different types of groups (local, global, etc)
    /// and not using individual well known types in down-level scopes (only all well known types
    /// can be specified).
    /// </para>
    /// <para>
    /// The scope location is also simplified by combining down-level and up-level variations
    /// into a single locations flag, e.g. external domains.
    /// </para>
    /// </remarks>
	public class DirectoryObjectPickerDialog : CommonDialog
	{
        private Locations allowedLocations;
        private ObjectTypes allowedTypes;
        private Locations defaultLocations;
        private ObjectTypes defaultTypes;
        private ADsPathsProviders providers;
        private bool multiSelect;
        private bool skipDomainControllerCheck;
        private List<string> attributesToFetch;
        private DirectoryObject[] selectedObjects;
        private bool showAdvancedView;
        private string targetComputer;

        /// <summary>
        /// Constructor. Sets all properties to their default values.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default values for the DirectoryObjectPickerDialog properties are:
        /// </para>
        /// <para>
        /// <list type="table">
        /// <listheader><term>Property</term><description>Default value</description></listheader>
        /// <item><term>AllowedLocations</term><description>All locations.</description></item>
        /// <item><term>AllowedObjectTypes</term><description>All object types.</description></item>
        /// <item><term>DefaultLocations</term><description>None. (Will default to first location.)</description></item>
        /// <item><term>DefaultObjectTypes</term><description>All object types.</description></item>
        /// <item><term>MultiSelect</term><description>false.</description></item>
        /// <item><term>SelectedObject</term><description>null.</description></item>
        /// <item><term>SelectedObjects</term><description>Empty array.</description></item>
        /// <item><term>ShowAdvancedView</term><description>false.</description></item>
        /// <item><term>TargetComputer</term><description>null.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public DirectoryObjectPickerDialog()
        {
            ResetInner();
        }

        /// <summary>
        /// Gets or sets the scopes the DirectoryObjectPickerDialog is allowed to search.
        /// </summary>
        public Locations AllowedLocations
        {
            get { return allowedLocations; }
            set { allowedLocations = value; }
        }

        /// <summary>
        /// Gets or sets the types of objects the DirectoryObjectPickerDialog is allowed to search for.
        /// </summary>
        public ObjectTypes AllowedObjectTypes
        {
            get { return allowedTypes; }
            set { allowedTypes = value; }
        }

        /// <summary>
        /// Gets or sets the initially selected scope in the DirectoryObjectPickerDialog.
        /// </summary>
        public Locations DefaultLocations
        {
            get { return defaultLocations; }
            set { defaultLocations = value; }
        }

        /// <summary>
        /// Gets or sets the initially seleted types of objects in the DirectoryObjectPickerDialog.
        /// </summary>
        public ObjectTypes DefaultObjectTypes
        {
            get { return defaultTypes; }
            set { defaultTypes = value; }
        }

        /// <summary>
        /// Gets or sets the providers affecting the ADPath returned in objects.
        /// </summary>
        public ADsPathsProviders Providers
        {
            get { return providers; }
            set { providers = value; }
        }

        /// <summary>
        /// Gets or sets whether the user can select multiple objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this flag is false, the user can select only one object.
        /// </para>
        /// </remarks>
        public bool MultiSelect
        {
            get { return multiSelect; }
            set { multiSelect = value; }
        }

        /// <summary>
        /// Gets or sets the whether to check whether the target is a Domain Controller and hide the "Local Computer" scope
        /// </summary>
        /// <remarks>
        /// <para>
        /// From MSDN:
        /// 
        /// If this flag is NOT set, then the DSOP_SCOPE_TYPE_TARGET_COMPUTER flag
        /// will be ignored if the target computer is a DC.  This flag has no effect
        /// unless DSOP_SCOPE_TYPE_TARGET_COMPUTER is specified.
        /// </para>
        /// </remarks>
        public bool SkipDomainControllerCheck
        {
            get { return skipDomainControllerCheck; }
            set { skipDomainControllerCheck = value; }
        }

        /// <summary>
        /// An list of LDAP attribute names that will be retrieved for picked objects
        /// </summary>
        public IList<string> AttributesToFetch
        {
            get { return attributesToFetch; }
        }

        /// <summary>
        /// Gets the directory object selected in the dialog, or null if no object was selected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If MultiSelect is enabled, then this property returns only the first selected object.
        /// Use SelectedObjects to get an array of all objects selected.
        /// </para>
        /// </remarks>
        public DirectoryObject SelectedObject
        {
            get
            {
                if (selectedObjects == null || selectedObjects.Length == 0)
                {
                    return null;
                }
                return selectedObjects[0];
            }
        }

        /// <summary>
        /// Gets an array of the directory objects selected in the dialog.
        /// </summary>
        public DirectoryObject[] SelectedObjects
        {
            get
            {
                if (selectedObjects == null)
                {
                    return new DirectoryObject[0];
                }
                return (DirectoryObject[])selectedObjects.Clone();
            }
        }

        /// <summary>
        /// Gets or sets whether objects flagged as show in advanced view only are displayed (up-level).
        /// </summary>
        public bool ShowAdvancedView
        {
            get { return showAdvancedView; }
            set { showAdvancedView = value; }
        }

        /// <summary>
        /// Gets or sets the name of the target computer. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The dialog box operates as if it is running on the target computer, using the target computer 
        /// to determine the joined domain and enterprise. If this value is null or empty, the target computer 
        /// is the local computer.
        /// </para>
        /// </remarks>
        public string TargetComputer
        {
            get { return targetComputer; }
            set { targetComputer = value; }
        }

        /// <summary>
        /// Resets all properties to their default values. 
        /// </summary>
        public override void Reset()
        {
            ResetInner();
        }

        private void ResetInner() // can be called from constructor without a "Virtual member call in constructor" warning
        {
            allowedLocations = Locations.All;
            allowedTypes = ObjectTypes.All;
            defaultLocations = Locations.None;
            defaultTypes = ObjectTypes.All;
            providers = ADsPathsProviders.Default;
            multiSelect = false;
            skipDomainControllerCheck = false;
            attributesToFetch = new List<string>();
            selectedObjects = null;
            showAdvancedView = false;
            targetComputer = null;
        }


        /// <summary>
        /// Displays the Directory Object Picker (Active Directory) common dialog, when called by ShowDialog.
        /// </summary>
        /// <param name="hwndOwner">Handle to the window that owns the dialog.</param>
        /// <returns>If the user clicks the OK button of the Directory Object Picker dialog that is displayed, true is returned; 
        /// otherwise, false.</returns>
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            IDsObjectPicker ipicker = Initialize();
            if (ipicker == null)
            {
                selectedObjects = null;
                return false;
            }

            try
            {
                IDataObject dataObj = null;
                int hresult = ipicker.InvokeDialog(hwndOwner, out dataObj);
                if (hresult == HRESULT.S_OK)
                {
                    selectedObjects = ProcessSelections(dataObj);
                    Marshal.ReleaseComObject(dataObj);
                    return true;
                }
                else if (hresult == HRESULT.S_FALSE)
                {
                    selectedObjects = null;
                    return false;
                }
                else
                    throw new COMException("IDsObjectPicker.InvokeDialog failed", hresult);
            }
            finally
            {
                Marshal.ReleaseComObject(ipicker);
            }
        }

        #region Private implementation

        // Convert ObjectTypes to DSCOPE_SCOPE_INIT_INFO_FLAGS
        private uint GetDefaultFilter()
        {
            uint defaultFilter = 0;
            if (((defaultTypes & ObjectTypes.Users) == ObjectTypes.Users) ||
                ((defaultTypes & ObjectTypes.WellKnownPrincipals) == ObjectTypes.WellKnownPrincipals))
            {
                defaultFilter |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS;
            }
            if (((defaultTypes & ObjectTypes.Groups) == ObjectTypes.Groups) ||
                ((defaultTypes & ObjectTypes.BuiltInGroups) == ObjectTypes.BuiltInGroups))
            {
                defaultFilter |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS;
            }
            if ((defaultTypes & ObjectTypes.Computers) == ObjectTypes.Computers)
            {
                defaultFilter |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS;
            }
            if ((defaultTypes & ObjectTypes.Contacts) == ObjectTypes.Contacts)
            {
                defaultFilter |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS;
            }
            return defaultFilter;
        }

        // Convert ObjectTypes to DSOP_DOWNLEVEL_FLAGS
        private uint GetDownLevelFilter()
        {
            uint downlevelFilter = 0;
            if ((allowedTypes & ObjectTypes.Users) == ObjectTypes.Users)
            {
                downlevelFilter |= DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS;
            }
            if ((allowedTypes & ObjectTypes.Groups) == ObjectTypes.Groups)
            {
                downlevelFilter |= DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS |
                                    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS;
            }
            if ((allowedTypes & ObjectTypes.Computers) == ObjectTypes.Computers)
            {
                downlevelFilter |=  DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_COMPUTERS;
            }
            // Contacts not available in downlevel scopes
            //if ((allowedTypes & ObjectTypes.Contacts) == ObjectTypes.Contacts)
            // Exclude build in groups if not selected
            if ((allowedTypes & ObjectTypes.BuiltInGroups) == 0)
            {
                downlevelFilter |= DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_EXCLUDE_BUILTIN_GROUPS;
            }
            if ((allowedTypes & ObjectTypes.WellKnownPrincipals) == ObjectTypes.WellKnownPrincipals)
            {
                downlevelFilter |= DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS;
                // This includes all the following:
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_WORLD |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ANONYMOUS |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_BATCH |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_DIALUP |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_INTERACTIVE |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SERVICE |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SYSTEM |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE |
                //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON;
            }
            return downlevelFilter;
        }

        // Convert ObjectTypes to DSOP_FILTER_FLAGS_FLAGS
        private uint GetUpLevelFilter()
        {
            uint uplevelFilter = 0;
            if ((allowedTypes & ObjectTypes.Users) == ObjectTypes.Users)
            {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS;
            }
            if ((allowedTypes & ObjectTypes.Groups) == ObjectTypes.Groups)
            {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL |
                                DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE |
                                DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL |
                                DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE |
                                DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL |
                                DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE;
            }
            if ((allowedTypes & ObjectTypes.Computers) == ObjectTypes.Computers)
            {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_COMPUTERS;
            }
            if ((allowedTypes & ObjectTypes.Contacts) == ObjectTypes.Contacts)
            {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_CONTACTS;
            }
            if ((allowedTypes & ObjectTypes.BuiltInGroups) == ObjectTypes.BuiltInGroups)
            {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS;
            }
            if ((allowedTypes & ObjectTypes.WellKnownPrincipals) == ObjectTypes.WellKnownPrincipals)
            {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_WELL_KNOWN_PRINCIPALS;
            }
            if( showAdvancedView ) {
                uplevelFilter |= DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW;
            }
            return uplevelFilter;
        }

        // Convert Locations to DSOP_SCOPE_TYPE_FLAGS
        private static uint GetScope( Locations locations )
        {
            uint scope = 0;
            if ((locations & Locations.LocalComputer) == Locations.LocalComputer)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER;
            }
            if ((locations & Locations.JoinedDomain) == Locations.JoinedDomain)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN |
                        DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN;
            }
            if ((locations & Locations.EnterpriseDomain) == Locations.EnterpriseDomain)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN;
            }
            if ((locations & Locations.GlobalCatalog) == Locations.GlobalCatalog)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG;
            }
            if ((locations & Locations.ExternalDomain) == Locations.ExternalDomain)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN |
                        DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN;
            }
            if ((locations & Locations.Workgroup) == Locations.Workgroup)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_WORKGROUP;
            }
            if ((locations & Locations.UserEntered) == Locations.UserEntered)
            {
                scope |= DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE |
                DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE;
            }
            return scope;
        }

        // Convert ADsPathsProviders to DSOP_SCOPE_INIT_INFO_FLAGS
        private uint GetProviderFlags()
        {
            uint scope = 0;
            if ((providers & ADsPathsProviders.WinNT) == ADsPathsProviders.WinNT)
                scope |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT;

            if ((providers & ADsPathsProviders.LDAP) == ADsPathsProviders.LDAP)
                scope |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP;

            if ((providers & ADsPathsProviders.GC) == ADsPathsProviders.GC)
                scope |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_GC;

            if ((providers & ADsPathsProviders.SIDPath) == ADsPathsProviders.SIDPath)
                scope |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_SID_PATH;

            if ((providers & ADsPathsProviders.DownlevelBuildinPath) == ADsPathsProviders.DownlevelBuildinPath)
                scope |= DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH;

            return scope;
        }


		private IDsObjectPicker Initialize()
		{
			DSObjectPicker picker = new DSObjectPicker();
			IDsObjectPicker ipicker = (IDsObjectPicker)picker;

            List<DSOP_SCOPE_INIT_INFO> scopeInitInfoList = new List<DSOP_SCOPE_INIT_INFO>();

            // Note the same default and filters are used by all scopes
            uint defaultFilter = GetDefaultFilter();
            uint upLevelFilter = GetUpLevelFilter();
            uint downLevelFilter = GetDownLevelFilter();
            uint providerFlags = GetProviderFlags ();

            // Internall, use one scope for the default (starting) locations.
            uint startingScope = GetScope(defaultLocations);
            if (startingScope > 0)
            {
                DSOP_SCOPE_INIT_INFO startingScopeInfo = new DSOP_SCOPE_INIT_INFO();
                startingScopeInfo.cbSize = (uint)Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO));
                startingScopeInfo.flType = startingScope;
                startingScopeInfo.flScope = DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_STARTING_SCOPE | defaultFilter | providerFlags;
                startingScopeInfo.FilterFlags.Uplevel.flBothModes = upLevelFilter;
                startingScopeInfo.FilterFlags.flDownlevel = downLevelFilter;
                startingScopeInfo.pwzADsPath = null;
                startingScopeInfo.pwzDcName = null;
                startingScopeInfo.hr = 0;
                scopeInitInfoList.Add(startingScopeInfo);
            }

            // And another scope for all other locations (AllowedLocation values not in DefaultLocation)
            Locations otherLocations = allowedLocations & (~defaultLocations);
            uint otherScope = GetScope(otherLocations);
            if (otherScope > 0)
            {
                DSOP_SCOPE_INIT_INFO otherScopeInfo = new DSOP_SCOPE_INIT_INFO();
                otherScopeInfo.cbSize = (uint)Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO));
                otherScopeInfo.flType = otherScope;
                otherScopeInfo.flScope = defaultFilter | providerFlags;
                otherScopeInfo.FilterFlags.Uplevel.flBothModes = upLevelFilter;
                otherScopeInfo.FilterFlags.flDownlevel = downLevelFilter;
                otherScopeInfo.pwzADsPath = null;
                otherScopeInfo.pwzDcName = null;
                otherScopeInfo.hr = 0;
                scopeInitInfoList.Add(otherScopeInfo);
            }

            DSOP_SCOPE_INIT_INFO[] scopeInitInfo = scopeInitInfoList.ToArray();

            // TODO: Scopes for alternate ADs, alternate domains, alternate computers, etc

			// Allocate memory from the unmananged mem of the process, this should be freed later!??
			IntPtr refScopeInitInfo = Marshal.AllocHGlobal
                (Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO)) * scopeInitInfo.Length);
			
			// Marshal structs to pointers
            for (int index = 0; index < scopeInitInfo.Length; index++)
            {
                Marshal.StructureToPtr(scopeInitInfo[index],
                                       refScopeInitInfo.OffsetWith(index * Marshal.SizeOf(typeof(DSOP_SCOPE_INIT_INFO))),
                                       false);
            }

			// Initialize structure with data to initialize an object picker dialog box. 
			DSOP_INIT_INFO initInfo = new DSOP_INIT_INFO (); 						
			initInfo.cbSize = (uint) Marshal.SizeOf (initInfo); 
			//initInfo.pwzTargetComputer = null; // local computer
            initInfo.pwzTargetComputer = targetComputer;
            initInfo.cDsScopeInfos = (uint)scopeInitInfo.Length; 
			initInfo.aDsScopeInfos = refScopeInitInfo;  
			// Flags that determine the object picker options. 
            uint flOptions = 0; 
            if (multiSelect)
            {
                flOptions |= DSOP_INIT_INFO_FLAGS.DSOP_FLAG_MULTISELECT;
            }
            // Only set DSOP_INIT_INFO_FLAGS.DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK
            // if we know target is not a DC (which then saves initialization time).
            if (skipDomainControllerCheck)
            {
                flOptions |= DSOP_INIT_INFO_FLAGS.DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK;
            }
			initInfo.flOptions = flOptions;
			
            // there's a (seeming?) bug on my Windows XP when fetching the objectClass attribute - the pwzClass field is corrupted...
            // plus, it returns a multivalued array for this attribute. In Windows 2008 R2, however, only last value is returned,
            // just as in pwzClass. So won't actually be retrieving __objectClass__ - will give pwzClass instead
            List<string> goingToFetch = new List<string>(attributesToFetch);
            for (int i = 0; i < goingToFetch.Count; i++)
            {
                if (goingToFetch[i].Equals("objectClass", StringComparison.OrdinalIgnoreCase))
                    goingToFetch[i] = "__objectClass__";
            }

            initInfo.cAttributesToFetch = (uint)goingToFetch.Count;
            UnmanagedArrayOfStrings unmanagedAttributesToFetch = new UnmanagedArrayOfStrings(goingToFetch);
            initInfo.apwzAttributeNames = unmanagedAttributesToFetch.ArrayPtr;
			
            try
            {
                // Initialize the Object Picker Dialog Box with our options
                int hresult = ipicker.Initialize (ref initInfo);
                if (hresult != HRESULT.S_OK)
                {
                    Marshal.ReleaseComObject(ipicker);
                    throw new COMException("IDsObjectPicker.Initialize failed", hresult);
                }
                return ipicker;
            }
            finally
            {
               /*
                from MSDN (http://msdn.microsoft.com/en-us/library/ms675899(VS.85).aspx):
                
                    Initialize can be called multiple times, but only the last call has effect.
                    Be aware that object picker makes its own copy of InitInfo. 
                */
                Marshal.FreeHGlobal(refScopeInitInfo);
                unmanagedAttributesToFetch.Dispose();
             }
		}

		private DirectoryObject[] ProcessSelections(IDataObject dataObj)
		{
			if(dataObj == null)
				return null;			

			DirectoryObject[] selections = null;

			// The STGMEDIUM structure is a generalized global memory handle used for data transfer operations
			STGMEDIUM stg = new STGMEDIUM();
			stg.tymed = (uint)TYMED.TYMED_HGLOBAL;
			stg.hGlobal = IntPtr.Zero;
            stg.pUnkForRelease = IntPtr.Zero;

			// The FORMATETC structure is a generalized Clipboard format.
			FORMATETC fe = new FORMATETC();
            fe.cfFormat = System.Windows.Forms.DataFormats.GetFormat(CLIPBOARD_FORMAT.CFSTR_DSOP_DS_SELECTION_LIST).Id; 
            // The CFSTR_DSOP_DS_SELECTION_LIST clipboard format is provided by the IDataObject obtained 
            // by calling IDsObjectPicker::InvokeDialog
			fe.ptd = IntPtr.Zero;
			fe.dwAspect = 1; //DVASPECT_CONTENT    = 1,  
			fe.lindex = -1; // all of the data
			fe.tymed = (uint)TYMED.TYMED_HGLOBAL; //The storage medium is a global memory handle (HGLOBAL)

			int hresult = dataObj.GetData(ref fe, ref stg);
			if (hresult != HRESULT.S_OK) throw new COMException("IDataObject.GetData failed", hresult);
	
			IntPtr pDsSL = PInvoke.GlobalLock(stg.hGlobal);
			if (pDsSL == IntPtr.Zero) throw new Win32Exception("GlobalLock(stg.hGlobal) failed");

			try
			{
				// the start of our structure
				IntPtr current = pDsSL;
				// get the # of items selected
				int cnt = Marshal.ReadInt32(current);
                int cFetchedAttributes = Marshal.ReadInt32(current, Marshal.SizeOf(typeof(uint)));
				
				// if we selected at least 1 object
				if (cnt > 0)
				{				
					selections = new DirectoryObject[cnt];
					// increment the pointer so we can read the DS_SELECTION structure
					current = current.OffsetWith(Marshal.SizeOf(typeof(uint))*2);
					// now loop through the structures
					for (int i = 0; i < cnt; i++)
					{
						// marshal the pointer to the structure
						DS_SELECTION s = (DS_SELECTION)Marshal.PtrToStructure(current, typeof(DS_SELECTION));

						// increment the position of our pointer by the size of the structure
                        current = current.OffsetWith(Marshal.SizeOf(typeof(DS_SELECTION)));

                        string name = s.pwzName;
                        string path = s.pwzADsPath;
                        string schemaClassName = s.pwzClass;
                        string upn =  s.pwzUPN;
                        object[] fetchedAttributes = GetFetchedAttributes(s.pvarFetchedAttributes, cFetchedAttributes, schemaClassName);

                        selections[i] = new DirectoryObject(name, path, schemaClassName, upn, fetchedAttributes);
					}
				}
			}			
			finally
			{
                PInvoke.GlobalUnlock(pDsSL);
                PInvoke.ReleaseStgMedium(ref stg);
			}		
			return selections;
        }

        private Object[] GetFetchedAttributes(IntPtr pvarFetchedAttributes, int cFetchedAttributes, string schemaClassName)
        {
            object[] fetchedAttributes;
            if (cFetchedAttributes > 0)
                fetchedAttributes = Marshal.GetObjectsForNativeVariants(pvarFetchedAttributes, cFetchedAttributes);
            else
                fetchedAttributes = new Object[0];

            for (int i = 0; i < fetchedAttributes.Length; i++)
            {
                var largeInteger = fetchedAttributes[i] as IAdsLargeInteger;
                if (largeInteger != null)
                {
                    long l = (largeInteger.HighPart * 0x100000000L) + ((uint) largeInteger.LowPart);
                    fetchedAttributes[i] = l;
                }

                if (attributesToFetch[i].Equals("objectClass", StringComparison.OrdinalIgnoreCase)) // see comments in Initialize() function
                    fetchedAttributes[i] = schemaClassName;
            }

            return fetchedAttributes;
        }

        #endregion
    }
}
