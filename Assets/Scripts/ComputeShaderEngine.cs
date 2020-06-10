using UnityEngine;
using Types;

public class ComputeShaderEngine : MonoBehaviour
{
    const int GPUGroupSize = 256;
    
    [Header("CPU Emulation")]
    public int NumCPUThreads = 1;
    
    [Header("Resources")]
    public ComputeShader ComputeShader;

    [Header("Random numbers")] 
    public bool RngSeedFromTimer = false;
    public int RngSeed = 0;
    public int RngMaxUniform = 1000000;
    public int RngCount = 256;
    public int RngStateLength = 55;
    
    private ComputeBuffer _rngState = null;
    
    #region Private
    void InitRng()
    {
        if (RngSeedFromTimer)
        {
            Random.InitState( (int)System.DateTime.Now.Ticks );
        }
        else
        {
            Random.InitState( (int)RngSeed );
        }
        
        int[] rngStateData = new int[RngCount*(RngStateLength+1)];
        for (int i = 0; i < RngCount; i++)
        {
            rngStateData[i * (RngStateLength + 1)] = 0;
            for (int j = 0; j < RngStateLength; j++)
            {
                rngStateData[i * (RngStateLength + 1) + j + 1] = (int)Random.Range( 0, (int)RngMaxUniform );
            }
        }
        _rngState = new ComputeBuffer( (int)(RngCount*(RngStateLength+1)), sizeof(int) );
        _rngState.SetData( rngStateData );
        
        ComputeShader.SetInt( "_rngMax", (int)RngMaxUniform);
        ComputeShader.SetInt( "_rngCount", (int)RngCount);
        ComputeShader.SetInt( "_rngStateLength", (int)RngStateLength);

        ComputeShaderEmulator._rngMax = RngMaxUniform;
        ComputeShaderEmulator._rngCount = RngCount;
        ComputeShaderEmulator._rngStateLength = RngStateLength;
        ComputeShaderEmulator._rngState = new int[rngStateData.Length];
        rngStateData.CopyTo(ComputeShaderEmulator._rngState, 0);
    }

    void TestThreadGroupIDs()
    {
        const int GroupSizeX = 16;
        const int GroupSizeY = 8;
        const int GroupSizeZ = 4;

        const int OutBufferSizeX = 32;
        const int OutBufferSizeY = 32;
        const int OutBufferSizeZ = 32;

        int3[] outBufferData = new int3[OutBufferSizeX * OutBufferSizeY * OutBufferSizeZ];
        ComputeBuffer outBuffer = new ComputeBuffer(OutBufferSizeX * OutBufferSizeY * OutBufferSizeZ, sizeof(int) * 3);
        
        int kernel = ComputeShader.FindKernel("GenerateThreadIDs");
        ComputeShader.SetInt("_outBufferSizeX", OutBufferSizeX);
        ComputeShader.SetInt("_outBufferSizeY", OutBufferSizeY);
        ComputeShader.SetInt("_outBufferSizeZ", OutBufferSizeZ);
        ComputeShader.SetBuffer(kernel,"_outBuffer", outBuffer);
        ComputeShader.Dispatch(kernel, (OutBufferSizeX / GroupSizeX) + 1, (OutBufferSizeY / GroupSizeY) + 1, (OutBufferSizeZ / GroupSizeZ) + 1);
        outBuffer.GetData(outBufferData);
        
        ComputeShaderEmulator._outBufferSizeX = OutBufferSizeX;
        ComputeShaderEmulator._outBufferSizeY = OutBufferSizeY;
        ComputeShaderEmulator._outBufferSizeZ = OutBufferSizeZ;
        ComputeShaderEmulator._outBuffer = new int3[OutBufferSizeX * OutBufferSizeY * OutBufferSizeZ];
        ComputeShaderEmulator.Dispatch(ComputeShaderEmulator.GenerateThreadIDs, (OutBufferSizeX / GroupSizeX) + 1, (OutBufferSizeY / GroupSizeY) + 1, (OutBufferSizeZ / GroupSizeZ) + 1);
        
        /**/
        using (var writer = new System.IO.StreamWriter("out.txt"))
        {
            string s = "";
            for (int i = 0; i < outBufferData.Length; i++)
            {
                s = i.ToString("D6") + " = " + 
                    outBufferData[i].x.ToString("D3") + " " + outBufferData[i].y.ToString("D3") + " " + outBufferData[i].z.ToString("D3") + "   " + 
                    ComputeShaderEmulator._outBuffer[i].x.ToString("D3") + " " + ComputeShaderEmulator._outBuffer[i].y.ToString("D3") + " " + ComputeShaderEmulator._outBuffer[i].z.ToString("D3");
                writer.WriteLine(s);
            }
        }
        
    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        ComputeShaderEmulator.NumCPUThreads = NumCPUThreads;
        
        InitRng();
        TestThreadGroupIDs();
    }
    
    void OnDestroy()
    {
        _rngState.Release();
    }
    #endregion
    
    #region Interface
    public void GenerateRandomNumbers(RenderTexture renderTexture)
    {
        int kernel = ComputeShader.FindKernel("GenerateRandomNumbers");
        ComputeShader.SetInt("_outRenderTextureWidth", renderTexture.width);
        ComputeShader.SetInt("_outRenderTextureHeight", renderTexture.height);
        ComputeShader.SetBuffer(kernel, "_rngState", _rngState);
        ComputeShader.SetTexture(kernel, "_outRenderTexture", renderTexture);
        ComputeShader.Dispatch(kernel, ((renderTexture.width * renderTexture.height ) / GPUGroupSize) + 1, 1, 1);
    }
    #endregion
    
    /*
    void TestRng()
    {
        Random.InitState( (int)DateTime.Now.Ticks );
        
        ComputeShaderEmulator._outRenderTextureWidth = 128;
        ComputeShaderEmulator._outRenderTextureHeight = 128;
        ComputeShaderEmulator._outRenderTexture = new Types.RWTexture2D<float>();

        ComputeShaderEmulator._rngMax = 100000;
        ComputeShaderEmulator._rngCount = 1;
        ComputeShaderEmulator._rngStateLength = 55;
        ComputeShaderEmulator._rngState = new Types.RWStructuredBuffer<int>();

        for (int rngIndex = 0; rngIndex < ComputeShaderEmulator._rngCount; rngIndex++)
        {
            // position
            ComputeShaderEmulator._rngState.Add(0);
            // state
            for (int stateIndex = 0; stateIndex < ComputeShaderEmulator._rngStateLength; stateIndex++)
            {
                ComputeShaderEmulator._rngState.Add(Random.Range(0, ComputeShaderEmulator._rngMax));
            }
        }
        
        
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.GenerateRandomNumbers, 128, 128, 1, 1 );

        const float HistogramMin = -1.0f;
        const float HistogramMax = 1.0f;
        const float HistogramRange = (HistogramMax - HistogramMin);
        int[] histogram = new int[21];
        float slotRange = HistogramRange / histogram.Length;

        for (int x = 0; x < ComputeShaderEmulator._outRenderTextureWidth; x++)
        {
            for (int y = 0; y < ComputeShaderEmulator._outRenderTextureWidth; y++)
            {
                Types.int2 xy = new Types.int2(x, y);
                if (ComputeShaderEmulator._outRenderTexture.ContainsKey(xy))
                {
                    float value = ComputeShaderEmulator._outRenderTexture[xy];

                    for (int i = 0; i < histogram.Length; i++)
                    {
                        if (value >= HistogramMin + i * slotRange && value < HistogramMin + (i + 1) * slotRange)
                        {
                            histogram[i]++;
                        }
                    }
                }
            }
        }

        string s = "";
        for (int i = 0; i < histogram.Length; i++)
        {
            s += histogram[i].ToString() + "\n";
        }
        Debug.Log(s);
    }
     */
}
