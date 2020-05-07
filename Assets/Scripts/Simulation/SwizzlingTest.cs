using Types;
using UnityEngine;

public class SwizzlingTest : MonoBehaviour
{
    bool AreEqual(float2 v0, float2 v1)
    {
        return Mathf.Abs(v0.x - v1.x) <= Mathf.Epsilon &&
               Mathf.Abs(v0.y - v1.y) <= Mathf.Epsilon;
    }
    
    bool AreEqual(float3 v0, float3 v1)
    {
        return Mathf.Abs(v0.x - v1.x) <= Mathf.Epsilon &&
               Mathf.Abs(v0.y - v1.y) <= Mathf.Epsilon &&
               Mathf.Abs(v0.z - v1.z) <= Mathf.Epsilon;
    }
    
    bool AreEqual(float4 v0, float4 v1)
    {
        return Mathf.Abs(v0.x - v1.x) <= Mathf.Epsilon &&
               Mathf.Abs(v0.y - v1.y) <= Mathf.Epsilon &&
               Mathf.Abs(v0.z - v1.z) <= Mathf.Epsilon &&
               Mathf.Abs(v0.w - v1.w) <= Mathf.Epsilon;
    }
    
    void TestSwizzling()
    {
        const float X = 1;
        const float Y = 2;
        const float Z = 3;
        const float W = 5;
        
        float2 xx = new float2( X, X );
        float2 xy = new float2( X, Y );
        float2 xz = new float2( X, Z );
        float2 xw = new float2( X, W );
        float2 yx = new float2( Y, X );
        float2 yy = new float2( Y, Y );
        float2 yz = new float2( Y, Z );
        float2 yw = new float2( Y, W );
        float2 zx = new float2( Z, X );
        float2 zy = new float2( Z, Y );
        float2 zz = new float2( Z, Z );
        float2 zw = new float2( Z, W );
        float2 wx = new float2( W, X );
        float2 wy = new float2( W, Y );
        float2 wz = new float2( W, Z );
        float2 ww = new float2( W, W );
        
        float3 xxx = new float3( X, X, X );
        float3 xxy = new float3( X, X, Y );
        float3 xxz = new float3( X, X, Z );
        float3 xxw = new float3( X, X, W );
        float3 xyx = new float3( X, Y, X );
        float3 xyy = new float3( X, Y, Y );
        float3 xyz = new float3( X, Y, Z );
        float3 xyw = new float3( X, Y, W );
        float3 xzx = new float3( X, Z, X );
        float3 xzy = new float3( X, Z, Y );
        float3 xzz = new float3( X, Z, Z );
        float3 xzw = new float3( X, Z, W );
        float3 xwx = new float3( X, W, X );
        float3 xwy = new float3( X, W, Y );
        float3 xwz = new float3( X, W, Z );
        float3 xww = new float3( X, W, W );
        float3 yxx = new float3( Y, X, X );
        float3 yxy = new float3( Y, X, Y );
        float3 yxz = new float3( Y, X, Z );
        float3 yxw = new float3( Y, X, W );
        float3 yyx = new float3( Y, Y, X );
        float3 yyy = new float3( Y, Y, Y );
        float3 yyz = new float3( Y, Y, Z );
        float3 yyw = new float3( Y, Y, W );
        float3 yzx = new float3( Y, Z, X );
        float3 yzy = new float3( Y, Z, Y );
        float3 yzz = new float3( Y, Z, Z );
        float3 yzw = new float3( Y, Z, W );
        float3 ywx = new float3( Y, W, X );
        float3 ywy = new float3( Y, W, Y );
        float3 ywz = new float3( Y, W, Z );
        float3 yww = new float3( Y, W, W );
        float3 zxx = new float3( Z, X, X );
        float3 zxy = new float3( Z, X, Y );
        float3 zxz = new float3( Z, X, Z );
        float3 zxw = new float3( Z, X, W );
        float3 zyx = new float3( Z, Y, X );
        float3 zyy = new float3( Z, Y, Y );
        float3 zyz = new float3( Z, Y, Z );
        float3 zyw = new float3( Z, Y, W );
        float3 zzx = new float3( Z, Z, X );
        float3 zzy = new float3( Z, Z, Y );
        float3 zzz = new float3( Z, Z, Z );
        float3 zzw = new float3( Z, Z, W );
        float3 zwx = new float3( Z, W, X );
        float3 zwy = new float3( Z, W, Y );
        float3 zwz = new float3( Z, W, Z );
        float3 zww = new float3( Z, W, W );
        float3 wxx = new float3( W, X, X );
        float3 wxy = new float3( W, X, Y );
        float3 wxz = new float3( W, X, Z );
        float3 wxw = new float3( W, X, W );
        float3 wyx = new float3( W, Y, X );
        float3 wyy = new float3( W, Y, Y );
        float3 wyz = new float3( W, Y, Z );
        float3 wyw = new float3( W, Y, W );
        float3 wzx = new float3( W, Z, X );
        float3 wzy = new float3( W, Z, Y );
        float3 wzz = new float3( W, Z, Z );
        float3 wzw = new float3( W, Z, W );
        float3 wwx = new float3( W, W, X );
        float3 wwy = new float3( W, W, Y );
        float3 wwz = new float3( W, W, Z );
        float3 www = new float3( W, W, W );
        
        float4 xxxx = new float4( X, X, X, X );
        float4 xxxy = new float4( X, X, X, Y );
        float4 xxxz = new float4( X, X, X, Z );
        float4 xxxw = new float4( X, X, X, W );
        float4 xxyx = new float4( X, X, Y, X );
        float4 xxyy = new float4( X, X, Y, Y );
        float4 xxyz = new float4( X, X, Y, Z );
        float4 xxyw = new float4( X, X, Y, W );
        float4 xxzx = new float4( X, X, Z, X );
        float4 xxzy = new float4( X, X, Z, Y );
        float4 xxzz = new float4( X, X, Z, Z );
        float4 xxzw = new float4( X, X, Z, W );
        float4 xxwx = new float4( X, X, W, X );
        float4 xxwy = new float4( X, X, W, Y );
        float4 xxwz = new float4( X, X, W, Z );
        float4 xxww = new float4( X, X, W, W );
        float4 xyxx = new float4( X, Y, X, X );
        float4 xyxy = new float4( X, Y, X, Y );
        float4 xyxz = new float4( X, Y, X, Z );
        float4 xyxw = new float4( X, Y, X, W );
        float4 xyyx = new float4( X, Y, Y, X );
        float4 xyyy = new float4( X, Y, Y, Y );
        float4 xyyz = new float4( X, Y, Y, Z );
        float4 xyyw = new float4( X, Y, Y, W );
        float4 xyzx = new float4( X, Y, Z, X );
        float4 xyzy = new float4( X, Y, Z, Y );
        float4 xyzz = new float4( X, Y, Z, Z );
        float4 xyzw = new float4( X, Y, Z, W );
        float4 xywx = new float4( X, Y, W, X );
        float4 xywy = new float4( X, Y, W, Y );
        float4 xywz = new float4( X, Y, W, Z );
        float4 xyww = new float4( X, Y, W, W );
        float4 xzxx = new float4( X, Z, X, X );
        float4 xzxy = new float4( X, Z, X, Y );
        float4 xzxz = new float4( X, Z, X, Z );
        float4 xzxw = new float4( X, Z, X, W );
        float4 xzyx = new float4( X, Z, Y, X );
        float4 xzyy = new float4( X, Z, Y, Y );
        float4 xzyz = new float4( X, Z, Y, Z );
        float4 xzyw = new float4( X, Z, Y, W );
        float4 xzzx = new float4( X, Z, Z, X );
        float4 xzzy = new float4( X, Z, Z, Y );
        float4 xzzz = new float4( X, Z, Z, Z );
        float4 xzzw = new float4( X, Z, Z, W );
        float4 xzwx = new float4( X, Z, W, X );
        float4 xzwy = new float4( X, Z, W, Y );
        float4 xzwz = new float4( X, Z, W, Z );
        float4 xzww = new float4( X, Z, W, W );
        float4 xwxx = new float4( X, W, X, X );
        float4 xwxy = new float4( X, W, X, Y );
        float4 xwxz = new float4( X, W, X, Z );
        float4 xwxw = new float4( X, W, X, W );
        float4 xwyx = new float4( X, W, Y, X );
        float4 xwyy = new float4( X, W, Y, Y );
        float4 xwyz = new float4( X, W, Y, Z );
        float4 xwyw = new float4( X, W, Y, W );
        float4 xwzx = new float4( X, W, Z, X );
        float4 xwzy = new float4( X, W, Z, Y );
        float4 xwzz = new float4( X, W, Z, Z );
        float4 xwzw = new float4( X, W, Z, W );
        float4 xwwx = new float4( X, W, W, X );
        float4 xwwy = new float4( X, W, W, Y );
        float4 xwwz = new float4( X, W, W, Z );
        float4 xwww = new float4( X, W, W, W );
        float4 yxxx = new float4( Y, X, X, X );
        float4 yxxy = new float4( Y, X, X, Y );
        float4 yxxz = new float4( Y, X, X, Z );
        float4 yxxw = new float4( Y, X, X, W );
        float4 yxyx = new float4( Y, X, Y, X );
        float4 yxyy = new float4( Y, X, Y, Y );
        float4 yxyz = new float4( Y, X, Y, Z );
        float4 yxyw = new float4( Y, X, Y, W );
        float4 yxzx = new float4( Y, X, Z, X );
        float4 yxzy = new float4( Y, X, Z, Y );
        float4 yxzz = new float4( Y, X, Z, Z );
        float4 yxzw = new float4( Y, X, Z, W );
        float4 yxwx = new float4( Y, X, W, X );
        float4 yxwy = new float4( Y, X, W, Y );
        float4 yxwz = new float4( Y, X, W, Z );
        float4 yxww = new float4( Y, X, W, W );
        float4 yyxx = new float4( Y, Y, X, X );
        float4 yyxy = new float4( Y, Y, X, Y );
        float4 yyxz = new float4( Y, Y, X, Z );
        float4 yyxw = new float4( Y, Y, X, W );
        float4 yyyx = new float4( Y, Y, Y, X );
        float4 yyyy = new float4( Y, Y, Y, Y );
        float4 yyyz = new float4( Y, Y, Y, Z );
        float4 yyyw = new float4( Y, Y, Y, W );
        float4 yyzx = new float4( Y, Y, Z, X );
        float4 yyzy = new float4( Y, Y, Z, Y );
        float4 yyzz = new float4( Y, Y, Z, Z );
        float4 yyzw = new float4( Y, Y, Z, W );
        float4 yywx = new float4( Y, Y, W, X );
        float4 yywy = new float4( Y, Y, W, Y );
        float4 yywz = new float4( Y, Y, W, Z );
        float4 yyww = new float4( Y, Y, W, W );
        float4 yzxx = new float4( Y, Z, X, X );
        float4 yzxy = new float4( Y, Z, X, Y );
        float4 yzxz = new float4( Y, Z, X, Z );
        float4 yzxw = new float4( Y, Z, X, W );
        float4 yzyx = new float4( Y, Z, Y, X );
        float4 yzyy = new float4( Y, Z, Y, Y );
        float4 yzyz = new float4( Y, Z, Y, Z );
        float4 yzyw = new float4( Y, Z, Y, W );
        float4 yzzx = new float4( Y, Z, Z, X );
        float4 yzzy = new float4( Y, Z, Z, Y );
        float4 yzzz = new float4( Y, Z, Z, Z );
        float4 yzzw = new float4( Y, Z, Z, W );
        float4 yzwx = new float4( Y, Z, W, X );
        float4 yzwy = new float4( Y, Z, W, Y );
        float4 yzwz = new float4( Y, Z, W, Z );
        float4 yzww = new float4( Y, Z, W, W );
        float4 ywxx = new float4( Y, W, X, X );
        float4 ywxy = new float4( Y, W, X, Y );
        float4 ywxz = new float4( Y, W, X, Z );
        float4 ywxw = new float4( Y, W, X, W );
        float4 ywyx = new float4( Y, W, Y, X );
        float4 ywyy = new float4( Y, W, Y, Y );
        float4 ywyz = new float4( Y, W, Y, Z );
        float4 ywyw = new float4( Y, W, Y, W );
        float4 ywzx = new float4( Y, W, Z, X );
        float4 ywzy = new float4( Y, W, Z, Y );
        float4 ywzz = new float4( Y, W, Z, Z );
        float4 ywzw = new float4( Y, W, Z, W );
        float4 ywwx = new float4( Y, W, W, X );
        float4 ywwy = new float4( Y, W, W, Y );
        float4 ywwz = new float4( Y, W, W, Z );
        float4 ywww = new float4( Y, W, W, W );
        float4 zxxx = new float4( Z, X, X, X );
        float4 zxxy = new float4( Z, X, X, Y );
        float4 zxxz = new float4( Z, X, X, Z );
        float4 zxxw = new float4( Z, X, X, W );
        float4 zxyx = new float4( Z, X, Y, X );
        float4 zxyy = new float4( Z, X, Y, Y );
        float4 zxyz = new float4( Z, X, Y, Z );
        float4 zxyw = new float4( Z, X, Y, W );
        float4 zxzx = new float4( Z, X, Z, X );
        float4 zxzy = new float4( Z, X, Z, Y );
        float4 zxzz = new float4( Z, X, Z, Z );
        float4 zxzw = new float4( Z, X, Z, W );
        float4 zxwx = new float4( Z, X, W, X );
        float4 zxwy = new float4( Z, X, W, Y );
        float4 zxwz = new float4( Z, X, W, Z );
        float4 zxww = new float4( Z, X, W, W );
        float4 zyxx = new float4( Z, Y, X, X );
        float4 zyxy = new float4( Z, Y, X, Y );
        float4 zyxz = new float4( Z, Y, X, Z );
        float4 zyxw = new float4( Z, Y, X, W );
        float4 zyyx = new float4( Z, Y, Y, X );
        float4 zyyy = new float4( Z, Y, Y, Y );
        float4 zyyz = new float4( Z, Y, Y, Z );
        float4 zyyw = new float4( Z, Y, Y, W );
        float4 zyzx = new float4( Z, Y, Z, X );
        float4 zyzy = new float4( Z, Y, Z, Y );
        float4 zyzz = new float4( Z, Y, Z, Z );
        float4 zyzw = new float4( Z, Y, Z, W );
        float4 zywx = new float4( Z, Y, W, X );
        float4 zywy = new float4( Z, Y, W, Y );
        float4 zywz = new float4( Z, Y, W, Z );
        float4 zyww = new float4( Z, Y, W, W );
        float4 zzxx = new float4( Z, Z, X, X );
        float4 zzxy = new float4( Z, Z, X, Y );
        float4 zzxz = new float4( Z, Z, X, Z );
        float4 zzxw = new float4( Z, Z, X, W );
        float4 zzyx = new float4( Z, Z, Y, X );
        float4 zzyy = new float4( Z, Z, Y, Y );
        float4 zzyz = new float4( Z, Z, Y, Z );
        float4 zzyw = new float4( Z, Z, Y, W );
        float4 zzzx = new float4( Z, Z, Z, X );
        float4 zzzy = new float4( Z, Z, Z, Y );
        float4 zzzz = new float4( Z, Z, Z, Z );
        float4 zzzw = new float4( Z, Z, Z, W );
        float4 zzwx = new float4( Z, Z, W, X );
        float4 zzwy = new float4( Z, Z, W, Y );
        float4 zzwz = new float4( Z, Z, W, Z );
        float4 zzww = new float4( Z, Z, W, W );
        float4 zwxx = new float4( Z, W, X, X );
        float4 zwxy = new float4( Z, W, X, Y );
        float4 zwxz = new float4( Z, W, X, Z );
        float4 zwxw = new float4( Z, W, X, W );
        float4 zwyx = new float4( Z, W, Y, X );
        float4 zwyy = new float4( Z, W, Y, Y );
        float4 zwyz = new float4( Z, W, Y, Z );
        float4 zwyw = new float4( Z, W, Y, W );
        float4 zwzx = new float4( Z, W, Z, X );
        float4 zwzy = new float4( Z, W, Z, Y );
        float4 zwzz = new float4( Z, W, Z, Z );
        float4 zwzw = new float4( Z, W, Z, W );
        float4 zwwx = new float4( Z, W, W, X );
        float4 zwwy = new float4( Z, W, W, Y );
        float4 zwwz = new float4( Z, W, W, Z );
        float4 zwww = new float4( Z, W, W, W );
        float4 wxxx = new float4( W, X, X, X );
        float4 wxxy = new float4( W, X, X, Y );
        float4 wxxz = new float4( W, X, X, Z );
        float4 wxxw = new float4( W, X, X, W );
        float4 wxyx = new float4( W, X, Y, X );
        float4 wxyy = new float4( W, X, Y, Y );
        float4 wxyz = new float4( W, X, Y, Z );
        float4 wxyw = new float4( W, X, Y, W );
        float4 wxzx = new float4( W, X, Z, X );
        float4 wxzy = new float4( W, X, Z, Y );
        float4 wxzz = new float4( W, X, Z, Z );
        float4 wxzw = new float4( W, X, Z, W );
        float4 wxwx = new float4( W, X, W, X );
        float4 wxwy = new float4( W, X, W, Y );
        float4 wxwz = new float4( W, X, W, Z );
        float4 wxww = new float4( W, X, W, W );
        float4 wyxx = new float4( W, Y, X, X );
        float4 wyxy = new float4( W, Y, X, Y );
        float4 wyxz = new float4( W, Y, X, Z );
        float4 wyxw = new float4( W, Y, X, W );
        float4 wyyx = new float4( W, Y, Y, X );
        float4 wyyy = new float4( W, Y, Y, Y );
        float4 wyyz = new float4( W, Y, Y, Z );
        float4 wyyw = new float4( W, Y, Y, W );
        float4 wyzx = new float4( W, Y, Z, X );
        float4 wyzy = new float4( W, Y, Z, Y );
        float4 wyzz = new float4( W, Y, Z, Z );
        float4 wyzw = new float4( W, Y, Z, W );
        float4 wywx = new float4( W, Y, W, X );
        float4 wywy = new float4( W, Y, W, Y );
        float4 wywz = new float4( W, Y, W, Z );
        float4 wyww = new float4( W, Y, W, W );
        float4 wzxx = new float4( W, Z, X, X );
        float4 wzxy = new float4( W, Z, X, Y );
        float4 wzxz = new float4( W, Z, X, Z );
        float4 wzxw = new float4( W, Z, X, W );
        float4 wzyx = new float4( W, Z, Y, X );
        float4 wzyy = new float4( W, Z, Y, Y );
        float4 wzyz = new float4( W, Z, Y, Z );
        float4 wzyw = new float4( W, Z, Y, W );
        float4 wzzx = new float4( W, Z, Z, X );
        float4 wzzy = new float4( W, Z, Z, Y );
        float4 wzzz = new float4( W, Z, Z, Z );
        float4 wzzw = new float4( W, Z, Z, W );
        float4 wzwx = new float4( W, Z, W, X );
        float4 wzwy = new float4( W, Z, W, Y );
        float4 wzwz = new float4( W, Z, W, Z );
        float4 wzww = new float4( W, Z, W, W );
        float4 wwxx = new float4( W, W, X, X );
        float4 wwxy = new float4( W, W, X, Y );
        float4 wwxz = new float4( W, W, X, Z );
        float4 wwxw = new float4( W, W, X, W );
        float4 wwyx = new float4( W, W, Y, X );
        float4 wwyy = new float4( W, W, Y, Y );
        float4 wwyz = new float4( W, W, Y, Z );
        float4 wwyw = new float4( W, W, Y, W );
        float4 wwzx = new float4( W, W, Z, X );
        float4 wwzy = new float4( W, W, Z, Y );
        float4 wwzz = new float4( W, W, Z, Z );
        float4 wwzw = new float4( W, W, Z, W );
        float4 wwwx = new float4( W, W, W, X );
        float4 wwwy = new float4( W, W, W, Y );
        float4 wwwz = new float4( W, W, W, Z );
        float4 wwww = new float4( W, W, W, W );
        
        // 2D
        {
            float2 v = new float2(X, Y);

            Debug.Assert(AreEqual(v.xx, xx));
            Debug.Assert(AreEqual(v.xy, xy));
            Debug.Assert(AreEqual(v.yy, yy));
            Debug.Assert(AreEqual(v.yx, yx));

            v.xy = zw;
            Debug.Assert(AreEqual(v.xy, zw));

            v.yx = zw;
            Debug.Assert(AreEqual(v.yx, zw));
        }

        // 3D
        {
            float3 v = new float3( X, Y, Z );
            
            Debug.Assert(AreEqual(v.xx, xx));
            Debug.Assert(AreEqual(v.xy, xy));
            Debug.Assert(AreEqual(v.xz, xz));
            
            Debug.Assert(AreEqual(v.yx, yx));
            Debug.Assert(AreEqual(v.yy, yy));
            Debug.Assert(AreEqual(v.yz, yz));
            
            Debug.Assert(AreEqual(v.zx, zx));
            Debug.Assert(AreEqual(v.zy, zy));
            Debug.Assert(AreEqual(v.zz, zz));
            
            Debug.Assert(AreEqual(v.xxx, xxx));
            Debug.Assert(AreEqual(v.xxy, xxy));
            Debug.Assert(AreEqual(v.xxz, xxz));
            
            Debug.Assert(AreEqual(v.xyx, xyx));
            Debug.Assert(AreEqual(v.xyy, xyy));
            Debug.Assert(AreEqual(v.xyz, xyz));
            
            Debug.Assert(AreEqual(v.xzx, xzx));
            Debug.Assert(AreEqual(v.xzy, xzy));
            Debug.Assert(AreEqual(v.xzz, xzz));
            
            Debug.Assert(AreEqual(v.yxx, yxx));
            Debug.Assert(AreEqual(v.yxy, yxy));
            Debug.Assert(AreEqual(v.yxz, yxz));
            
            Debug.Assert(AreEqual(v.yyx, yyx));
            Debug.Assert(AreEqual(v.yyy, yyy));
            Debug.Assert(AreEqual(v.yyz, yyz));
            
            Debug.Assert(AreEqual(v.yzx, yzx));
            Debug.Assert(AreEqual(v.yzy, yzy));
            Debug.Assert(AreEqual(v.yzz, yzz));
            
            Debug.Assert(AreEqual(v.zxx, zxx));
            Debug.Assert(AreEqual(v.zxy, zxy));
            Debug.Assert(AreEqual(v.zxz, zxz));
            
            Debug.Assert(AreEqual(v.zyx, zyx));
            Debug.Assert(AreEqual(v.zyy, zyy));
            Debug.Assert(AreEqual(v.zyz, zyz));
            
            Debug.Assert(AreEqual(v.zzx, zzx));
            Debug.Assert(AreEqual(v.zzy, zzy));
            Debug.Assert(AreEqual(v.zzz, zzz));
            
            v.xy = zw;
            Debug.Assert(AreEqual(v.xy, zw));
            
            v.yx = zw;
            Debug.Assert(AreEqual(v.yx, zw));
            
            v.xz = yw;
            Debug.Assert(AreEqual(v.xz, yw));
            
            v.zx = yw;
            Debug.Assert(AreEqual(v.zx, yw));
            
            v.yz = xw;
            Debug.Assert(AreEqual(v.yz, xw));
            
            v.zy = xw;
            Debug.Assert(AreEqual(v.zy, xw));
            
            v.xyz = wzy;
            Debug.Assert(AreEqual(v.xyz, wzy));
            
            v.yzx = wxy;
            Debug.Assert(AreEqual(v.yzx, wxy));
            
            v.zxy = wzx;
            Debug.Assert(AreEqual(v.zxy, wzx));
            
            v.xzy = wyx;
            Debug.Assert(AreEqual(v.xzy, wyx));
            
            v.zyx = wzy;
            Debug.Assert(AreEqual(v.zyx, wzy));
            
            v.yxz = wzy;
            Debug.Assert(AreEqual(v.yxz, wzy));
        }
        
        // 4D
        {
            float4 v = new float4( X, Y, Z, W );
            
            Debug.Assert(AreEqual(v.xx, xx));
            Debug.Assert(AreEqual(v.xy, xy));
            Debug.Assert(AreEqual(v.xz, xz));
            Debug.Assert(AreEqual(v.xw, xw));
            Debug.Assert(AreEqual(v.yx, yx));
            Debug.Assert(AreEqual(v.yy, yy));
            Debug.Assert(AreEqual(v.yz, yz));
            Debug.Assert(AreEqual(v.yw, yw));
            Debug.Assert(AreEqual(v.zx, zx));
            Debug.Assert(AreEqual(v.zy, zy));
            Debug.Assert(AreEqual(v.zz, zz));
            Debug.Assert(AreEqual(v.zw, zw));
            Debug.Assert(AreEqual(v.wx, wx));
            Debug.Assert(AreEqual(v.wy, wy));
            Debug.Assert(AreEqual(v.wz, wz));
            Debug.Assert(AreEqual(v.ww, ww));
            
            Debug.Assert(AreEqual(v.xxx, xxx));
            Debug.Assert(AreEqual(v.xxy, xxy));
            Debug.Assert(AreEqual(v.xxz, xxz));
            Debug.Assert(AreEqual(v.xxw, xxw));
            Debug.Assert(AreEqual(v.xyx, xyx));
            Debug.Assert(AreEqual(v.xyy, xyy));
            Debug.Assert(AreEqual(v.xyz, xyz));
            Debug.Assert(AreEqual(v.xyw, xyw));
            Debug.Assert(AreEqual(v.xzx, xzx));
            Debug.Assert(AreEqual(v.xzy, xzy));
            Debug.Assert(AreEqual(v.xzz, xzz));
            Debug.Assert(AreEqual(v.xzw, xzw));
            Debug.Assert(AreEqual(v.xwx, xwx));
            Debug.Assert(AreEqual(v.xwy, xwy));
            Debug.Assert(AreEqual(v.xwz, xwz));
            Debug.Assert(AreEqual(v.xww, xww));
            
            Debug.Assert(AreEqual(v.yxx, yxx));
            Debug.Assert(AreEqual(v.yxy, yxy));
            Debug.Assert(AreEqual(v.yxz, yxz));
            Debug.Assert(AreEqual(v.yxw, yxw));
            Debug.Assert(AreEqual(v.yyx, yyx));
            Debug.Assert(AreEqual(v.yyy, yyy));
            Debug.Assert(AreEqual(v.yyz, yyz));
            Debug.Assert(AreEqual(v.yyw, yyw));
            Debug.Assert(AreEqual(v.yzx, yzx));
            Debug.Assert(AreEqual(v.yzy, yzy));
            Debug.Assert(AreEqual(v.yzz, yzz));
            Debug.Assert(AreEqual(v.yzw, yzw));
            Debug.Assert(AreEqual(v.ywx, ywx));
            Debug.Assert(AreEqual(v.ywy, ywy));
            Debug.Assert(AreEqual(v.ywz, ywz));
            Debug.Assert(AreEqual(v.yww, yww));
            
            Debug.Assert(AreEqual(v.zxx, zxx));
            Debug.Assert(AreEqual(v.zxy, zxy));
            Debug.Assert(AreEqual(v.zxz, zxz));
            Debug.Assert(AreEqual(v.zxw, zxw));
            Debug.Assert(AreEqual(v.zyx, zyx));
            Debug.Assert(AreEqual(v.zyy, zyy));
            Debug.Assert(AreEqual(v.zyz, zyz));
            Debug.Assert(AreEqual(v.zyw, zyw));
            Debug.Assert(AreEqual(v.zzx, zzx));
            Debug.Assert(AreEqual(v.zzy, zzy));
            Debug.Assert(AreEqual(v.zzz, zzz));
            Debug.Assert(AreEqual(v.zzw, zzw));
            Debug.Assert(AreEqual(v.zwx, zwx));
            Debug.Assert(AreEqual(v.zwy, zwy));
            Debug.Assert(AreEqual(v.zwz, zwz));
            Debug.Assert(AreEqual(v.zww, zww));
            
            Debug.Assert(AreEqual(v.wxx, wxx));
            Debug.Assert(AreEqual(v.wxy, wxy));
            Debug.Assert(AreEqual(v.wxz, wxz));
            Debug.Assert(AreEqual(v.wxw, wxw));
            Debug.Assert(AreEqual(v.wyx, wyx));
            Debug.Assert(AreEqual(v.wyy, wyy));
            Debug.Assert(AreEqual(v.wyz, wyz));
            Debug.Assert(AreEqual(v.wyw, wyw));
            Debug.Assert(AreEqual(v.wzx, wzx));
            Debug.Assert(AreEqual(v.wzy, wzy));
            Debug.Assert(AreEqual(v.wzz, wzz));
            Debug.Assert(AreEqual(v.wzw, wzw));
            Debug.Assert(AreEqual(v.wwx, wwx));
            Debug.Assert(AreEqual(v.wwy, wwy));
            Debug.Assert(AreEqual(v.wwz, wwz));
            Debug.Assert(AreEqual(v.www, www));
            
            Debug.Assert(AreEqual(v.xxxx, xxxx));
            Debug.Assert(AreEqual(v.xxxy, xxxy));
            Debug.Assert(AreEqual(v.xxxz, xxxz));
            Debug.Assert(AreEqual(v.xxxw, xxxw));
            Debug.Assert(AreEqual(v.xxyx, xxyx));
            Debug.Assert(AreEqual(v.xxyy, xxyy));
            Debug.Assert(AreEqual(v.xxyz, xxyz));
            Debug.Assert(AreEqual(v.xxyw, xxyw));
            Debug.Assert(AreEqual(v.xxzx, xxzx));
            Debug.Assert(AreEqual(v.xxzy, xxzy));
            Debug.Assert(AreEqual(v.xxzz, xxzz));
            Debug.Assert(AreEqual(v.xxzw, xxzw));
            Debug.Assert(AreEqual(v.xxwx, xxwx));
            Debug.Assert(AreEqual(v.xxwy, xxwy));
            Debug.Assert(AreEqual(v.xxwz, xxwz));
            Debug.Assert(AreEqual(v.xxww, xxww));
            Debug.Assert(AreEqual(v.xyxx, xyxx));
            Debug.Assert(AreEqual(v.xyxy, xyxy));
            Debug.Assert(AreEqual(v.xyxz, xyxz));
            Debug.Assert(AreEqual(v.xyxw, xyxw));
            Debug.Assert(AreEqual(v.xyyx, xyyx));
            Debug.Assert(AreEqual(v.xyyy, xyyy));
            Debug.Assert(AreEqual(v.xyyz, xyyz));
            Debug.Assert(AreEqual(v.xyyw, xyyw));
            Debug.Assert(AreEqual(v.xyzx, xyzx));
            Debug.Assert(AreEqual(v.xyzy, xyzy));
            Debug.Assert(AreEqual(v.xyzz, xyzz));
            Debug.Assert(AreEqual(v.xyzw, xyzw));
            Debug.Assert(AreEqual(v.xywx, xywx));
            Debug.Assert(AreEqual(v.xywy, xywy));
            Debug.Assert(AreEqual(v.xywz, xywz));
            Debug.Assert(AreEqual(v.xyww, xyww));
            Debug.Assert(AreEqual(v.xzxx, xzxx));
            Debug.Assert(AreEqual(v.xzxy, xzxy));
            Debug.Assert(AreEqual(v.xzxz, xzxz));
            Debug.Assert(AreEqual(v.xzxw, xzxw));
            Debug.Assert(AreEqual(v.xzyx, xzyx));
            Debug.Assert(AreEqual(v.xzyy, xzyy));
            Debug.Assert(AreEqual(v.xzyz, xzyz));
            Debug.Assert(AreEqual(v.xzyw, xzyw));
            Debug.Assert(AreEqual(v.xzzx, xzzx));
            Debug.Assert(AreEqual(v.xzzy, xzzy));
            Debug.Assert(AreEqual(v.xzzz, xzzz));
            Debug.Assert(AreEqual(v.xzzw, xzzw));
            Debug.Assert(AreEqual(v.xzwx, xzwx));
            Debug.Assert(AreEqual(v.xzwy, xzwy));
            Debug.Assert(AreEqual(v.xzwz, xzwz));
            Debug.Assert(AreEqual(v.xzww, xzww));
            Debug.Assert(AreEqual(v.xwxx, xwxx));
            Debug.Assert(AreEqual(v.xwxy, xwxy));
            Debug.Assert(AreEqual(v.xwxz, xwxz));
            Debug.Assert(AreEqual(v.xwxw, xwxw));
            Debug.Assert(AreEqual(v.xwyx, xwyx));
            Debug.Assert(AreEqual(v.xwyy, xwyy));
            Debug.Assert(AreEqual(v.xwyz, xwyz));
            Debug.Assert(AreEqual(v.xwyw, xwyw));
            Debug.Assert(AreEqual(v.xwzx, xwzx));
            Debug.Assert(AreEqual(v.xwzy, xwzy));
            Debug.Assert(AreEqual(v.xwzz, xwzz));
            Debug.Assert(AreEqual(v.xwzw, xwzw));
            Debug.Assert(AreEqual(v.xwwx, xwwx));
            Debug.Assert(AreEqual(v.xwwy, xwwy));
            Debug.Assert(AreEqual(v.xwwz, xwwz));
            Debug.Assert(AreEqual(v.xwww, xwww));
            Debug.Assert(AreEqual(v.yxxx, yxxx));
            Debug.Assert(AreEqual(v.yxxy, yxxy));
            Debug.Assert(AreEqual(v.yxxz, yxxz));
            Debug.Assert(AreEqual(v.yxxw, yxxw));
            Debug.Assert(AreEqual(v.yxyx, yxyx));
            Debug.Assert(AreEqual(v.yxyy, yxyy));
            Debug.Assert(AreEqual(v.yxyz, yxyz));
            Debug.Assert(AreEqual(v.yxyw, yxyw));
            Debug.Assert(AreEqual(v.yxzx, yxzx));
            Debug.Assert(AreEqual(v.yxzy, yxzy));
            Debug.Assert(AreEqual(v.yxzz, yxzz));
            Debug.Assert(AreEqual(v.yxzw, yxzw));
            Debug.Assert(AreEqual(v.yxwx, yxwx));
            Debug.Assert(AreEqual(v.yxwy, yxwy));
            Debug.Assert(AreEqual(v.yxwz, yxwz));
            Debug.Assert(AreEqual(v.yxww, yxww));
            Debug.Assert(AreEqual(v.yyxx, yyxx));
            Debug.Assert(AreEqual(v.yyxy, yyxy));
            Debug.Assert(AreEqual(v.yyxz, yyxz));
            Debug.Assert(AreEqual(v.yyxw, yyxw));
            Debug.Assert(AreEqual(v.yyyx, yyyx));
            Debug.Assert(AreEqual(v.yyyy, yyyy));
            Debug.Assert(AreEqual(v.yyyz, yyyz));
            Debug.Assert(AreEqual(v.yyyw, yyyw));
            Debug.Assert(AreEqual(v.yyzx, yyzx));
            Debug.Assert(AreEqual(v.yyzy, yyzy));
            Debug.Assert(AreEqual(v.yyzz, yyzz));
            Debug.Assert(AreEqual(v.yyzw, yyzw));
            Debug.Assert(AreEqual(v.yywx, yywx));
            Debug.Assert(AreEqual(v.yywy, yywy));
            Debug.Assert(AreEqual(v.yywz, yywz));
            Debug.Assert(AreEqual(v.yyww, yyww));
            Debug.Assert(AreEqual(v.yzxx, yzxx));
            Debug.Assert(AreEqual(v.yzxy, yzxy));
            Debug.Assert(AreEqual(v.yzxz, yzxz));
            Debug.Assert(AreEqual(v.yzxw, yzxw));
            Debug.Assert(AreEqual(v.yzyx, yzyx));
            Debug.Assert(AreEqual(v.yzyy, yzyy));
            Debug.Assert(AreEqual(v.yzyz, yzyz));
            Debug.Assert(AreEqual(v.yzyw, yzyw));
            Debug.Assert(AreEqual(v.yzzx, yzzx));
            Debug.Assert(AreEqual(v.yzzy, yzzy));
            Debug.Assert(AreEqual(v.yzzz, yzzz));
            Debug.Assert(AreEqual(v.yzzw, yzzw));
            Debug.Assert(AreEqual(v.yzwx, yzwx));
            Debug.Assert(AreEqual(v.yzwy, yzwy));
            Debug.Assert(AreEqual(v.yzwz, yzwz));
            Debug.Assert(AreEqual(v.yzww, yzww));
            Debug.Assert(AreEqual(v.ywxx, ywxx));
            Debug.Assert(AreEqual(v.ywxy, ywxy));
            Debug.Assert(AreEqual(v.ywxz, ywxz));
            Debug.Assert(AreEqual(v.ywxw, ywxw));
            Debug.Assert(AreEqual(v.ywyx, ywyx));
            Debug.Assert(AreEqual(v.ywyy, ywyy));
            Debug.Assert(AreEqual(v.ywyz, ywyz));
            Debug.Assert(AreEqual(v.ywyw, ywyw));
            Debug.Assert(AreEqual(v.ywzx, ywzx));
            Debug.Assert(AreEqual(v.ywzy, ywzy));
            Debug.Assert(AreEqual(v.ywzz, ywzz));
            Debug.Assert(AreEqual(v.ywzw, ywzw));
            Debug.Assert(AreEqual(v.ywwx, ywwx));
            Debug.Assert(AreEqual(v.ywwy, ywwy));
            Debug.Assert(AreEqual(v.ywwz, ywwz));
            Debug.Assert(AreEqual(v.ywww, ywww));
            Debug.Assert(AreEqual(v.zxxx, zxxx));
            Debug.Assert(AreEqual(v.zxxy, zxxy));
            Debug.Assert(AreEqual(v.zxxz, zxxz));
            Debug.Assert(AreEqual(v.zxxw, zxxw));
            Debug.Assert(AreEqual(v.zxyx, zxyx));
            Debug.Assert(AreEqual(v.zxyy, zxyy));
            Debug.Assert(AreEqual(v.zxyz, zxyz));
            Debug.Assert(AreEqual(v.zxyw, zxyw));
            Debug.Assert(AreEqual(v.zxzx, zxzx));
            Debug.Assert(AreEqual(v.zxzy, zxzy));
            Debug.Assert(AreEqual(v.zxzz, zxzz));
            Debug.Assert(AreEqual(v.zxzw, zxzw));
            Debug.Assert(AreEqual(v.zxwx, zxwx));
            Debug.Assert(AreEqual(v.zxwy, zxwy));
            Debug.Assert(AreEqual(v.zxwz, zxwz));
            Debug.Assert(AreEqual(v.zxww, zxww));
            Debug.Assert(AreEqual(v.zyxx, zyxx));
            Debug.Assert(AreEqual(v.zyxy, zyxy));
            Debug.Assert(AreEqual(v.zyxz, zyxz));
            Debug.Assert(AreEqual(v.zyxw, zyxw));
            Debug.Assert(AreEqual(v.zyyx, zyyx));
            Debug.Assert(AreEqual(v.zyyy, zyyy));
            Debug.Assert(AreEqual(v.zyyz, zyyz));
            Debug.Assert(AreEqual(v.zyyw, zyyw));
            Debug.Assert(AreEqual(v.zyzx, zyzx));
            Debug.Assert(AreEqual(v.zyzy, zyzy));
            Debug.Assert(AreEqual(v.zyzz, zyzz));
            Debug.Assert(AreEqual(v.zyzw, zyzw));
            Debug.Assert(AreEqual(v.zywx, zywx));
            Debug.Assert(AreEqual(v.zywy, zywy));
            Debug.Assert(AreEqual(v.zywz, zywz));
            Debug.Assert(AreEqual(v.zyww, zyww));
            Debug.Assert(AreEqual(v.zzxx, zzxx));
            Debug.Assert(AreEqual(v.zzxy, zzxy));
            Debug.Assert(AreEqual(v.zzxz, zzxz));
            Debug.Assert(AreEqual(v.zzxw, zzxw));
            Debug.Assert(AreEqual(v.zzyx, zzyx));
            Debug.Assert(AreEqual(v.zzyy, zzyy));
            Debug.Assert(AreEqual(v.zzyz, zzyz));
            Debug.Assert(AreEqual(v.zzyw, zzyw));
            Debug.Assert(AreEqual(v.zzzx, zzzx));
            Debug.Assert(AreEqual(v.zzzy, zzzy));
            Debug.Assert(AreEqual(v.zzzz, zzzz));
            Debug.Assert(AreEqual(v.zzzw, zzzw));
            Debug.Assert(AreEqual(v.zzwx, zzwx));
            Debug.Assert(AreEqual(v.zzwy, zzwy));
            Debug.Assert(AreEqual(v.zzwz, zzwz));
            Debug.Assert(AreEqual(v.zzww, zzww));
            Debug.Assert(AreEqual(v.zwxx, zwxx));
            Debug.Assert(AreEqual(v.zwxy, zwxy));
            Debug.Assert(AreEqual(v.zwxz, zwxz));
            Debug.Assert(AreEqual(v.zwxw, zwxw));
            Debug.Assert(AreEqual(v.zwyx, zwyx));
            Debug.Assert(AreEqual(v.zwyy, zwyy));
            Debug.Assert(AreEqual(v.zwyz, zwyz));
            Debug.Assert(AreEqual(v.zwyw, zwyw));
            Debug.Assert(AreEqual(v.zwzx, zwzx));
            Debug.Assert(AreEqual(v.zwzy, zwzy));
            Debug.Assert(AreEqual(v.zwzz, zwzz));
            Debug.Assert(AreEqual(v.zwzw, zwzw));
            Debug.Assert(AreEqual(v.zwwx, zwwx));
            Debug.Assert(AreEqual(v.zwwy, zwwy));
            Debug.Assert(AreEqual(v.zwwz, zwwz));
            Debug.Assert(AreEqual(v.zwww, zwww));
            Debug.Assert(AreEqual(v.wxxx, wxxx));
            Debug.Assert(AreEqual(v.wxxy, wxxy));
            Debug.Assert(AreEqual(v.wxxz, wxxz));
            Debug.Assert(AreEqual(v.wxxw, wxxw));
            Debug.Assert(AreEqual(v.wxyx, wxyx));
            Debug.Assert(AreEqual(v.wxyy, wxyy));
            Debug.Assert(AreEqual(v.wxyz, wxyz));
            Debug.Assert(AreEqual(v.wxyw, wxyw));
            Debug.Assert(AreEqual(v.wxzx, wxzx));
            Debug.Assert(AreEqual(v.wxzy, wxzy));
            Debug.Assert(AreEqual(v.wxzz, wxzz));
            Debug.Assert(AreEqual(v.wxzw, wxzw));
            Debug.Assert(AreEqual(v.wxwx, wxwx));
            Debug.Assert(AreEqual(v.wxwy, wxwy));
            Debug.Assert(AreEqual(v.wxwz, wxwz));
            Debug.Assert(AreEqual(v.wxww, wxww));
            Debug.Assert(AreEqual(v.wyxx, wyxx));
            Debug.Assert(AreEqual(v.wyxy, wyxy));
            Debug.Assert(AreEqual(v.wyxz, wyxz));
            Debug.Assert(AreEqual(v.wyxw, wyxw));
            Debug.Assert(AreEqual(v.wyyx, wyyx));
            Debug.Assert(AreEqual(v.wyyy, wyyy));
            Debug.Assert(AreEqual(v.wyyz, wyyz));
            Debug.Assert(AreEqual(v.wyyw, wyyw));
            Debug.Assert(AreEqual(v.wyzx, wyzx));
            Debug.Assert(AreEqual(v.wyzy, wyzy));
            Debug.Assert(AreEqual(v.wyzz, wyzz));
            Debug.Assert(AreEqual(v.wyzw, wyzw));
            Debug.Assert(AreEqual(v.wywx, wywx));
            Debug.Assert(AreEqual(v.wywy, wywy));
            Debug.Assert(AreEqual(v.wywz, wywz));
            Debug.Assert(AreEqual(v.wyww, wyww));
            Debug.Assert(AreEqual(v.wzxx, wzxx));
            Debug.Assert(AreEqual(v.wzxy, wzxy));
            Debug.Assert(AreEqual(v.wzxz, wzxz));
            Debug.Assert(AreEqual(v.wzxw, wzxw));
            Debug.Assert(AreEqual(v.wzyx, wzyx));
            Debug.Assert(AreEqual(v.wzyy, wzyy));
            Debug.Assert(AreEqual(v.wzyz, wzyz));
            Debug.Assert(AreEqual(v.wzyw, wzyw));
            Debug.Assert(AreEqual(v.wzzx, wzzx));
            Debug.Assert(AreEqual(v.wzzy, wzzy));
            Debug.Assert(AreEqual(v.wzzz, wzzz));
            Debug.Assert(AreEqual(v.wzzw, wzzw));
            Debug.Assert(AreEqual(v.wzwx, wzwx));
            Debug.Assert(AreEqual(v.wzwy, wzwy));
            Debug.Assert(AreEqual(v.wzwz, wzwz));
            Debug.Assert(AreEqual(v.wzww, wzww));
            Debug.Assert(AreEqual(v.wwxx, wwxx));
            Debug.Assert(AreEqual(v.wwxy, wwxy));
            Debug.Assert(AreEqual(v.wwxz, wwxz));
            Debug.Assert(AreEqual(v.wwxw, wwxw));
            Debug.Assert(AreEqual(v.wwyx, wwyx));
            Debug.Assert(AreEqual(v.wwyy, wwyy));
            Debug.Assert(AreEqual(v.wwyz, wwyz));
            Debug.Assert(AreEqual(v.wwyw, wwyw));
            Debug.Assert(AreEqual(v.wwzx, wwzx));
            Debug.Assert(AreEqual(v.wwzy, wwzy));
            Debug.Assert(AreEqual(v.wwzz, wwzz));
            Debug.Assert(AreEqual(v.wwzw, wwzw));
            Debug.Assert(AreEqual(v.wwwx, wwwx));
            Debug.Assert(AreEqual(v.wwwy, wwwy));
            Debug.Assert(AreEqual(v.wwwz, wwwz));
            Debug.Assert(AreEqual(v.wwww, wwww));
            
            v.xy = zw;
            Debug.Assert(AreEqual(v.xy, zw));
            
            v.xz = yw;
            Debug.Assert(AreEqual(v.xz, yw));
            
            v.xw = yz;
            Debug.Assert(AreEqual(v.xw, yz));

            v.yx = wz;
            Debug.Assert(AreEqual(v.yx, wz));
            
            v.yz = wx;
            Debug.Assert(AreEqual(v.yz, wx));
            
            v.yw = wz;
            Debug.Assert(AreEqual(v.yw, wz));
            
            v.wx = yz;
            Debug.Assert(AreEqual(v.wx, yz));
            
            v.wy = zx;
            Debug.Assert(AreEqual(v.wy, zx));
            
            v.wz = xy;
            Debug.Assert(AreEqual(v.wz, xy));
            
            v.xyz = wzy;
            Debug.Assert(AreEqual(v.xyz, wzy));
            
            v.xyw = zwx;
            Debug.Assert(AreEqual(v.xyw, zwx));
            
            v.xzy = zyw;
            Debug.Assert(AreEqual(v.xzy, zyw));
            
            v.xzw = zyx;
            Debug.Assert(AreEqual(v.xzw, zyx));
            
            v.xwy = yzx;
            Debug.Assert(AreEqual(v.xwy, yzx));
            
            v.xwz = yzw;
            Debug.Assert(AreEqual(v.xwz, yzw));
            
            v.yxz = wzx;
            Debug.Assert(AreEqual(v.yxz, wzx));
            
            v.yxw = zwx;
            Debug.Assert(AreEqual(v.yxw, zwx));
            
            v.yzx = wxz;
            Debug.Assert(AreEqual(v.yzx, wxz));
            
            v.yzw = zxy;
            Debug.Assert(AreEqual(v.yzw, zxy));
            
            v.ywx = wxz;
            Debug.Assert(AreEqual(v.ywx, wxz));
            
            v.ywz = wxy;
            Debug.Assert(AreEqual(v.ywz, wxy));
            
            v.zxy = wyz;
            Debug.Assert(AreEqual(v.zxy, wyz));
            
            v.zxw = wyx;
            Debug.Assert(AreEqual(v.zxw, wyx));
            
            v.zyx = wxy;
            Debug.Assert(AreEqual(v.zyx, wxy));
            
            v.zyw = wxz;
            Debug.Assert(AreEqual(v.zyw, wxz));
            
            v.zwx = xyz;
            Debug.Assert(AreEqual(v.zwx, xyz));
            
            v.zwy = xyz;
            Debug.Assert(AreEqual(v.zwy, xyz));
            
            v.wxz = zyx;
            Debug.Assert(AreEqual(v.wxz, zyx));
            
            v.wxy = zwx;
            Debug.Assert(AreEqual(v.wxy, zwx));
            
            v.wyx = zxy;
            Debug.Assert(AreEqual(v.wyx, zxy));
            
            v.wyz = zxw;
            Debug.Assert(AreEqual(v.wyz, zxw));
            
            v.wzx = ywz;
            Debug.Assert(AreEqual(v.wzx, ywz));
            
            v.wzy = ywx;
            Debug.Assert(AreEqual(v.wzy, ywx));
            
            v.xyzw = wzyx;
            Debug.Assert(AreEqual(v.xyzw, wzyx));
            
            v.xywz = wzxy;
            Debug.Assert(AreEqual(v.xywz, wzxy));
            
            v.yxzw = zwyx;
            Debug.Assert(AreEqual(v.yxzw, zwyx));
            
            v.yxwz = zwxy;
            Debug.Assert(AreEqual(v.yxwz, zwxy));
            
            v.xzyw = wyzx;
            Debug.Assert(AreEqual(v.xzyw, wyzx));
            
            v.xzwy = wyxz;
            Debug.Assert(AreEqual(v.xzwy, wyxz));
            
            v.zxyw = ywzx;
            Debug.Assert(AreEqual(v.zxyw, ywzx));
            
            v.zxwy = ywxz;
            Debug.Assert(AreEqual(v.zxwy, ywxz));
            
            v.xwyz = wxzy;
            Debug.Assert(AreEqual(v.xwyz, wxzy));
            
            v.xwzy = wxyz;
            Debug.Assert(AreEqual(v.xwzy, wxyz));
            
            v.wxyz = xwzy;
            Debug.Assert(AreEqual(v.wxyz, xwzy));
            
            v.wxzy = xwyz;
            Debug.Assert(AreEqual(v.wxzy, xwyz));
            
            v.yzxw = zywx;
            Debug.Assert(AreEqual(v.yzxw, zywx));
            
            v.yzwx = zyxw;
            Debug.Assert(AreEqual(v.yzwx, zyxw));
            
            v.ywxz = zxwy;
            Debug.Assert(AreEqual(v.ywxz, zxwy));
            
            v.ywzx = zxyw;
            Debug.Assert(AreEqual(v.ywzx, zxyw));
            
            v.zyxw = yzwx;
            Debug.Assert(AreEqual(v.zyxw, yzwx));
            
            v.zywx = yzxw;
            Debug.Assert(AreEqual(v.zywx, yzxw));

            v.zwxy = yxwz;
            Debug.Assert(AreEqual(v.zwxy, yxwz));
            
            v.zwyx = yxzw;
            Debug.Assert(AreEqual(v.zwyx, yxzw));
            
            v.wzyx = xyzw;
            Debug.Assert(AreEqual(v.wzyx, xyzw));
            
            v.wzxy = xywz;
            Debug.Assert(AreEqual(v.wzxy, xywz));
            
            v.wyzx = xzyw;
            Debug.Assert(AreEqual(v.wyzx, xzyw));
            
            v.wyxz = xzwy;
            Debug.Assert(AreEqual(v.wyxz, xzwy));
        }
    }

    void Start()
    {
        TestSwizzling();
    }
}
