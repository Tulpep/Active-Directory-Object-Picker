Active Directory Object Picker [![Build status](https://ci.appveyor.com/api/projects/status/q5ttquqholl8oomi?svg=true)](https://ci.appveyor.com/project/Boardgent/active-directory-object-picker)

========================
### The standard Active Directory object picker dialog for .NET

![Screenshot](Screenshots/ADPickerTester.gif)


This project is based on a [Active Directory Common Dialogs .NET (ADUI)](https://adui.codeplex.com/) created in 2004 by Armand du Plessis. It has been updated and make it working with 64 bit Windows Editions.
Now it works in all .Net framework versions, including version 2.0, 3.5, 4.0, 4.5 and .NET Core.

### How to use it
You can install the latest version using [NuGet](https://www.nuget.org/packages/Tulpep.ActiveDirectoryObjectPicker/)
```powershell
Install-Package Tulpep.ActiveDirectoryObjectPicker
```

And use it this way:
```cs
DirectoryObjectPickerDialog picker = new DirectoryObjectPickerDialog()
{
    AllowedObjectTypes = ObjectTypes.Users | ObjectTypes.Groups | ObjectTypes.Computers,
    DefaultObjectTypes = ObjectTypes.Computers,
    AllowedLocations = Locations.All,
    DefaultLocations = Locations.JoinedDomain,
    MultiSelect = true,
    ShowAdvancedView = true
};
using (picker)
{
    if (picker.ShowDialog() == DialogResult.OK)
    {
        foreach (var sel in picker.SelectedObjects)
        {
            Console.WriteLine(sel.Name);
        }
    }
}
```
This repository contains a Visual Studio Test Project if you want a working example.
