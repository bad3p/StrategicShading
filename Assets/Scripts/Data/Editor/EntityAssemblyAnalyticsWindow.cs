using UnityEngine;
using UnityEditor;

public class EntityAssemblyAnalyticsWindow : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("EntityAssembly/Analytics")]
    static void Init()
    {
        EntityAssemblyAnalyticsWindow window =
            (EntityAssemblyAnalyticsWindow) GetWindowWithRect(typeof(EntityAssemblyAnalyticsWindow),
                new Rect(64, 64, 504, 512));
        window.Show();
        window.entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        GradientColorKey[] probabilityColors = new GradientColorKey[]
        {
            new GradientColorKey( Color.red, 0.0f ),
            new GradientColorKey( Color.Lerp( Color.yellow, Color.black, 0.333f ), 0.5f ),
            new GradientColorKey( Color.Lerp( Color.green, Color.black, 0.5f ), 1.0f ) 
        };
        GradientAlphaKey[] probabilityAlpha = new GradientAlphaKey[]
        {
            new GradientAlphaKey( 1.0f, 0.0f ),
            new GradientAlphaKey( 1.0f, 0.5f ),
            new GradientAlphaKey( 1.0f, 1.0f )
        };
        
        window._probabilityColor = new Gradient();
        window._probabilityColor.SetKeys( probabilityColors, probabilityAlpha );
        window._probabilityColor.mode = GradientMode.Blend;
    }

    private uint _firearmDescId = 0;
    private uint _personnelDescId = 0;
    
    private Gradient _probabilityColor = new Gradient();

    public EntityAssembly entityAssembly { get; private set; }

    private float GetFirearmFirepower(float distance)
    {
        return ComputeShaderEmulator.GetFirearmFirepower(
            ref entityAssembly.FirearmDescBuffer[_firearmDescId],
            ref entityAssembly.PersonnelDescBuffer[_personnelDescId], distance,
            entityAssembly.FirearmDescBuffer[_firearmDescId].maxBurstAmmo
        );
    }
    
    private float GetProbabilityToKill(float distance)
    {
        float firepowerAtDistance = GetFirearmFirepower(distance);

        var firepower = entityAssembly.Firepower;
        var killProbability = entityAssembly.KillProbability;

        if (firepowerAtDistance < firepower.x)
        {
            return ComputeShaderEmulator.max(0.0f, ComputeShaderEmulator.liney(firepower.x, killProbability.x, firepower.y, killProbability.y, firepowerAtDistance ) );
        }
        else if (firepowerAtDistance < firepower.y)
        {
            float t = (firepowerAtDistance - firepower.x) / (firepower.y - firepower.x);
            return ComputeShaderEmulator.lerp(killProbability.x, killProbability.y, t);
        }
        else if (firepowerAtDistance < firepower.z)
        {
            float t = (firepowerAtDistance - firepower.y) / (firepower.z - firepower.y);
            return ComputeShaderEmulator.lerp(killProbability.y, killProbability.z, t);
        }
        else if (firepowerAtDistance < firepower.w)
        {
            float t = (firepowerAtDistance - firepower.z) / (firepower.w - firepower.z);
            return ComputeShaderEmulator.lerp(killProbability.z, killProbability.w, t);
        }
        else
        {
            return killProbability.w;
        }
    }
    
    private float GetProbabilityToWound(float distance)
    {
        float firepowerAtDistance = GetFirearmFirepower(distance);

        var firepower = entityAssembly.Firepower;
        var woundProbability = entityAssembly.WoundProbability;

        if (firepowerAtDistance < firepower.x)
        {
            return ComputeShaderEmulator.max(0.0f, ComputeShaderEmulator.liney(firepower.x, woundProbability.x, firepower.y, woundProbability.y, firepowerAtDistance ) );
        }
        else if (firepowerAtDistance < firepower.y)
        {
            float t = (firepowerAtDistance - firepower.x) / (firepower.y - firepower.x);
            return ComputeShaderEmulator.lerp(woundProbability.x, woundProbability.y, t);
        }
        else if (firepowerAtDistance < firepower.z)
        {
            float t = (firepowerAtDistance - firepower.y) / (firepower.z - firepower.y);
            return ComputeShaderEmulator.lerp(woundProbability.y, woundProbability.z, t);
        }
        else if (firepowerAtDistance < firepower.w)
        {
            float t = (firepowerAtDistance - firepower.z) / (firepower.w - firepower.z);
            return ComputeShaderEmulator.lerp(woundProbability.z, woundProbability.w, t);
        }
        else
        {
            return woundProbability.w;
        }
    }
    
    private float GetMoraleDamage(float distance)
    {
        float firepowerAtDistance = GetFirearmFirepower(distance);

        var firepower = entityAssembly.Firepower;
        var moraleDamage = entityAssembly.MoraleDamage;

        if (firepowerAtDistance < firepower.x)
        {
            float result = ComputeShaderEmulator.max(0.0f, ComputeShaderEmulator.liney(firepower.x, moraleDamage.x, firepower.y, moraleDamage.y, firepowerAtDistance ) );
            return result;
        }
        else if (firepowerAtDistance < firepower.y)
        {
            float t = (firepowerAtDistance - firepower.x) / (firepower.y - firepower.x);
            return ComputeShaderEmulator.lerp(moraleDamage.x, moraleDamage.y, t);
        }
        else if (firepowerAtDistance < firepower.z)
        {
            float t = (firepowerAtDistance - firepower.y) / (firepower.z - firepower.y);
            return ComputeShaderEmulator.lerp(moraleDamage.y, moraleDamage.z, t);
        }
        else if (firepowerAtDistance < firepower.w)
        {
            float t = (firepowerAtDistance - firepower.z) / (firepower.w - firepower.z);
            return ComputeShaderEmulator.lerp(moraleDamage.z, moraleDamage.w, t);
        }
        else
        {
            return moraleDamage.w;
        }
    }

    private string GetProbabilityToKill(float distance, string format)
    {
        float p = GetProbabilityToKill(distance);
        Color color = _probabilityColor.Evaluate( p * 0.01f );

        string richColorValue = "#";
        richColorValue += Mathf.RoundToInt(color.r * 255).ToString("X2");
        richColorValue += Mathf.RoundToInt(color.g * 255).ToString("X2");
        richColorValue += Mathf.RoundToInt(color.b * 255).ToString("X2");
        richColorValue += "FF";

        return "<color=" + richColorValue + ">" + p.ToString(format) + "</color>";
    }
    
    private string GetProbabilityToWound(float distance, string format)
    {
        float p = GetProbabilityToWound(distance);
        Color color = _probabilityColor.Evaluate( p * 0.01f );

        string richColorValue = "#";
        richColorValue += Mathf.RoundToInt(color.r * 255).ToString("X2");
        richColorValue += Mathf.RoundToInt(color.g * 255).ToString("X2");
        richColorValue += Mathf.RoundToInt(color.b * 255).ToString("X2");
        richColorValue += "FF";

        return "<color=" + richColorValue + ">" + p.ToString(format) + "</color>";
    }
    
    public static EntityAssemblyAnalyticsWindow instance { get; private set; } 

    void Awake()
    {
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Analyse firepower", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Firearm", EditorStyles.boldLabel);
        _firearmDescId = (uint) EditorGUILayout.Popup((int) _firearmDescId, entityAssembly.FirearmNameBuffer);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Personnel", EditorStyles.boldLabel);
        _personnelDescId = (uint) EditorGUILayout.Popup((int) _personnelDescId, entityAssembly.PersonnelNameBuffer);
        EditorGUILayout.EndHorizontal();

        var cellArea = new GUIStyle(GUI.skin.box);
        cellArea.alignment = TextAnchor.MiddleLeft;
        cellArea.fontStyle = FontStyle.Bold;
        cellArea.stretchHeight = false;
        cellArea.stretchWidth = false;
        cellArea.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
        cellArea.fixedHeight = 23;
        cellArea.fixedWidth = 96;

        var valueStyle = EditorStyles.label;
        valueStyle.richText = true;

        float[] distances = new float[] {10, 25, 50, 75, 100, 150, 200, 300, 400, 500, 600, 700, 800, 900, 1000};

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(cellArea);
            EditorGUILayout.LabelField("Distance", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_firearmDescId > 0 && _personnelDescId > 0)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal(cellArea);
                    EditorGUILayout.LabelField(distances[i].ToString("F0"));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(cellArea);
            EditorGUILayout.LabelField("Firepower", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_firearmDescId > 0 && _personnelDescId > 0)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal(cellArea);
                    EditorGUILayout.LabelField(GetFirearmFirepower(distances[i]).ToString("F1"));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(cellArea);
            EditorGUILayout.LabelField("P(kill)", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_firearmDescId > 0 && _personnelDescId > 0)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal(cellArea);
                    EditorGUILayout.LabelField(GetProbabilityToKill(distances[i], "F1"), valueStyle);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(cellArea);
            EditorGUILayout.LabelField("P(wound)", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_firearmDescId > 0 && _personnelDescId > 0)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal(cellArea);
                    EditorGUILayout.LabelField(GetProbabilityToWound(distances[i], "F1"), valueStyle);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(cellArea);
            EditorGUILayout.LabelField("D(morale)", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            if (_firearmDescId > 0 && _personnelDescId > 0)
            {
                for (int i = 0; i < distances.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal(cellArea);
                    EditorGUILayout.LabelField(GetMoraleDamage(distances[i]).ToString("F1"), valueStyle);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
}
        