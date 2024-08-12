using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerInput))]
public class PlayerInputEditor : Editor
{
    SerializedProperty m_InputReaderProp;
    readonly GUIContent m_InputReaderContent = new("Input Reader");
    SerializedProperty m_InputStartProp;
    readonly GUIContent m_InputStartContent = new("Enable Input By Start");

    #region MOVEMENT
    bool m_MovementFoldout;
    readonly GUIContent m_MovementContent = new("Movement");

    SerializedProperty m_HorizontalMovementProp;
    readonly GUIContent m_HorizontalMovementContent = new("Horizontal Movement");
    SerializedProperty m_JumpProp;
    readonly GUIContent m_JumpContent = new("Jumping");
    SerializedProperty m_DashProp;
    readonly GUIContent m_DashContent = new("Dashing");
    SerializedProperty m_InteractProp;
    readonly GUIContent m_InteractContent = new("Interacting");
    #endregion // MOVEMENT

    #region ATTACK
    bool m_AttackFoldout;
    readonly GUIContent m_AttackContent = new("Attack");

    SerializedProperty m_FocusProp;
    readonly GUIContent m_FocusContent = new("Focus Attack");
    SerializedProperty m_LightAttackProp;
    readonly GUIContent m_LightAttackContent = new("Light Attack");
    SerializedProperty m_HeavyAttackProp;
    readonly GUIContent m_HeavyAttackContent = new("Heavy Attack");
    SerializedProperty m_SuperAttackProp;
    readonly GUIContent m_SuperAttackContent = new("Super Attack");
    #endregion // ATTACK

    #region SLOTS
    bool m_InventorySlotsFoldout;
    readonly GUIContent m_InventorySlotsContent = new("Inventory Slots");

    SerializedProperty m_SlotAProp;
    readonly GUIContent m_SlotAContent = new("Slots Slot A");
    SerializedProperty m_SlotBProp;
    readonly GUIContent m_SlotBContent = new("Slots Slot B");
    SerializedProperty m_SlotCProp;
    readonly GUIContent m_SlotCContent = new("Slots Slot C");
    SerializedProperty m_SlotDProp;
    readonly GUIContent m_SlotDContent = new("Slots Slot D");
    #endregion // ATTACK

    void OnEnable()
    {
        #region REFERENCES
        m_InputReaderProp = serializedObject.FindProperty("inputReader");
        #endregion // REFERENCES

        #region MOVEMENT
        m_HorizontalMovementProp = serializedObject.FindProperty("enableMovement");
        m_JumpProp = serializedObject.FindProperty("enableJump");
        m_DashProp = serializedObject.FindProperty("enableDash");
        m_InteractProp = serializedObject.FindProperty("enableInteract");
        #endregion // MOVEMENT

        m_InputStartProp = serializedObject.FindProperty("enableInputByStart");

        #region ATTACK
        m_FocusProp = serializedObject.FindProperty("enableFocus");
        m_LightAttackProp = serializedObject.FindProperty("enableLightAttack");
        m_HeavyAttackProp = serializedObject.FindProperty("enableHeavyAttack");
        m_SuperAttackProp = serializedObject.FindProperty("enableSuperAttack");
        #endregion // ATTACK

        #region SLOTS
        m_SlotAProp = serializedObject.FindProperty("enableSlotA");
        m_SlotBProp = serializedObject.FindProperty("enableSlotB");
        m_SlotCProp = serializedObject.FindProperty("enableSlotC");
        m_SlotDProp = serializedObject.FindProperty("enableSlotD");
        #endregion // SLOTS
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle boldFoldoutStyle = new(EditorStyles.foldout);
        boldFoldoutStyle.fontStyle = FontStyle.Bold;
        boldFoldoutStyle.fontSize = 14;

        #region REFERENCES
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_InputReaderProp, m_InputReaderContent);
        EditorGUILayout.PropertyField(m_InputStartProp, m_InputStartContent);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // REFERENCES

        #region MOVEMENT
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_MovementFoldout = EditorGUILayout.Foldout(m_MovementFoldout, m_MovementContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_MovementFoldout)
        {
            EditorGUILayout.PropertyField(m_HorizontalMovementProp, m_HorizontalMovementContent);
            EditorGUILayout.PropertyField(m_JumpProp, m_JumpContent);
            EditorGUILayout.PropertyField(m_DashProp, m_DashContent);
            EditorGUILayout.PropertyField(m_InteractProp, m_InteractContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // MOVEMENT

        #region ATTACK
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_AttackFoldout = EditorGUILayout.Foldout(m_AttackFoldout, m_AttackContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_AttackFoldout)
        {
            EditorGUILayout.PropertyField(m_FocusProp, m_FocusContent);
            EditorGUILayout.PropertyField(m_LightAttackProp, m_LightAttackContent);
            EditorGUILayout.PropertyField(m_HeavyAttackProp, m_HeavyAttackContent);
            EditorGUILayout.PropertyField(m_SuperAttackProp, m_SuperAttackContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // ATTACK

        #region SLOTS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_InventorySlotsFoldout = EditorGUILayout.Foldout(m_InventorySlotsFoldout, m_InventorySlotsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_InventorySlotsFoldout)
        {
            EditorGUILayout.PropertyField(m_SlotAProp, m_SlotAContent);
            EditorGUILayout.PropertyField(m_SlotBProp, m_SlotBContent);
            EditorGUILayout.PropertyField(m_SlotCProp, m_SlotCContent);
            EditorGUILayout.PropertyField(m_SlotDProp, m_SlotDContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // SLOTS

        serializedObject.ApplyModifiedProperties();
    }
}
