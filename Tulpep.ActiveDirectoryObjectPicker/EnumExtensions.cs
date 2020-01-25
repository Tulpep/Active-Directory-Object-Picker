using System;

namespace Tulpep.ActiveDirectoryObjectPicker
{
	internal static class EnumExtensions
	{
		/// <summary>Determines whether the enumerated flag value has the specified flag set.</summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <param name="flags">The enumerated flag value.</param>
		/// <param name="flag">The flag value to check.</param>
		/// <returns><c>true</c> if is flag set; otherwise, <c>false</c>.</returns>
		public static bool IsFlagSet<T>(this T flags, T flag) where T : struct, System.Enum
		{
			CheckHasFlags<T>();
			var flagValue = Convert.ToInt64(flag);
			return (Convert.ToInt64(flags) & flagValue) == flagValue;
		}

		/// <summary>Checks if <typeparamref name="T"/> represents an enumeration and throws an exception if not.</summary>
		/// <typeparam name="T">The <see cref="Type"/> to validate.</typeparam>
		/// <exception cref="System.ArgumentException"></exception>
		private static void CheckHasFlags<T>() where T : struct, System.Enum
		{
			if (!IsFlags<T>())
				throw new ArgumentException($"Type '{typeof(T).FullName}' doesn't have the 'Flags' attribute");
		}

		/// <summary>Determines whether this enumerations has the <see cref="FlagsAttribute"/> set.</summary>
		/// <typeparam name="T">The enumerated type.</typeparam>
		/// <returns><c>true</c> if this instance has the <see cref="FlagsAttribute"/> set; otherwise, <c>false</c>.</returns>
		private static bool IsFlags<T>() where T : struct, System.Enum => Attribute.IsDefined(typeof(T), typeof(FlagsAttribute));
	}
}