using System;
using System.Runtime.InteropServices;

namespace Tulpep.ActiveDirectoryObjectPicker
{
    internal class PInvoke
	{
		/// <summary>
		/// The GlobalLock function locks a global memory object and returns a pointer to the first byte of the object's memory block.
		/// GlobalLock function increments the lock count by one.
		/// Needed for the clipboard functions when getting the data from IDataObject
		/// </summary>
		/// <param name="hMem"></param>
		/// <returns></returns>
		[DllImport("Kernel32.dll", SetLastError=true)]
		public static extern IntPtr GlobalLock(IntPtr hMem);

		/// <summary>
		/// The GlobalUnlock function decrements the lock count associated with a memory object.
		/// </summary>
		/// <param name="hMem"></param>
		/// <returns></returns>
		[DllImport("Kernel32.dll", SetLastError=true)]
		public static extern bool GlobalUnlock(IntPtr hMem);

        /// <summary>
        /// Frees the specified storage medium.
        /// </summary>
        /// <param name="pmedium">Pointer to the storage medium that is to be freed.</param>
        [DllImport("ole32.dll")]
        internal static extern void ReleaseStgMedium([In] ref STGMEDIUM pmedium);
	}
}
