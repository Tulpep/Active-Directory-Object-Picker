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
        private IntPtr _unmanagedArray;
        private IntPtr[] _unmanagedStrings;

        public UnmanagedArrayOfStrings(List<string> strings)
        {
            int neededSize = 0;
            if (strings != null)
            {
                this._length = strings.Count;
                this._unmanagedStrings = new IntPtr[this._length];
                neededSize = this._length*IntPtr.Size;
                this._unmanagedArray = Marshal.AllocCoTaskMem(neededSize);
                for (int cx = this._length - 1; cx >= 0; cx--)
                {
                    this._unmanagedStrings[cx] = Marshal.StringToCoTaskMemUni(strings[cx]);
                    Marshal.WriteIntPtr(this._unmanagedArray, cx*IntPtr.Size, this._unmanagedStrings[cx]);
                }
            }
        }

        public IntPtr ArrayPtr
        {
            get { return this._unmanagedArray; }
        }

        public void Dispose()
        {
            if (_unmanagedArray != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(this._unmanagedArray);
                _unmanagedArray = IntPtr.Zero;
            }

            for (int cx = 0; cx < this._length; cx++)
            {
                if (this._unmanagedStrings[cx] != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(this._unmanagedStrings[cx]);
                    this._unmanagedStrings[cx] = IntPtr.Zero;
                }
            }
        }
    }
}