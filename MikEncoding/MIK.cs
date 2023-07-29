// Ignore Spelling: MIK

using System;
using System.Linq;
using System.Reflection;

namespace Skyware.Text.Encoding;

/// <summary>
/// MIK (МИК) is an 8-bit Cyrillic code page used with DOS and many hardware devices.
/// </summary>
public class MIK : System.Text.Encoding
{

    private const int UC_BASE = 0x390;
    private const int UC_OFFSET = 0x410;
    private const int MIK_BASE = 0x80;

    #region Properties

    /// <inheritdoc/>
    public override bool IsSingleByte => true;

    /// <inheritdoc/>
    public override string BodyName => "MIK";

    /// <inheritdoc/>
    public override int CodePage => 9999;

    /// <inheritdoc/>
    public override string EncodingName => "MIK Encoding (SKYWARE Group)";

    /// <inheritdoc/>
    public override string HeaderName => string.Empty;

    /// <inheritdoc/>
    public override bool IsBrowserSave => false;

    /// <inheritdoc/>
    public override bool IsMailNewsDisplay => false;

    /// <inheritdoc/>
    public override bool IsMailNewsSave => false;

    /// <inheritdoc/>
    public override string WebName => string.Empty;

    /// <inheritdoc/>
    public override int WindowsCodePage => 9999;

    #endregion

    #region Methods

    /// <inheritdoc/>
    public override int GetByteCount(char[] chars, int index, int count)
    {
        if (chars is null) throw new ArgumentNullException(nameof(chars));
        if (index < 0) throw new ArgumentNullException(nameof(index));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (index + count > chars.Length) throw new ArgumentOutOfRangeException(nameof(count));
        return count;
    }

    /// <inheritdoc/>
    public override int GetCharCount(byte[] bytes, int index, int count)
    {
        if (bytes is null) throw new ArgumentNullException(nameof(bytes));
        if (index < 0) throw new ArgumentNullException(nameof(index));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (index + count > bytes.Length) throw new ArgumentOutOfRangeException(nameof(count));
        return count;
    }

    /// <inheritdoc/>
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        if (chars is null) throw new ArgumentNullException(nameof(chars));
        if (charIndex < 0) throw new ArgumentOutOfRangeException(nameof(charIndex));
        if (charCount < 0) throw new ArgumentOutOfRangeException(nameof(charCount));
        if (bytes is null) throw new ArgumentNullException(nameof(bytes));
        if (byteIndex < 0) throw new ArgumentOutOfRangeException(nameof(byteIndex));
        if (charIndex + charCount > chars.Length) throw new ArgumentOutOfRangeException(nameof(charCount));

        byte[] ucBytes = Unicode.GetBytes(chars.Skip(charIndex).Take(charCount).ToArray());

        for (int ix = 0; ix < charCount * 2; ix += 2)
        {
            int charCode = (ucBytes[ix + 1] << 8) + ucBytes[ix];
            if (charCode <= 0x7F)
                // 1 - 127 (7F) 'Special chars and English characters
                bytes[byteIndex + (ix / 2)] = (byte)charCode;
            else if (charCode >= UC_OFFSET && charCode <= UC_OFFSET + 64)
                // 128 (80) - 191 (BF) Cyrillic characters
                bytes[charIndex + (ix / 2)] = (byte)(MIK_BASE + (charCode - UC_OFFSET)); 
            else
                bytes[byteIndex + (ix / 2)] = 0x3f; // = '?'
        }
        // TODO: Translate 192-255

        return charCount;

    }

    /// <inheritdoc/>
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
        if (bytes is null) throw new ArgumentNullException(nameof(bytes));
        if (byteIndex < 0) throw new ArgumentOutOfRangeException(nameof(byteIndex));
        if (byteCount < 0) throw new ArgumentOutOfRangeException(nameof(byteCount));
        if (chars is null) throw new ArgumentNullException(nameof(chars));
        if (charIndex < 0) throw new ArgumentOutOfRangeException(nameof(charIndex));
        if (byteIndex + byteCount > bytes.Length) throw new ArgumentOutOfRangeException(nameof(byteCount));

        for (int ix = 0; ix < byteCount; ix++)

            if (bytes[byteIndex + ix] <= 0x7F)
                // 1 - 127 (7F) 'Special chars and English characters
                chars[charIndex + ix] = (char)bytes[byteIndex + ix];
            else if (bytes[byteIndex + ix] >= 0x80 && bytes[byteIndex + ix] <= 0xBF)
                // 128 (80) - 191 (BF) Cyrillic characters
                chars[charIndex + ix] = (char)(UC_BASE + bytes[byteIndex + ix]);
            else
                chars[charIndex + ix] = '?';

        // TODO: Translate 192-255

        return byteCount;
    }


    /// <inheritdoc/>
    public override int GetMaxByteCount(int charCount) => charCount;

    /// <inheritdoc/>
    public override int GetMaxCharCount(int byteCount) => byteCount;

    #endregion

}
