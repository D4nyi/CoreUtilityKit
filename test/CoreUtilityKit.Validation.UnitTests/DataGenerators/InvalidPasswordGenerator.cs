namespace CoreUtilityKit.Validation.UnitTests.DataGenerators;

internal sealed class InvalidPasswordGenerator : TheoryData<string?>
{
    public InvalidPasswordGenerator()
    {
        Add(null);
        Add("");
        Add(" ");
        Add("\r");
        Add("\t");
        Add("\n");
        Add("\r\n");
        Add("short");
        Add("NotMatching");
        Add("LongEnough ButHasASpace");
        Add("!N7qXM3Wa2HLX5komTIUiSyhlhNRIqSA5G1fo2XO136ICYs0qSHkfIRXWsD2GdW4CVNaf1mntZr2rfpfOi4EL4WxCjhssoRt0PiurYAxM8IFYdSi3z8MYmskGuuIz8bXE");
    }
}
