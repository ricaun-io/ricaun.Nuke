class Module
{
#pragma warning disable CA2255 // The 'ModuleInitializer' attribute should not be used in libraries
    [System.Runtime.CompilerServices.ModuleInitializer]
#pragma warning restore CA2255 // The 'ModuleInitializer' attribute should not be used in libraries
    internal static void Initialize()
    {
        System.AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);
    }
}
