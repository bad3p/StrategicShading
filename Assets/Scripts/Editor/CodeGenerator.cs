using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CodeGenerator
{
    public const string ComputeShaderStructsPath = "Assets/Shaders/Simulation/Structs.cginc";
    public const string CSharpStructsPath = "Assets/Scripts/Simulation/Structs.cs";
    public const string CSharpStructsNamespace = "Simulation";
    public const string CSharpComponentsPath = "Assets/Scripts/Demo/CodeGenerator/";
    public const string CSharpComponentNamespace = "Demo";
    
    [MenuItem("Code/Generate structs and components %g")]
    static void Generate()
    {
        string src = System.IO.File.ReadAllText(ComputeShaderStructsPath, Encoding.Default);

        var structSrcs = SplitStructs(src);
        GenerateCSharpStructs( structSrcs );
        GenerateCSharpComponents( structSrcs );
    }

    static int IndexOfSpaceOrTab(string str)
    {
        int indexOfSpace = str.IndexOf(" ");
        int indexOfTab = str.IndexOf("\t");
        if (indexOfSpace != -1 && indexOfTab == -1)
        {
            return indexOfSpace;
        }
        else if (indexOfSpace == -1 && indexOfTab != -1)
        {
            return indexOfTab;
        }
        else if (indexOfSpace != -1 && indexOfTab != -1)
        {
            return Mathf.Min(indexOfSpace, indexOfTab);
        }
        else
        {
            return -1;
        }
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

            structHeader = structHeader.Trim(new char[]{' ', '\n', '\r'});
            structBody = structBody.Trim(new char[] {' ', '\n', '\r'});

            int spaceIndex = IndexOfSpaceOrTab(structHeader);
            if (spaceIndex == -1)
            {
                Debug.LogError( "[CodeGenerator] unable to parse struct header.");
                break;
            }

            while (spaceIndex < structHeader.Length && (structHeader[spaceIndex] == ' ' || structHeader[spaceIndex] == '\t'))
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
        text += "namespace " + CSharpStructsNamespace + "\n";
        text += "{\n";
        text += "\n";

        foreach (var structSrc in structSrcs)
        {
            string structName = structSrc.Key;
            string structBody = structSrc.Value;

            text += "public struct " + structName + "\n";
            text += "{\n";

            var fields = SplitFields( structBody );
            int stride = 0;
            foreach (var field in fields)
            {
                var fieldType = field.Key;
                var fieldName = field.Value;

                text += "    public ";
                if (fieldType == "float")
                {
                    text += "float";
                    stride += sizeof(float);
                }
                else if (fieldType == "int")
                {
                    text += "int";
                    stride += sizeof(int);
                }
                else if (fieldType == "float2")
                {
                    text += "Vector2";
                    stride += sizeof(float) * 2;
                }
                else if (fieldType == "int2")
                {
                    text += "Vector2Int";
                    stride += sizeof(int) * 2;
                }
                else if (fieldType == "float3")
                {
                    text += "Vector3";
                    stride += sizeof(float) * 3;
                }
                else if (fieldType == "int3")
                {
                    text += "Vector3Int";
                    stride += sizeof(int) * 3;
                }
                text += " " + field.Value + ";\n";
            }
            
            text += "\n";
            text += "    public const int Stride = " + stride.ToString() + ";\n";
            
            text += "};\n";
            text += "\n";
        }
        
        text += "\n";
        text += "}\n";
        
        System.IO.File.WriteAllText( CSharpStructsPath, text, Encoding.Default);
        AssetDatabase.Refresh();
    }

    static List<KeyValuePair<string,string>> SplitFields(string structBody)
    {
        var result = new List<KeyValuePair<string, string>>();

        structBody = structBody.Trim(new char[] {' ', '\n', '\r', '{', '}'});
        
        int index = 0;
        while (index < structBody.Length)
        {
            int semicolonIndex = structBody.IndexOf(";", index );
            if (semicolonIndex == -1)
            {
                break;
            }

            string fieldBody = structBody.Substring(index, semicolonIndex - index);
            fieldBody = fieldBody.Trim(new char[] {' ', '\n', '\r'});
            
            int startSpaceIndex = IndexOfSpaceOrTab( fieldBody );
            if (startSpaceIndex == -1)
            {
                Debug.LogError( "[CodeGenerator] unable to parse field body.");
                break;
            }
            
            int endSpaceIndex = startSpaceIndex;
            
            while (endSpaceIndex < fieldBody.Length && (fieldBody[endSpaceIndex] == ' ' || fieldBody[endSpaceIndex] == '\t'))
            {
                endSpaceIndex++;
            }

            if (endSpaceIndex == fieldBody.Length)
            {
                Debug.LogError( "[CodeGenerator] unable to parse field body.");
                break;
            }
            
            result.Add( new KeyValuePair<string,string>( 
                fieldBody.Substring(0, startSpaceIndex ),
                fieldBody.Substring(endSpaceIndex, fieldBody.Length - endSpaceIndex )
            ) );

            index = semicolonIndex+1;
        }
        
        return result;
    }
    
    static void GenerateCSharpComponents(List<KeyValuePair<string,string>> structSrcs)
    {
        var componentNames = new List<string>();
        foreach (var structSrc in structSrcs)
        {
            componentNames.Add( structSrc.Key );
        }
        
        foreach (var structSrc in structSrcs)
        {
            var structName = structSrc.Key;
            var structBody = structSrc.Value;
            var fields = SplitFields( structBody );
            
            string text = "";
            
            text += "using UnityEngine;\n";
            text += "\n";
            text += "namespace " + CSharpComponentNamespace + "\n";
            text += "{\n";
            text += "\n";
            text += "    public class " + structName + " : MonoBehaviour\n";
            text += "    {\n";
            foreach (var field in fields)
            {
                var fieldType = field.Key;
                var fieldName = field.Value;

                string referenceType = "";
                if (fieldName.Length > 2)
                {
                    var fieldNameEnd = fieldName.Substring(fieldName.Length - 2, 2).ToLower();
                    if (fieldNameEnd == "id")
                    {
                        foreach (var componentName in componentNames)
                        {
                            if (fieldName.ToLower().Contains(componentName.ToLower()))
                            {
                                referenceType = componentName;
                                break;
                            }
                        }
                    }
                }

                text += "        public ";
                if (referenceType.Length != 0)
                {
                    text += referenceType;    
                }
                else if (fieldType == "float")
                {
                    text += "float";
                }
                else if (fieldType == "int")
                {
                    text += "int";
                }
                else if (fieldType == "float2")
                {
                    text += "Vector2";
                }
                else if (fieldType == "int2")
                {
                    text += "Vector2Int";
                }
                else if (fieldType == "float3")
                {
                    text += "Vector3";
                }
                else if (fieldType == "int3")
                {
                    text += "Vector3Int";
                }
                text += " " + field.Value + ";\n";
            }
            text += "    }\n";
            text += "\n";
            text += "\n";
            text += "}\n";
        
            System.IO.File.WriteAllText( CSharpComponentsPath + structName + ".cs", text, Encoding.Default);
        }
        
        AssetDatabase.Refresh();
    }
}
