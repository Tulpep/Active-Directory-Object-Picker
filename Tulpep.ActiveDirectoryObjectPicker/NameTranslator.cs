using System;
using System.Collections.Generic;
using System.Text;

namespace Tulpep.ActiveDirectoryObjectPicker
{
    /// <summary>
    /// Active Directory name translation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Translates names between Active Directory formats, e.g. from down-level NT4 
    /// style names ("ACME\alice") to User Principal Name ("alice@acme.com").
    /// </para>
    /// <para>
    /// This utility class encapsulates the ActiveDs.dll COM library.
    /// </para>
    /// </remarks>
    public static class NameTranslator
    {
        const int NameTypeUpn = (int) ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_USER_PRINCIPAL_NAME;
        const int NameTypeNt4 = (int) ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_NT4;
        const int NameTypeDn  = (int) ADS_NAME_TYPE_ENUM.ADS_NAME_TYPE_1779;

        /// <summary>
        /// Convert from a down-level NT4 style name to an Active Directory User Principal Name (UPN).
        /// </summary>
        public static string TranslateDownLevelToUpn(string downLevelNt4Name)
        {
            if (downLevelNt4Name == null) throw new ArgumentNullException("downLevelNt4Name");
            if (downLevelNt4Name.Length == 0) throw new ArgumentOutOfRangeException("downLevelNt4Name", "downLevelNt4Name is empty");

            string userPrincipalName;
            IADsNameTranslate nt = (IADsNameTranslate) new NameTranslate();
            //ActiveDs.NameTranslate nt = new ActiveDs.NameTranslate();
            nt.Set(NameTypeNt4, downLevelNt4Name);
            userPrincipalName = nt.Get(NameTypeUpn);
            return userPrincipalName;
        }

        /// <summary>
        /// Convert from an Active Directory User Principal Name (UPN) to a down-level NT4 style name.
        /// </summary>
        public static string TranslateUpnToDownLevel(string userPrincipalName)
        {
            if (userPrincipalName == null) throw new ArgumentNullException("userPrincipalName");
            if (userPrincipalName.Length == 0) throw new ArgumentOutOfRangeException("userPrincipalName", "userPrincipalName is empty");

            string downLevelName;
            IADsNameTranslate nt = (IADsNameTranslate) new NameTranslate();
            //ActiveDs.NameTranslate nt = new ActiveDs.NameTranslate();
            nt.Set(NameTypeUpn, userPrincipalName);
            downLevelName = nt.Get(NameTypeNt4);
            return downLevelName;
        }
    }
}
