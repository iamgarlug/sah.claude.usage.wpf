namespace Sah.Claude.Usage.Wpf.UnitTests;

public class SampleTest
{
    [Test]
    public async Task Sample_PassingTest()
    {
        int result = 1 + 1;
        await Assert.That(result).IsEqualTo(2);
    }
}
