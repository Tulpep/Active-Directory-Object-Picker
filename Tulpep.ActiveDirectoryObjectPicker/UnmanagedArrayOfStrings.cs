using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tulpep.ActiveDirectoryObjectPicker
{
    // based on the packLPArray class.
    //   original from mailing list post by Beat Bucheli.
    //   or maybe from http://blogs.technolog.nl/eprogrammer/archive/2005/11/24/402.aspx
    //   or maybe from somewhere else..
    public sealed class UnmanagedArrayOfStrings : IDisposable
    {
        private readonly int _length;
	    private readonly IntPtr[] _unmanagedStrings;

        public UnmanagedArrayOfStrings(List<string> strings)
        {
            if (strings != null)
            {
                _length = strings.Count;
                _unmanagedStrings = new IntPtr[_length];
                int neededSize = _length*IntPtr.Size;
                ArrayPtr = Marshal.AllocCoTaskMem(neededSize);
                for (int cx = _length - 1; cx >= 0; cx--)
                {
                    _unmanagedStrings[cx] = Marshal.StringToCoTaskMemUni(strings[cx]);
                    Marshal.WriteIntPtr(ArrayPtr, cx*IntPtr.Size, _unmanagedStrings[cx]);
                }
            }
        }

        public IntPtr ArrayPtr { get; private set; }

	    public void Dispose()
        {
            if (ArrayPtr != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(ArrayPtr);
                ArrayPtr = IntPtr.Zero;
            }

            for (int cx = 0; cx < _length; cx++)
            {
                if (_unmanagedStrings[cx] != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(_unmanagedStrings[cx]);
                    _unmanagedStrings[cx] = IntPtr.Zero;
                }
            }
        }
    }
}