using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CodeGenerator
{
    public const string ComputeShaderStructsPath = "Assets/Shaders/Simulation/Structs.cginc";
    public const string CSharpStructsPath = "Assets/Scripts/Simulation/Structs.cs";
    public const string CSharpStructsNamespace = "Simulation";
    
    [MenuItem("Code/Generate structs and components %g")]
    static void Generate()
    {
        string src = System.IO.File.ReadAllText(ComputeShaderStructsPath, Encoding.Default);

        var structSrcs = SplitStructs(src);
        GenerateCSharpStructs( structSrcs );
    }

    static List<KeyValuePair<string,string>> SplitStructs(string src)
    {
        var result = new List<KeyValuePair<string,string>>();

        int index = 0;
        while (index < src.Length)
        {
            int substringIndex = src.IndexOf("struct", index );
            if (substringIndex == -1)
            {
                break;
            }
            
            int openingBraceIndex = src.IndexOf("{", substringIndex);
            if (openingBraceIndex == -1)
            {
                break;
            }
            
            int closingBraceIndex = src.IndexOf("}", openingBraceIndex);
            if (closingBraceIndex == -1)
            {
                break;
            }

            string structHeader = src.Substring(substringIndex, openingBraceIndex - substringIndex);
            string structBody = src.Substring(openingBraceIndex, closingBraceIndex - openingBraceIndex + 1);

            structHeader = structHeader.Trim(new char[]{' ', '\n'});
            structBody = structBody.Trim(new char[] {' ', '\n'});

            int spaceIndex = structHeader.IndexOf(" ");
            if (spaceIndex == -1)
            {
                Debug.LogError( "[CodeGenerator] unable to parse struct header.");
                break;
            }

            while (spaceIndex < structHeader.Length && structHeader[spaceIndex] == ' ')
            {
                spaceIndex++;
            }

            if (spaceIndex == structHeader.Length)
            {
                Debug.LogError( "[CodeGenerator] unable to parse struct header.");
                break;
            }

            string structName = structHeader.Substring(spaceIndex);
            
            result.Add( new KeyValuePair<string,string>(structName,structBody) );

            index = closingBraceIndex;
        }

        return result;
    }

    static void GenerateCSharpStructs(List<KeyValuePair<string,string>> structSrcs)
    {
        string text = "";

        text += "using UnityEngine;\n";
        text += "\n";
        text += "namespace Structs\n";
        text += "{\n";
        text += "}\n";
        
        System.IO.File.WriteAllText( CSharpStructsPath, text, Encoding.Default);
        AssetDatabase.Refresh();
    }
}
