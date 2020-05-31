using System.Collections.Generic;
using System.Threading;

public partial class ComputeShaderEmulator
{
    public static void InterlockedAdd(ref int dest, int value, out int original_value)
    {
        original_value = Interlocked.Add(ref dest, value) - value;
    }
    
    public static void InterlockedCompareExchange(ref int dest, int compare_value, int value, out int original_value)
    {
        original_value = Interlocked.CompareExchange( ref dest, value, compare_value );
    }
}