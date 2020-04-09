using UnityEngine;
using Types;

public class ComputeShaderEngine : MonoBehaviour
{
    const int GPUGroupSize = 256;
    
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
            Random.InitState( RngSeed );
        }
        
        int[] rngStateData = new int[RngCount*(RngStateLength+1)];
        for (int i = 0; i < RngCount; i++)
        {
            rngStateData[i * (RngStateLength + 1)] = 0;
            for (int j = 0; j < RngStateLength; j++)
            {
                rngStateData[i * (RngStateLength + 1) + j + 1] = Random.Range( 0, RngMaxUniform );
            }
        }
        _rngState = new ComputeBuffer( RngCount*(RngStateLength+1), sizeof(int) );
        _rngState.SetData( rngStateData );
        
        ComputeShader.SetInt( "_rngMax", RngMaxUniform);
        ComputeShader.SetInt( "_rngCount", RngCount);
        ComputeShader.SetInt( "_rngStateLength", RngStateLength);

        Simulation._rngMax = RngMaxUniform;
        Simulation._rngCount = RngCount;
        Simulation._rngStateLength = RngStateLength;
        Simulation._rngState = new RWStructuredBuffer<int>();
        Simulation._rngState.AddRange( rngStateData );
    }

    void TestThreadGroupIDs()
    {
        const int GroupSizeX = 8;
        const int GroupSizeY = 8;
        const int GroupSizeZ = 8;

        const int OutBufferSizeX = 64;
        const int OutBufferSizeY = 64;
        const int OutBufferSizeZ = 64;

        int3[] outBufferData = new int3[OutBufferSizeX * OutBufferSizeY * OutBufferSizeZ];
        ComputeBuffer outBuffer = new ComputeBuffer(OutBufferSizeX * OutBufferSizeY * OutBufferSizeZ, sizeof(int) * 3);
        
        int kernel = ComputeShader.FindKernel("GenerateThreadIDs");
        ComputeShader.SetInt("_outBufferSizeX", OutBufferSizeX);
        ComputeShader.SetInt("_outBufferSizeY", OutBufferSizeY);
        ComputeShader.SetInt("_outBufferSizeZ", OutBufferSizeZ);
        ComputeShader.SetBuffer(kernel,"_outBuffer", outBuffer);
        ComputeShader.Dispatch(kernel, (OutBufferSizeX / GroupSizeX) + 1, (OutBufferSizeY / GroupSizeY) + 1, (OutBufferSizeZ / GroupSizeZ) + 1);
        outBuffer.GetData(outBufferData);
        
        Simulation._outBufferSizeX = OutBufferSizeX;
        Simulation._outBufferSizeY = OutBufferSizeY;
        Simulation._outBufferSizeZ = OutBufferSizeZ;
        Simulation._outBuffer = new RWStructuredBuffer<int3>();
        for (int i = 0; i < OutBufferSizeX * OutBufferSizeY * OutBufferSizeZ; i++)
        {
            Simulation._outBuffer.Add(new int3());
        }
        Simulation.Dispatch(Simulation.GenerateThreadIDs, (OutBufferSizeX / GroupSizeX) + 1, (OutBufferSizeY / GroupSizeY) + 1, (OutBufferSizeZ / GroupSizeZ) + 1);
        
        /**/
        using (var writer = new System.IO.StreamWriter("out.txt"))
        {
            string s = "";
            for (int i = 0; i < outBufferData.Length; i++)
            {
                s = i.ToString("D6") + " = " + 
                    outBufferData[i].x.ToString("D3") + " " + outBufferData[i].y.ToString("D3") + " " + outBufferData[i].z.ToString("D3") + "   " + 
                    Simulation._outBuffer[i].x.ToString("D3") + " " + Simulation._outBuffer[i].y.ToString("D3") + " " + Simulation._outBuffer[i].z.ToString("D3");
                writer.WriteLine(s);
            }
        }
        
    }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
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
    public void GenerateRandomNumbers(RWTexture2D<float> renderTexture)
    {
        Simulation._outRenderTextureWidth = renderTexture.width;
        Simulation._outRenderTextureHeight = renderTexture.height;
        Simulation._outRenderTexture = renderTexture;
        
        ComputeShaderKernel kernel = Simulation.GenerateRandomNumbers;
        Simulation.Dispatch( kernel, (uint)((renderTexture.width * renderTexture.height ) / GPUGroupSize) + 1, 1, 1 );
    }
    #endregion
    
    /*
    void TestRng()
    {
        Random.InitState( (int)DateTime.Now.Ticks );
        
        Simulation._outRenderTextureWidth = 128;
        Simulation._outRenderTextureHeight = 128;
        Simulation._outRenderTexture = new Types.RWTexture2D<float>();

        Simulation._rngMax = 100000;
        Simulation._rngCount = 1;
        Simulation._rngStateLength = 55;
        Simulation._rngState = new Types.RWStructuredBuffer<int>();

        for (int rngIndex = 0; rngIndex < Simulation._rngCount; rngIndex++)
        {
            // position
            Simulation._rngState.Add(0);
            // state
            for (int stateIndex = 0; stateIndex < Simulation._rngStateLength; stateIndex++)
            {
                Simulation._rngState.Add(Random.Range(0, Simulation._rngMax));
            }
        }
        
        
        Simulation.Dispatch( Simulation.GenerateRandomNumbers, 128, 128, 1, 1 );

        const float HistogramMin = -1.0f;
        const float HistogramMax = 1.0f;
        const float HistogramRange = (HistogramMax - HistogramMin);
        int[] histogram = new int[21];
        float slotRange = HistogramRange / histogram.Length;

        for (int x = 0; x < Simulation._outRenderTextureWidth; x++)
        {
            for (int y = 0; y < Simulation._outRenderTextureWidth; y++)
            {
                Types.int2 xy = new Types.int2(x, y);
                if (Simulation._outRenderTexture.ContainsKey(xy))
                {
                    float value = Simulation._outRenderTexture[xy];

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
