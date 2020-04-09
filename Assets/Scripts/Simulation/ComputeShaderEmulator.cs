using Types;
using UnityEngine;


public partial class Simulation
{
    public static void Dispatch(ComputeShaderKernel kernel, uint threadGroupsX, uint threadGroupsY, uint threadGroupsZ)
    {
        uint3 numThreads = new uint3();
        numThreads.x = 1;
        numThreads.y = 1;
        numThreads.z = 1;

        bool hadNumThreads = kernel.Method.GetCustomAttributes(typeof(NumThreads), true).Length > 0;
        if (hadNumThreads)
        {
            NumThreads numThreadsAttribute = (NumThreads) kernel.Method.GetCustomAttributes(typeof(NumThreads), true)[0];
            if (numThreadsAttribute != null)
            {
                numThreads.x = (uint)(numThreadsAttribute.x);
                numThreads.y = (uint)(numThreadsAttribute.y);
                numThreads.z = (uint)(numThreadsAttribute.z);
            }
        }
        
        Debug.Log( "[ComputeShaderEmulator] running kernel " + kernel.Method.Name + " numThreads[" + numThreads.x + "," + numThreads.y + "," + numThreads.z + "]" );
        
        uint3 threadId = new uint3();
        uint3 groupId = new uint3();

        for (uint groupX = 0; groupX < threadGroupsX; groupX++)
        {
            for (uint groupY = 0; groupY < threadGroupsY; groupY++)
            {
                for (uint groupZ = 0; groupZ < threadGroupsZ; groupZ++)
                {
                    groupId.x = groupX;
                    groupId.y = groupY;
                    groupId.z = groupZ;
                    
                    for (uint threadX = 0; threadX < numThreads.x; threadX++)
                    {
                        for (uint threadY = 0; threadY < numThreads.y; threadY++)
                        {
                            for (uint threadZ = 0; threadZ < numThreads.z; threadZ++)
                            {
                                threadId.x = groupId.x * numThreads.x + threadX;
                                threadId.y = groupId.y * numThreads.y + threadY;
                                threadId.z = groupId.z * numThreads.z + threadZ;
                                
                                kernel(threadId);
                            }
                        }
                    }
                }
            }
        }
    }
}
