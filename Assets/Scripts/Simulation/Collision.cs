using Types;

public partial class ComputeShaderEmulator
{
    private const int RIGHT = 0;
    private const int LEFT = 1;
    private const int MIDDLE = 2;

    public static bool CollideRayAABB(float3 origin, float3 dir, float3 minB, float3 maxB, out float3 hit)
    {
        bool inside = true;
        int3 quadrant = new int3();
        int whichPlane;
        float3 maxT = new float3();
        float3 candidatePlane = new float3();
        hit = origin;
        
        // Find candidate planes

        if (origin.x < minB.x) 
        {
            quadrant.x = LEFT;
            candidatePlane.x = minB.x;
            inside = false;
        }
        else if (origin.x > maxB.x) 
        {
            quadrant.x = RIGHT;
            candidatePlane.x = maxB.x;
            inside = false;
        }
        else
        {
            quadrant.x = MIDDLE;
        }
        
        if (origin.y < minB.y) 
        {
            quadrant.y = LEFT;
            candidatePlane.y = minB.y;
            inside = false;
        }
        else if (origin.y > maxB.y) 
        {
            quadrant.y = RIGHT;
            candidatePlane.y = maxB.y;
            inside = false;
        }
        else
        {
            quadrant.y = MIDDLE;
        }
        
        if (origin.z < minB.z) 
        {
            quadrant.z = LEFT;
            candidatePlane.z = minB.z;
            inside = false;
        }
        else if (origin.z > maxB.z) 
        {
            quadrant.z = RIGHT;
            candidatePlane.z = maxB.z;
            inside = false;
        }
        else
        {
            quadrant.z = MIDDLE;
        }


        if (inside)	
        {
            return true;
        }
        
        // Calculate T distances to candidate planes

        if (quadrant.x != MIDDLE && abs(dir.x) > DOUBLE_EPSILON)
        {
            maxT.x = (candidatePlane.x - origin.x) / dir.x;
        }
        else
        {
            maxT.x = -1.0f;
        }
        
        if (quadrant.y != MIDDLE && abs(dir.y) > DOUBLE_EPSILON)
        {
            maxT.y = (candidatePlane.y - origin.y) / dir.y;
        }
        else
        {
            maxT.y = -1.0f;
        }
        
        if (quadrant.z != MIDDLE && abs(dir.z) > DOUBLE_EPSILON)
        {
            maxT.z = (candidatePlane.z - origin.z) / dir.z;
        }
        else
        {
            maxT.z = -1.0f;
        }

        // Get largest of the maxT's for final choice of intersection
        
        whichPlane = 0;

        if (maxT.x < maxT.y)
        {
            whichPlane = 1;
            if (maxT.y < maxT.z)
            {
                whichPlane = 2;
            }
        }
        else if (maxT.x < maxT.z)
        {
            whichPlane = 2;
        }

        // Check final candidate actually inside box 
        
        if (whichPlane == 0 && maxT.x < 0.0f) return false;
        if (whichPlane == 1 && maxT.y < 0.0f) return false;
        if (whichPlane == 2 && maxT.z < 0.0f) return false;
        
        float hitDist = (whichPlane == 0) ? maxT.x : (whichPlane == 1 ? maxT.y : maxT.z);
        
        if (whichPlane != 0 ) 
        {
            hit.x = origin.x + hitDist * dir.x;
            if (hit.x < minB.x || hit.x > maxB.x)
            {
                return false;
            }
        } 
        else 
        {
            hit.x = candidatePlane.x;
        }
        
        if (whichPlane != 1 ) 
        {
            hit.y = origin.y + hitDist * dir.y;
            if (hit.y < minB.y || hit.y > maxB.y)
            {
                return false;
            }
        } 
        else 
        {
            hit.y = candidatePlane.y;
        }
        
        if (whichPlane != 2 ) 
        {
            hit.z = origin.z + hitDist * dir.z;
            if (hit.z < minB.z || hit.z > maxB.z)
            {
                return false;
            }
        } 
        else 
        {
            hit.z = candidatePlane.z;
        }

        return true;
    }
}
