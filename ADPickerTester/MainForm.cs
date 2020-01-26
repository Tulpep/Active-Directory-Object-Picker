using System;
using System.Collections;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

using Tulpep.ActiveDirectoryObjectPicker;

namespace ADPickerTester
{
	/// <summary>Summary description for Form1.</summary>
	public partial class MainForm : Form
	{
		public MainForm() =>
			// Required for Windows Form Designer support
			InitializeComponent();

		/// <summary>The main entry point for the application.</summary>
		[STAThread]
		private static void Main() => Application.Run(new MainForm());

		private void btnInvoke_Click(object sender, EventArgs e)
		{
			try
			{
				var allowedTypes = ObjectTypes.None;
				foreach (ObjectTypes value in chklistAllowedTypes.CheckedItems)
				{
					allowedTypes |= value;
				}
				var defaultTypes = ObjectTypes.None;
				foreach (ObjectTypes value in chklistDefaultTypes.CheckedItems)
				{
					defaultTypes |= value;
				}
				var allowedLocations = Locations.None;
				foreach (Locations value in chklistAllowedLocations.CheckedItems)
				{
					allowedLocations |= value;
				}
				var defaultLocations = Locations.None;
				foreach (Locations value in chklistDefaultLocations.CheckedItems)
				{
					defaultLocations |= value;
				}
				// Show dialog
				var picker = new DirectoryObjectPickerDialog
				{
					AllowedObjectTypes = allowedTypes,
					DefaultObjectTypes = defaultTypes,
					AllowedLocations = allowedLocations,
					DefaultLocations = defaultLocations,
					MultiSelect = chkMultiSelect.Checked,
					TargetComputer = txtTargetComputer.Text,
					SkipDomainControllerCheck = chkSkipDcCheck.Checked,
					ShowAdvancedView = chkShowAdvanced.Checked
				};

				if (comboPathProvider.SelectedItem is ADsPathsProviders)
					picker.Providers = (ADsPathsProviders)comboPathProvider.SelectedItem;

				if (txUserName.TextLength > 0)
					picker.SetCredentials(txUserName.Text, txPassword.Text);

				foreach (string attribute in chklistAttributes.CheckedItems)
				{
					var trimmed = attribute.Trim();
					if (trimmed.Length > 0)
						picker.AttributesToFetch.Add(trimmed);
				}
				var dialogResult = picker.ShowDialog(this);
				if (dialogResult == DialogResult.OK)
				{
					var results = picker.SelectedObjects;
					if (results == null)
					{
						txtFeedback.Text = "Results null.";
						return;
					}

					var sb = new StringBuilder();

					for (var i = 0; i <= results.Length - 1; i++)
					{
						sb.Append(string.Format("Name: \t\t {0}", results[i].Name));
						sb.Append(Environment.NewLine);
						sb.Append(string.Format("UPN: \t\t {0}", results[i].Upn));
						sb.Append(Environment.NewLine);
						sb.Append(string.Format("Path: \t\t {0}", results[i].Path));
						sb.Append(Environment.NewLine);
						sb.Append(string.Format("Schema Class: \t\t {0} ", results[i].SchemaClassName));
						sb.Append(Environment.NewLine);
						var downLevelName = "";
						try
						{
							if (!string.IsNullOrEmpty(results[i].Upn))
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
						for (var j = 0; j < results[i].FetchedAttributes.Length; j++)
						{
							var attributeName = picker.AttributesToFetch[j];
							sb.AppendFormat("\t{0}. {1}", j, attributeName);
							sb.Append(Environment.NewLine);

							var multivaluedAttribute = results[i].FetchedAttributes[j];
							if (!(multivaluedAttribute is IEnumerable) || multivaluedAttribute is byte[] || multivaluedAttribute is string)
								multivaluedAttribute = new[] { multivaluedAttribute };

							foreach (var attribute in (IEnumerable)multivaluedAttribute)
							{
								sb.Append("\t");
								if (attribute == null)
								{
									sb.Append("(not present)");
								}
								else if (attribute is byte[])
								{
									var bytes = (byte[])attribute;
									if (attributeName.Equals("objectSid", StringComparison.OrdinalIgnoreCase))
									{
										sb.Append(SIDBytesToString(bytes));
									}
									else
									{
										sb.Append(GuidBytesToString(bytes));
									}
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
					txtFeedback.Text = sb.ToString();
				}
				else
				{
					txtFeedback.Text = "Dialog result: " + dialogResult;
				}
			}
			catch (Exception e1)
			{
				MessageBox.Show(e1.ToString());
			}
		}

		private string BytesToString(byte[] bytes)
		{
			return "0x" + BitConverter.ToString(bytes).Replace('-', ' ');
		}

		private string GuidBytesToString(byte[] bytes)
		{
			try
			{
				var guid = new Guid(bytes);
				return guid.ToString("D");
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch (Exception)
			{
			}

			return BytesToString(bytes);
		}

		private string SIDBytesToString(byte[] bytes)
		{
			try
			{
				var sid = new SecurityIdentifier(bytes, 0);
				return sid.ToString();
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch (Exception)
			{
			}

			return BytesToString(bytes);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			chklistAllowedTypes.Items.Clear();
			chklistDefaultTypes.Items.Clear();
			foreach (ObjectTypes objectType in Enum.GetValues(typeof(ObjectTypes)))
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
			chklistAttributes.Items.AddRange(new object[]
			{
				"objectSid",
			});
		}
	}
}