// Ignore Spelling: MIK АБВ ШЩЪЫЭЮЯ абв шщъыьэюя ABC yzAB XYZ АБВШЩЪЫЭЮЯ абвшщъыьэюя 거룩한

namespace MikEncodingTest;

public class Tests
{

    private static readonly Skyware.Text.Encoding.MIK mik = new();

    [Test]
    [TestCase("АБВ", ExpectedResult = new byte[] { 0x80, 0x81, 0x82, })]
    [TestCase("ШЩЪЫЭЮЯ", ExpectedResult = new byte[] { 0x98, 0x99, 0x9a, 0x9b, 0x9d, 0x9e, 0x9f, })]
    [TestCase("абв", ExpectedResult = new byte[] { 0xa0, 0xa1, 0xa2, })]
    [TestCase("шщъыьэюя", ExpectedResult = new byte[] { 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf, })]
    [TestCase("ABC", ExpectedResult = new byte[] { 0x41, 0x42, 0x43, })]
    [TestCase(@"YZ[\]^_`abc", ExpectedResult = new byte[] { 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, })]
    [TestCase("XYZ", ExpectedResult = new byte[] { 0x58, 0x59, 0x5a, })]
    [TestCase("░▒", ExpectedResult = new byte[] { 0x3f, 0x3f, })]
    [TestCase("거룩한", ExpectedResult = new byte[] { 0x3f, 0x3f, 0x3f, })]
    public byte[] TestBytes(string src)
    {
        return mik.GetBytes(src);
    }

    [Test]
    [TestCase(new byte[] { 0x80, 0x81, 0x82, 0x98, 0x99, 0x9a, 0x9b, 0x9d, 0x9e, 0x9f, }, ExpectedResult = "АБВШЩЪЫЭЮЯ")]
    [TestCase(new byte[] { 0xa0, 0xa1, 0xa2, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf, }, ExpectedResult = "абвшщъыьэюя")]
    [TestCase(new byte[] { 65, 66, 67, }, ExpectedResult = "ABC")]
    [TestCase(new byte[] { 91, 92, 93, 94, 95, 96, }, ExpectedResult = @"[\]^_`")]
    public string TestChars(byte[] src)
    {
        return new string(mik.GetChars(src));
    }


}