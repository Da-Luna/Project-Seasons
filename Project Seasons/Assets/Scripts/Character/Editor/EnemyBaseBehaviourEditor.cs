using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EnemyBaseBehaviour), true)]
public class EnemyBaseBehaviourEditor : Editor
{
    #region MOVEMENT
    SerializedProperty m_PatrolSpeedProp;
    SerializedProperty m_RunSpeedProp;
    SerializedProperty m_GravityProp;
    SerializedProperty m_UseGravityProp;
    #endregion // MOVEMENT

    #region PATROLLING
    SerializedProperty m_PatrolDistanceToTravelProp;
    SerializedProperty m_MinPatrolWaitTimeProp;
    SerializedProperty m_MaxPatrolWaitTimeProp;
    #endregion // PATROLLING

    #region PATROLLING OBSTACLE
    SerializedProperty m_GroundObstacleMaskProp;
    SerializedProperty m_wallObstacleMaskProp;
    SerializedProperty m_abyssCheckSphereOffsetProp;
    #endregion // PATROLLING OBSTACLE

    #region TARGETING
    SerializedProperty m_viewPointHeightProp;
    SerializedProperty m_viewDirectionProp;
    SerializedProperty m_viewFovProp;
    SerializedProperty m_viewDistanceProp;
    SerializedProperty m_timeBeforeLostTargetProp;
    #endregion // TARGETING

    #region ATTACKING
    SerializedProperty m_attackObstacleMaskProp;
    SerializedProperty m_attackMaskProp;
    SerializedProperty m_attackOffsetProp;
    SerializedProperty m_attackRangeProp;
    SerializedProperty m_attackRadiusProp;
    #endregion // ATTACKING

    #region SPECIFICS
    SerializedProperty m_TargetPointProp;
    SerializedProperty m_TargetPointYOffsetProp;
    SerializedProperty m_minWaitTimeForNosediveProp;
    SerializedProperty m_maxWaitTimeForNosediveProp;
    SerializedProperty m_noseDiveSpeedProp;
    #endregion // SPECIFICS

    #region CONTROL PANEL
    SerializedProperty m_showViewFieldProp;
    SerializedProperty m_colorViewFieldProp;
    SerializedProperty m_showPatrolTargetPositionProp;
    SerializedProperty m_colorPatrolTargetPositionProp;
    SerializedProperty m_showGroundDetectSphereProp;
    SerializedProperty m_colorGroundDetectSphereProp;
    SerializedProperty m_showAttackPropertiesProp;
    SerializedProperty m_colorAttackFieldProp;
    SerializedProperty m_attackTargetProp;
    #endregion // CONTROL PANEL

    bool m_MovementFoldout;
    bool m_PatrollingFoldout;
    bool m_PatrollingObstacleFoldout;
    bool m_TargetingFoldout;
    bool m_AttackingFoldout;
    bool m_ControlPanelFoldout;
    bool m_SpecificsFoldout;

    #region MOVEMENT
    readonly GUIContent m_PatrolSpeedContent = new("Patrol Speed");
    readonly GUIContent m_RunSpeedContent = new("Run Speed");
    readonly GUIContent m_GravityContent = new("Gravity");
    readonly GUIContent m_UseGravityContent = new("Use Gravity");
    #endregion // MOVEMENT

    #region PATROLLING
    readonly GUIContent m_PatrolDistanceToTravelContent = new("Patrol Distance");
    readonly GUIContent m_MinPatrolWaitTimeContent = new("Min. Patrol Wait Time");
    readonly GUIContent m_MaxPatrolWaitTimeContent = new("Max. Patrol Wait Time");
    #endregion // PATROLLING

    #region PATROLLING OBSTACLE
    readonly GUIContent m_GroundObstacleMaskContent = new("Ground Obstacle Mask");
    readonly GUIContent m_wallObstacleMaskContent = new("Wall Obstacle Mask");
    readonly GUIContent m_abyssCheckSphereOffsetContent = new("Abyss Sphere Offset");
    #endregion // PATROLLING OBSTACLE

    #region TARGETING
    readonly GUIContent m_viewPointHeightContent = new("View Point Height");
    readonly GUIContent m_viewDirectionContent = new("View Direction");
    readonly GUIContent m_viewFovContent = new("View FOV");
    readonly GUIContent m_viewDistanceContent = new("View Distance");
    readonly GUIContent m_timeBeforeLostTargetContent = new("Time before lost Target");
    #endregion // TARGETING

    #region ATTACKING
    readonly GUIContent m_attackObstacleMaskContent = new("Attack Obstacle Mask");
    readonly GUIContent m_attackMaskContent = new("Attack Mask");
    readonly GUIContent m_attackOffsetContent = new("Attack Offset");
    readonly GUIContent m_attackRangeContent = new("Attack Range");
    readonly GUIContent m_attackRadiusContent = new("Attack Radius");
    #endregion // ATTACKING

    #region SPECIFICS
    readonly GUIContent m_TargetPointContent = new("Target Point");
    readonly GUIContent m_TargetPointYOffsetContent = new("Target Point Y Offset");
    readonly GUIContent m_minWaitTimeForNosediveContent = new("Min. Wait Time");
    readonly GUIContent m_maxWaitTimeForNosediveContent = new("Max. Wait Time");
    readonly GUIContent m_noseDiveSpeedContent = new("Attack Speed");
    #endregion // SPECIFICS

    #region CONTROL PANEL
    readonly GUIContent m_showViewFieldContent = new("Show View Field");
    readonly GUIContent m_colorViewFieldContent = new("Color View Field");
    readonly GUIContent m_showPatrolTargetPositionContent = new("Show Patrol Target Position");
    readonly GUIContent m_colorPatrolTargetPositionContent = new("Color Patrol Target Position");
    readonly GUIContent m_showGroundDetectSphereContent = new("Show Ground Detect Sphere");
    readonly GUIContent m_colorGroundDetectSphereContent = new("Color Ground Detect Sphere");
    readonly GUIContent m_showAttackPropertiesContent = new("Show Attack Properties");
    readonly GUIContent m_colorAttackFieldContent = new("Color Attack Field");
    readonly GUIContent m_attackTargetContent = new("Attack Target");
    #endregion // CONTROL PANEL


    #region HEADER TITLES
    readonly GUIContent m_MovementContent = new("Movement");
    readonly GUIContent m_PatrollingContent = new("Patrolling");
    readonly GUIContent m_PatrollingObstacleContent = new("Patrolling Obstacle");
    readonly GUIContent m_TargetingContent = new("Targeting");
    readonly GUIContent m_AttackingContent = new("Attacking");
    readonly GUIContent m_SpecificsContent = new("Enemy Specifics");
    readonly GUIContent m_ControlPanelContent = new("CONTROL PANEL (ONLY EDITOR)");
    #endregion // TITLES

    void OnEnable()
    {
        #region MOVEMENT
        m_PatrolSpeedProp = serializedObject.FindProperty("patrolSpeed");
        m_RunSpeedProp = serializedObject.FindProperty("runSpeed");
        m_GravityProp = serializedObject.FindProperty("gravity");
        m_UseGravityProp = serializedObject.FindProperty("useGravity");
        #endregion // MOVEMENT

        #region PATROLLING
        m_PatrolDistanceToTravelProp = serializedObject.FindProperty("patrolDistanceToTravel");
        m_MinPatrolWaitTimeProp = serializedObject.FindProperty("minPatrolWaitTime");
        m_MaxPatrolWaitTimeProp = serializedObject.FindProperty("maxPatrolWaitTime");
        #endregion // PATROLLING

        #region PATROLLING OBSTACLE
        m_GroundObstacleMaskProp = serializedObject.FindProperty("groundObstacleMask");
        m_wallObstacleMaskProp = serializedObject.FindProperty("wallObstacleMask");
        m_abyssCheckSphereOffsetProp = serializedObject.FindProperty("abyssCheckSphereOffset");
        #endregion // PATROLLING OBSTACLE

        #region TARGETING
        m_viewPointHeightProp = serializedObject.FindProperty("viewPointHeight");
        m_viewDirectionProp = serializedObject.FindProperty("viewDirection");
        m_viewFovProp = serializedObject.FindProperty("viewFov");
        m_viewDistanceProp = serializedObject.FindProperty("viewDistance");
        m_timeBeforeLostTargetProp = serializedObject.FindProperty("timeBeforeLostTarget");
        #endregion // TARGETING

        #region ATTACKING
        m_attackObstacleMaskProp = serializedObject.FindProperty("attackObstacleMask");
        m_attackMaskProp = serializedObject.FindProperty("attackMask");
        m_attackOffsetProp = serializedObject.FindProperty("attackOffset");
        m_attackRangeProp = serializedObject.FindProperty("attackRange");
        m_attackRadiusProp = serializedObject.FindProperty("attackRadius");
        #endregion // ATTACKING

        #region CONTROL PANEL
        m_showViewFieldProp = serializedObject.FindProperty("showViewField");
        m_colorViewFieldProp = serializedObject.FindProperty("colorViewField");
        m_showPatrolTargetPositionProp = serializedObject.FindProperty("showPatrolTargetPosition");
        m_colorPatrolTargetPositionProp = serializedObject.FindProperty("colorPatrolTargetPosition");
        m_showGroundDetectSphereProp = serializedObject.FindProperty("showGroundDetectSphere");
        m_colorGroundDetectSphereProp = serializedObject.FindProperty("colorGroundDetectSphere");
        m_showAttackPropertiesProp = serializedObject.FindProperty("showAttackProperties");
        m_colorAttackFieldProp = serializedObject.FindProperty("colorAttackField");
        m_attackTargetProp = serializedObject.FindProperty("attackTarget");
        #endregion // CONTROL PANEL 

        #region SPECIFICS    
        m_TargetPointProp = serializedObject.FindProperty("targetPointObject");
        m_TargetPointYOffsetProp = serializedObject.FindProperty("targetPointYOffset");
        m_minWaitTimeForNosediveProp = serializedObject.FindProperty("minWaitTimeForNosedive");
        m_maxWaitTimeForNosediveProp = serializedObject.FindProperty("maxWaitTimeForNosedive");
        m_noseDiveSpeedProp = serializedObject.FindProperty("noseDiveSpeed");
        #endregion // SPECIFICS
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUIStyle boldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        boldFoldoutStyle.fontStyle = FontStyle.Bold;
        boldFoldoutStyle.fontSize = 15; // Ändere die Schriftgröße nach Bedarf

        #region MOVEMENT
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_MovementFoldout = EditorGUILayout.Foldout(m_MovementFoldout, m_MovementContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_MovementFoldout)
        {
            EditorGUILayout.PropertyField(m_PatrolSpeedProp, m_PatrolSpeedContent);
            EditorGUILayout.PropertyField(m_RunSpeedProp, m_RunSpeedContent);
            EditorGUILayout.PropertyField(m_GravityProp, m_GravityContent);
            EditorGUILayout.PropertyField(m_UseGravityProp, m_UseGravityContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // MOVEMENT

        #region PATROLLING
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_PatrollingFoldout = EditorGUILayout.Foldout(m_PatrollingFoldout, m_PatrollingContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_PatrollingFoldout)
        {
            EditorGUILayout.PropertyField(m_PatrolDistanceToTravelProp, m_PatrolDistanceToTravelContent);
            EditorGUILayout.PropertyField(m_MinPatrolWaitTimeProp, m_MinPatrolWaitTimeContent);
            EditorGUILayout.PropertyField(m_MaxPatrolWaitTimeProp, m_MaxPatrolWaitTimeContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // PATROLLING

        #region PATROLLING OBSTACLE
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_PatrollingObstacleFoldout = EditorGUILayout.Foldout(m_PatrollingObstacleFoldout, m_PatrollingObstacleContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_PatrollingObstacleFoldout)
        {
            EditorGUILayout.PropertyField(m_GroundObstacleMaskProp, m_GroundObstacleMaskContent);
            EditorGUILayout.PropertyField(m_wallObstacleMaskProp, m_wallObstacleMaskContent);
            EditorGUILayout.PropertyField(m_abyssCheckSphereOffsetProp, m_abyssCheckSphereOffsetContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // PATROLLING OBSTACLE

        #region TARGETING
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_TargetingFoldout = EditorGUILayout.Foldout(m_TargetingFoldout, m_TargetingContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_TargetingFoldout)
        {
            EditorGUILayout.PropertyField(m_viewPointHeightProp, m_viewPointHeightContent);
            EditorGUILayout.PropertyField(m_viewDirectionProp, m_viewDirectionContent);
            EditorGUILayout.PropertyField(m_viewFovProp, m_viewFovContent);
            EditorGUILayout.PropertyField(m_viewDistanceProp, m_viewDistanceContent);
            EditorGUILayout.PropertyField(m_timeBeforeLostTargetProp, m_timeBeforeLostTargetContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // TARGETING

        #region ATTACKING
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_AttackingFoldout = EditorGUILayout.Foldout(m_AttackingFoldout, m_AttackingContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_AttackingFoldout)
        {
            EditorGUILayout.PropertyField(m_attackObstacleMaskProp, m_attackObstacleMaskContent);
            EditorGUILayout.PropertyField(m_attackMaskProp, m_attackMaskContent);
            EditorGUILayout.PropertyField(m_attackOffsetProp, m_attackOffsetContent);
            EditorGUILayout.PropertyField(m_attackRangeProp, m_attackRangeContent);
            EditorGUILayout.PropertyField(m_attackRadiusProp, m_attackRadiusContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // ATTACKING

        #region SPECIFICS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_SpecificsFoldout = EditorGUILayout.Foldout(m_SpecificsFoldout, m_SpecificsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_SpecificsFoldout)
        {
            EnemyBaseBehaviour enemyBase = (EnemyBaseBehaviour)target;
            if (enemyBase as FlyingAndWalkingEnemy)
            {
                EditorGUILayout.PropertyField(m_TargetPointProp, m_TargetPointContent);
                EditorGUILayout.PropertyField(m_TargetPointYOffsetProp, m_TargetPointYOffsetContent);
                EditorGUILayout.PropertyField(m_minWaitTimeForNosediveProp, m_minWaitTimeForNosediveContent);
                EditorGUILayout.PropertyField(m_maxWaitTimeForNosediveProp, m_maxWaitTimeForNosediveContent);
                EditorGUILayout.PropertyField(m_noseDiveSpeedProp, m_noseDiveSpeedContent);
            }
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // SPECIFICS

        #region CONTROL PANEL
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_ControlPanelFoldout = EditorGUILayout.Foldout(m_ControlPanelFoldout, m_ControlPanelContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_ControlPanelFoldout)
        {
            EditorGUILayout.PropertyField(m_showViewFieldProp, m_showViewFieldContent);
            EditorGUILayout.PropertyField(m_colorViewFieldProp, m_colorViewFieldContent);
            EditorGUILayout.PropertyField(m_showPatrolTargetPositionProp, m_showPatrolTargetPositionContent);
            EditorGUILayout.PropertyField(m_colorPatrolTargetPositionProp, m_colorPatrolTargetPositionContent);
            EditorGUILayout.PropertyField(m_showGroundDetectSphereProp, m_showGroundDetectSphereContent);
            EditorGUILayout.PropertyField(m_colorGroundDetectSphereProp, m_colorGroundDetectSphereContent);
            EditorGUILayout.PropertyField(m_showAttackPropertiesProp, m_showAttackPropertiesContent);
            EditorGUILayout.PropertyField(m_colorAttackFieldProp, m_colorAttackFieldContent);
            EditorGUILayout.PropertyField(m_attackTargetProp, m_attackTargetContent);
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // CONTROL PANEL

        serializedObject.ApplyModifiedProperties();
    }
}