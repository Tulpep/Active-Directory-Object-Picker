using System;
using System.Collections;
using System.Security.Principal;
using System.Windows.Forms;
using Tulpep.ActiveDirectoryObjectPicker;

namespace ADPickerTester
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public partial class MainForm : Form
	{

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void btnInvoke_Click(object sender, System.EventArgs e)
		{
			try
			{
                ObjectTypes allowedTypes = ObjectTypes.None;
                foreach (ObjectTypes value in chklistAllowedTypes.CheckedItems)
                {
                    allowedTypes |= value;
                }
                ObjectTypes defaultTypes = ObjectTypes.None;
                foreach (ObjectTypes value in chklistDefaultTypes.CheckedItems)
                {
                    defaultTypes |= value;
                }
                Locations allowedLocations = Locations.None;
                foreach (Locations value in chklistAllowedLocations.CheckedItems)
                {
                    allowedLocations |= value;
                }
                Locations defaultLocations = Locations.None;
                foreach (Locations value in chklistDefaultLocations.CheckedItems)
                {
                    defaultLocations |= value;
                }
                // Show dialog
                DirectoryObjectPickerDialog picker = new DirectoryObjectPickerDialog();
                picker.AllowedObjectTypes = allowedTypes;
                picker.DefaultObjectTypes = defaultTypes;
                picker.AllowedLocations = allowedLocations;
                picker.DefaultLocations = defaultLocations;
                picker.MultiSelect = chkMultiSelect.Checked;
                picker.TargetComputer = txtTargetComputer.Text;
                picker.SkipDomainControllerCheck = chkSkipDcCheck.Checked;
				if (comboPathProvider.SelectedItem is ADsPathsProviders)
					picker.Providers = (ADsPathsProviders) comboPathProvider.SelectedItem;
                foreach (string attribute in chklistAttributes.CheckedItems)
                {
                    string trimmed = attribute.Trim();
                    if (trimmed.Length > 0)
                        picker.AttributesToFetch.Add(trimmed);
                }
                DialogResult dialogResult = picker.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DirectoryObject[] results;
                    results = picker.SelectedObjects;
                    if (results == null)
                    {
                        lblFeedback.Text = "Results null.";
                        return;
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    for (int i = 0; i <= results.Length - 1; i++)
                    {
                        sb.Append(string.Format("Name: \t\t {0}", results[i].Name));
                        sb.Append(Environment.NewLine);
                        sb.Append(string.Format("UPN: \t\t {0}", results[i].Upn));
                        sb.Append(Environment.NewLine);
                        sb.Append(string.Format("Path: \t\t {0}", results[i].Path));
                        sb.Append(Environment.NewLine);
                        sb.Append(string.Format("Schema Class: \t\t {0} ", results[i].SchemaClassName));
                        sb.Append(Environment.NewLine);
                        string downLevelName = "";
                        try
                        {
                            if (!String.IsNullOrEmpty(results[i].Upn))
                                downLevelName = NameTranslator.TranslateUpnToDownLevel(results[i].Upn);
                        }
                        catch (Exception ex)
                        {
                            downLevelName = string.Format("{0}: {1}", ex.GetType().Name, ex.Message);
                        }
                        sb.Append(string.Format("Down-level Name: \t\t {0} ", downLevelName));
                        sb.Append(Environment.NewLine);
						sb.AppendFormat("Fetched {0} attributes", results[i].FetchedAttributes.Length);
						sb.Append(Environment.NewLine);
                        for (int j = 0; j < results[i].FetchedAttributes.Length; j++)
                        {
                            sb.AppendFormat("\t{0}. {1}", j, picker.AttributesToFetch[j]);
							sb.Append(Environment.NewLine);

                            object multivaluedAttribute = results[i].FetchedAttributes[j];
                            if (!(multivaluedAttribute is IEnumerable) || multivaluedAttribute is byte[] || multivaluedAttribute is string)
                                multivaluedAttribute = new Object[1] { multivaluedAttribute };
                            
                            foreach (object attribute in (IEnumerable) multivaluedAttribute)
                            {
                                sb.Append("\t");
							    if (attribute == null)
							    {
								    sb.Append("(not present)");
							    }
                                else if (attribute is byte[])
                                {
                                    byte[] bytes = (byte[]) attribute;
                                    sb.Append(BytesToString(bytes));
                                }
                                else
                                {
                                    sb.AppendFormat("{0}", attribute);
                                }
							    sb.Append(Environment.NewLine);
                            }

                            sb.Append(Environment.NewLine);
                        }

                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                    }
                    lblFeedback.Text = sb.ToString();
                }
                else
                {
                    lblFeedback.Text = "Dialog result: " + dialogResult.ToString();
                }
			}
			catch(Exception e1)
			{				
				MessageBox.Show(e1.ToString());
			}
		}

        private string BytesToString(byte[] bytes)
        {
            try
            {
                Guid guid = new Guid(bytes);
                return guid.ToString("D");
            }
            catch (Exception)
            {
            }

            try
            {
                SecurityIdentifier sid = new SecurityIdentifier(bytes, 0);
                return sid.ToString();
            }
            catch (Exception)
            {
            }

            return "0x" + BitConverter.ToString(bytes).Replace('-', ' ');
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chklistAllowedTypes.Items.Clear();
            chklistDefaultTypes.Items.Clear();
            foreach(ObjectTypes objectType in Enum.GetValues(typeof(ObjectTypes)))
            {
                if (objectType != ObjectTypes.None && objectType != ObjectTypes.All)
                {
                    chklistAllowedTypes.Items.Add(objectType, CheckState.Checked);
                    if (objectType == ObjectTypes.Users || objectType == ObjectTypes.Groups 
                        || objectType == ObjectTypes.Computers || objectType == ObjectTypes.Contacts)
                    {
                        chklistDefaultTypes.Items.Add(objectType, CheckState.Checked);
                    }
                    else
                    {
                        chklistDefaultTypes.Items.Add(objectType, CheckState.Unchecked);
                    }
                }
            }

            chklistAllowedLocations.Items.Clear();
            chklistDefaultLocations.Items.Clear();
            foreach (Locations location in Enum.GetValues(typeof(Locations)))
            {
                if (location != Locations.None && location != Locations.All)
                {
                    chklistAllowedLocations.Items.Add(location, CheckState.Checked);
                    if (location == Locations.JoinedDomain)
                    {
                        chklistDefaultLocations.Items.Add(location, CheckState.Checked);
                    }
                    else
                    {
                        chklistDefaultLocations.Items.Add(location, CheckState.Unchecked);
                    }
                }
            }

			comboPathProvider.Items.Clear();
			foreach (ADsPathsProviders provider in Enum.GetValues(typeof(ADsPathsProviders)))
			{
				comboPathProvider.Items.Add(provider);
			}
			comboPathProvider.SelectedIndex = 0;

            chklistAttributes.Items.Clear();
            //to find more, go to http://msdn.microsoft.com/en-us/library/cc219752.aspx, http://msdn.microsoft.com/en-us/library/cc220155.aspx and http://msdn.microsoft.com/en-us/library/cc220700.aspx
            chklistAttributes.Items.AddRange(new []
            {
                "objectSid",
            });
        }

	}
}
