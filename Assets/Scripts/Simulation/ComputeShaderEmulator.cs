
using System.Threading;
using Types;
using UnityEngine;

#if UNITY_EDITOR_WIN
using System.Runtime.InteropServices;
#endif

public partial class ComputeShaderEmulator
{
    #region Performance
    #if UNITY_EDITOR_WIN
    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceFrequency(out long lpFrequency);
    #endif
    
    private static double Timestamp()
    {
        #if UNITY_EDITOR_WIN
            long freq;
            if (QueryPerformanceFrequency(out freq))
            {
                long time ;
                QueryPerformanceCounter(out time);
                double result = (double)(time);
                return result / freq;
            }
            else
            {
                return 0.0;
            }
        #else
            return 0.0;
        #endif
    }
    #endregion

    #region Multithreading
    private static int _numCPUThreads = 1;

    public static int NumCPUThreads
    {
        get { return _numCPUThreads; }
        set { _numCPUThreads = Mathf.Max( 1, value ); }
    }
    #endregion
    
    public static void Dispatch(ComputeShaderKernel kernel, uint threadGroupsX, uint threadGroupsY, uint threadGroupsZ)
    {
        uint3 numThreads = new uint3(1,1,1);

        bool hasNumThreads = kernel.Method.GetCustomAttributes(typeof(NumThreads), true).Length > 0;
        if (hasNumThreads)
        {
            NumThreads numThreadsAttribute = (NumThreads) kernel.Method.GetCustomAttributes(typeof(NumThreads), true)[0];
            if (numThreadsAttribute != null)
            {
                numThreads.x = (uint)(numThreadsAttribute.x);
                numThreads.y = (uint)(numThreadsAttribute.y);
                numThreads.z = (uint)(numThreadsAttribute.z);
            }
        }

        double t0 = Timestamp();
        
        if (NumCPUThreads == 1)
        {
            uint3 threadId = new uint3();
            uint3 groupId = new uint3();
            
            for (uint groupZ = 0; groupZ < threadGroupsZ; groupZ++)
            {
                groupId.z = groupZ;
                for (uint groupY = 0; groupY < threadGroupsY; groupY++)
                {
                    groupId.y = groupY;
                    for (uint groupX = 0; groupX < threadGroupsX; groupX++)                    
                    {
                        groupId.x = groupX;

                        for (uint threadZ = 0; threadZ < numThreads.z; threadZ++)
                        {
                            threadId.z = groupId.z * numThreads.z + threadZ;
                            for (uint threadY = 0; threadY < numThreads.y; threadY++)
                            {
                                threadId.y = groupId.y * numThreads.y + threadY;
                                for (uint threadX = 0; threadX < numThreads.x; threadX++)
                                {
                                    threadId.x = groupId.x * numThreads.x + threadX;
                                    kernel(threadId);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            System.Action<int> ThreadFunc = (cpuThreadId) =>
            {
                uint3 threadId = new uint3();
                uint3 groupId = new uint3();
                
                for (uint groupZ = 0; groupZ < threadGroupsZ; groupZ++)
                {
                    groupId.z = groupZ;
                    for (uint groupY = 0; groupY < threadGroupsY; groupY++)
                    {
                        groupId.y = groupY;
                        for (uint groupX = 0; groupX < threadGroupsX; groupX++)                        
                        {
                            groupId.x = groupX;

                            uint waveId = groupId.z * (uint) (threadGroupsX) * (uint) (threadGroupsY) +
                                          groupId.y * (uint) (threadGroupsX) +
                                          groupId.x;

                            if (waveId % NumCPUThreads == cpuThreadId)
                            {
                                for (uint threadZ = 0; threadZ < numThreads.z; threadZ++)
                                {
                                    threadId.z = groupId.z * numThreads.z + threadZ;
                                    for (uint threadY = 0; threadY < numThreads.y; threadY++)
                                    {
                                        threadId.y = groupId.y * numThreads.y + threadY;
                                        for (uint threadX = 0; threadX < numThreads.x; threadX++)
                                        {
                                            threadId.x = groupId.x * numThreads.x + threadX;
                                            kernel(threadId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }    
            };

            Thread[] threads = new Thread[NumCPUThreads];
            for (int i = 0; i < NumCPUThreads; i++)
            {
                int cpuThreadId = i; // closuring "i" to "cpuThreadId"
                threads[i] = new Thread(() => ThreadFunc(cpuThreadId));
                threads[i].Start();
            }
            for (int i = 0; i < NumCPUThreads; i++)
            {
                threads[i].Join();
            }
        }

        double t1 = Timestamp();
        
        Debug.Log( "[ComputeShaderEmulator] executed kernel " + kernel.Method.Name + " numThreads[" + numThreads.x + "," + numThreads.y + "," + numThreads.z + "] time " + (t1-t0).ToString("F6") + "sec." );
    }
}
