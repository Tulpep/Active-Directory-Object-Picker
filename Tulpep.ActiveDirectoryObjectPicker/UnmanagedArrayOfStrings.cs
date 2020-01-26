using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tulpep.ActiveDirectoryObjectPicker
{
	// based on the packLPArray class. original from mailing list post by Beat Bucheli. or maybe from
	// http://blogs.technolog.nl/eprogrammer/archive/2005/11/24/402.aspx or maybe from somewhere else..
	/// <summary>A packed array of strings.</summary>
	/// <seealso cref="System.IDisposable"/>
	public sealed class UnmanagedArrayOfStrings : IDisposable
	{
		private readonly IntPtr[] _unmanagedStrings;

		/// <summary>Initializes a new instance of the <see cref="UnmanagedArrayOfStrings"/> class.</summary>
		/// <param name="strings">The strings to pack.</param>
		public UnmanagedArrayOfStrings(IList<string> strings)
		{
			if (strings != null)
			{
				var length = strings.Count;
				_unmanagedStrings = new IntPtr[length];
				var neededSize = length * IntPtr.Size;
				ArrayPtr = Marshal.AllocCoTaskMem(neededSize);
				for (var cx = length - 1; cx >= 0; cx--)
				{
					_unmanagedStrings[cx] = Marshal.StringToCoTaskMemUni(strings[cx]);
					Marshal.WriteIntPtr(ArrayPtr, cx * IntPtr.Size, _unmanagedStrings[cx]);
				}
			}
		}

		/// <summary>Gets the pointer to the packed array memory.</summary>
		public IntPtr ArrayPtr { get; private set; }

		/// <summary>Releases unmanaged and - optionally - managed resources.</summary>
		public void Dispose()
		{
			if (ArrayPtr != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(ArrayPtr);
				ArrayPtr = IntPtr.Zero;
			}

			foreach (var ptr in _unmanagedStrings)
			{
				Marshal.FreeCoTaskMem(ptr);
			}
		}
	}
}