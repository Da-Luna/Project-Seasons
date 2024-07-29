using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AetherController))]
public class AetherControllerEditor : Editor
{
    #region LIGHT AND HEAVY AETHER PROPERTYS
    SerializedProperty m_UIPrefabAetherLightHeavyProp;
    SerializedProperty m_ParentUIAetherLightHeavyProp;
    SerializedProperty m_DelayAfterUseLHAttackProp;
    SerializedProperty m_ActiveAetherLHPointsProp;
    SerializedProperty m_AetherLHTimeStepProp;
    SerializedProperty m_SuperAttackBulletSpeedProp;
    SerializedProperty m_SuperAttackTimeBeforeShotProp;

    SerializedProperty m_LightAttackCostProp;
    SerializedProperty m_LightAttackCadenceProp;
    SerializedProperty m_LightAttackBulletSpeedProp;
    
    SerializedProperty m_HeavyAttackCostProp;
    SerializedProperty m_HeavyAttackBulletSpeedProp;
    SerializedProperty m_HeavyAttackTimeBeforeShotProp;
    #endregion // LIGHT AND HEAVY AETHER PROPERTYS

    #region SUPER AETHER PROPERTYS
    SerializedProperty m_StartAetherValueProp;
    SerializedProperty m_MaxAetherValueProp;
    SerializedProperty m_AetherValueFillPerSecondProp;
    SerializedProperty m_AetherValueTimeStepProp;
    #endregion // SUPER AETHER PROPERTYS

    #region LIGHT AND HEAVY CONTENT
    readonly GUIContent m_UIPrefabAetherLightHeavyContent = new("UI Prefab Aether Light Heavy");
    readonly GUIContent m_ParentUIAetherLightHeavyContent = new("Parent UI Aether Light Heavy");
    readonly GUIContent m_DelayAfterUseLHAttackContent = new("Delay After Use LH Attack");
    readonly GUIContent m_ActiveAetherLHPointsContent = new("Active Aether LH Points");
    readonly GUIContent m_AetherLHTimeStepContent = new("Aether LH Time Step");

    readonly GUIContent m_LightAttackCostContent = new("Light Attack Cost");
    readonly GUIContent m_LightAttackCadenceContent = new("Attack Cadence");
    readonly GUIContent m_LightAttackBulletSpeedContent = new("Bullet Speed");

    readonly GUIContent m_HeavyAttackCostContent = new("Heavy Attack Cost");
    readonly GUIContent m_HeavyAttackBulletSpeedContent = new("Heavy Bullet Speed");
    readonly GUIContent m_HeavyAttackTimeBeforeShotContent = new("Heavy Time Before Shot");
    #endregion // LIGHT AND HEAVY CONTENT

    #region SUPER AETHER CONTENT
    readonly GUIContent m_StartAetherValueContent = new("Start Aether Value");
    readonly GUIContent m_MaxAetherValueContent = new("Max Aether Value");
    readonly GUIContent m_AetherValueFillPerSecondContent = new("Aether Value Fill Per Second");
    readonly GUIContent m_AetherValueTimeStepContent = new("Aether Value Time Step");
    readonly GUIContent m_SuperAttackBulletSpeedContent = new("Super Bullet Speed");
    readonly GUIContent m_SuperAttackTimeBeforeShotContent = new("Super Time Before Shot");
    #endregion // SUPER AETHER CONTENT

    #region EDITOR TITLES
    readonly GUIContent m_LightAndHeavyAetherSettingsContent = new("Light And Heavy Aether Settings");
    readonly GUIContent m_SuperAetherSettingsContent = new("Super Aether Settings");
    #endregion // EDITOR TITLES

    bool m_SuperAetherSettingsFoldout;
    bool m_LightAndHeayAetherSettingsFoldout;

    GUIStyle boldFoldoutStyle;

    void OnEnable()
    {
        #region LIGHT AND HEAVY AETHER PROPERTYS
        m_UIPrefabAetherLightHeavyProp = serializedObject.FindProperty("uIPrefabAetherLightHeavy");
        m_ParentUIAetherLightHeavyProp = serializedObject.FindProperty("parentUIAetherLightHeavy");
        m_DelayAfterUseLHAttackProp = serializedObject.FindProperty("delayAfterUseLHAttack");
        m_ActiveAetherLHPointsProp = serializedObject.FindProperty("activeAetherLHPoints");
        m_AetherLHTimeStepProp = serializedObject.FindProperty("aetherLHTimeStep");

        m_LightAttackCostProp = serializedObject.FindProperty("lightAttackCost");
        m_LightAttackCadenceProp = serializedObject.FindProperty("lightAttackCadence");
        m_LightAttackBulletSpeedProp = serializedObject.FindProperty("lightAttackBulletSpeed");

        m_HeavyAttackCostProp = serializedObject.FindProperty("heavyAttackCost");
        m_HeavyAttackBulletSpeedProp = serializedObject.FindProperty("heavyAttackBulletSpeed");
        m_HeavyAttackTimeBeforeShotProp = serializedObject.FindProperty("heavyAttackTimeBeforeShot");
        #endregion // LIGHT AND HEAVY AETHER PROPERTYS

        #region SUPER AETHER PROPERTYS
        m_StartAetherValueProp = serializedObject.FindProperty("startingAetherValue");
        m_MaxAetherValueProp = serializedObject.FindProperty("maxAetherValue");
        m_AetherValueFillPerSecondProp = serializedObject.FindProperty("aetherValueFillPerSecond");
        m_AetherValueTimeStepProp = serializedObject.FindProperty("aetherValueTimeStep");
        m_SuperAttackBulletSpeedProp = serializedObject.FindProperty("superAttackBulletSpeed");
        m_SuperAttackTimeBeforeShotProp = serializedObject.FindProperty("superAttackTimeBeforeShot");
        #endregion // SUPER AETHER PROPERTYS

        #region GUI STYLE
        boldFoldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 14
        };
        #endregion // GUI STYLE
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        #region LIGHT AND HEAVY AETHER LAYOUT
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_LightAndHeayAetherSettingsFoldout = EditorGUILayout.Foldout(m_LightAndHeayAetherSettingsFoldout, m_LightAndHeavyAetherSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_LightAndHeayAetherSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_UIPrefabAetherLightHeavyProp, m_UIPrefabAetherLightHeavyContent);
            EditorGUILayout.PropertyField(m_ParentUIAetherLightHeavyProp, m_ParentUIAetherLightHeavyContent);
            EditorGUILayout.PropertyField(m_DelayAfterUseLHAttackProp, m_DelayAfterUseLHAttackContent);
            EditorGUILayout.PropertyField(m_ActiveAetherLHPointsProp, m_ActiveAetherLHPointsContent);
            EditorGUILayout.PropertyField(m_AetherLHTimeStepProp, m_AetherLHTimeStepContent);

            EditorGUILayout.PropertyField(m_LightAttackCostProp, m_LightAttackCostContent);
            EditorGUILayout.PropertyField(m_LightAttackCadenceProp, m_LightAttackCadenceContent);
            EditorGUILayout.PropertyField(m_LightAttackBulletSpeedProp, m_LightAttackBulletSpeedContent);

            EditorGUILayout.PropertyField(m_HeavyAttackCostProp, m_HeavyAttackCostContent);
            EditorGUILayout.PropertyField(m_HeavyAttackBulletSpeedProp, m_HeavyAttackBulletSpeedContent);
            EditorGUILayout.PropertyField(m_HeavyAttackTimeBeforeShotProp, m_HeavyAttackTimeBeforeShotContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // LIGHT AND HEAVY AETHER LAYOUT

        #region SUPER AETHER LAYOUT
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_SuperAetherSettingsFoldout = EditorGUILayout.Foldout(m_SuperAetherSettingsFoldout, m_SuperAetherSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_SuperAetherSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_StartAetherValueProp, m_StartAetherValueContent);
            EditorGUILayout.PropertyField(m_MaxAetherValueProp, m_MaxAetherValueContent);
            EditorGUILayout.PropertyField(m_AetherValueFillPerSecondProp, m_AetherValueFillPerSecondContent);
            EditorGUILayout.PropertyField(m_AetherValueTimeStepProp, m_AetherValueTimeStepContent);
            EditorGUILayout.PropertyField(m_SuperAttackBulletSpeedProp, m_SuperAttackBulletSpeedContent);
            EditorGUILayout.PropertyField(m_SuperAttackTimeBeforeShotProp, m_SuperAttackTimeBeforeShotContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // SUPER AETHER LAYOUT

        serializedObject.ApplyModifiedProperties();
    }
}
