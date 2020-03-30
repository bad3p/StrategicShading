using System.Net.Sockets;
using Types;
using UnityEngine;

public partial class Simulation
{
    public static void Dispatch(ComputeShaderKernel kernel, uint threadGroupSize, uint threadGroupsX, uint threadGroupsY, uint threadGroupsZ)
    {
        uint3 idrange = new uint3(
            threadGroupsX <= 1 ? 1 : threadGroupsX * threadGroupSize,
            threadGroupsY <= 1 ? 1 : threadGroupsY * threadGroupSize,
            threadGroupsZ <= 1 ? 1 : threadGroupsZ * threadGroupSize
        );  
        
        uint3 id = new uint3();
        while (true)
        {
            kernel(id);

            id.x++;
            if (id.x >= idrange.x)
            {
                id.y++;
                if (id.y >= idrange.y)
                {
                    id.z++;
                    if (id.z >= idrange.z)
                    {
                        break;
                    }
                    else
                    {
                        id.y = 0;
                        id.x = 0;
                    }
                }
                else
                {
                    id.x = 0;
                }
            }
        }
    }
}
