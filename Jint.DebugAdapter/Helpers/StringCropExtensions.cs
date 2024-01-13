namespace Jint.DebugAdapter.Helpers;

public static class StringCropExtensions
{
	/// <summary>
	/// Crops end of string to make it fit a maximum length, including a separator (e.g. ellipsis).
	/// </summary>
	/// <remarks>
	/// Note that the string is not guaranteed to be cropped to exactly max length, if the string includes
	/// surrogate pairs (32-bit code points). In that case, it may be shorter, in order to not split a
	/// pair.
	/// </remarks>
	public static string CropEnd(this string str, int maxLength, string ellipsis = "…")
	{
		if (maxLength < ellipsis.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength,
				$"{nameof(maxLength)} should be >= length of {nameof(ellipsis)} (i.e. {ellipsis.Length}).");
		}
		if (str.Length <= maxLength)
		{
			return str;
		}
		int length = maxLength - ellipsis.Length;
		if (length <= 0)
		{
			return ellipsis;
		}
		if (Char.IsSurrogatePair(str, length - 1))
		{
			length--;
		}

		return string.Concat(str.AsSpan(0, length), ellipsis);
	}

	/// <summary>
	/// Crops middle of string to make it fit a maximum length, including a separator (e.g. ellipsis).
	/// </summary>
	/// <remarks>
	/// Note that the string is not guaranteed to be cropped to exactly max length, if the string includes
	/// surrogate pairs (32-bit code points). In that case, it may be shorter, in order to not split a
	/// pair.
	/// </remarks>
	public static string CropMiddle(this string str, int maxLength, string ellipsis = "…")
	{
		if (maxLength < ellipsis.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength,
				$"{nameof(maxLength)} should be >= length of {nameof(ellipsis)} (i.e. {ellipsis.Length}).");
		}
		if (str.Length <= maxLength)
		{
			return str;
		}

		if (maxLength == ellipsis.Length)
		{
			return ellipsis;
		}

		maxLength -= ellipsis.Length;
		int leftLength = maxLength / 2;
		int rightLength = maxLength - leftLength;

		// No space for anything except separator (or a single character on one side)
		if (rightLength <= 0 || leftLength <= 0)
		{
			return ellipsis;
		}

		// Ensure we're not splitting surrogate pairs:
		if (Char.IsSurrogatePair(str, str.Length - rightLength - 1))
		{
			leftLength++;
			rightLength--;
		}

		if (Char.IsSurrogatePair(str, leftLength - 1))
		{
			leftLength--;
		}

		return string.Concat(
			str.AsSpan(0, leftLength), 
			ellipsis, 
			str.AsSpan(str.Length - rightLength, rightLength)
		);
	}
}
