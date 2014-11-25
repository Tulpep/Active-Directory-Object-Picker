using System;

namespace Tulpep.ActiveDirectoryObjectPicker
{
    internal static class IntPtrExtensions
    {
        /// <summary>
        /// Adds an offset to the value of a pointer.
        /// </summary>
        /// <param name="pointer">The pointer to add the offset to.</param>
        /// <param name="offset">The offset to add.</param>
        /// <returns>A new pointer that reflects the addition of <paramref name="offset"/> to <paramref name="pointer"/>.</returns>
        public static IntPtr OffsetWith(this IntPtr pointer, long offset)
        {
            // On 64bits computer, we need ToInt64() to prevent exceptions when process is using more than int.MaxValue of memory.
            //
            // On 32bits computer, the use of ToInt64() has no effect except when the pointer moved by offset would make it past
            // the int.MaxValue barrier. We still need to fail in that case so let's make the IntPtr constructor fail with a 
            // meaningful error message.
            return new IntPtr(pointer.ToInt64() + offset);
        }
    }
}
