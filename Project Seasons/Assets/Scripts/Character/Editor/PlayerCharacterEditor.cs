using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCharacter))]
public class PlayerCharacterEditor : Editor
{
    #region PROPS

    #region REFERENCES
    SerializedProperty m_SpriteRendererProp;
    SerializedProperty m_HealthControllerProp;
    SerializedProperty m_AetherControllerProp;
    SerializedProperty m_FacingLeftBulletSpawnProp;
    SerializedProperty m_FacingRightBulletSpawnProp;
    SerializedProperty m_BulletPoolLightAttackProp;
    SerializedProperty m_BulletPoolHeavyAttackProp;
    SerializedProperty m_BulletPoolSuperAttackProp;
    #endregion // REFERENCES

    #region MOVEMENT SETTINGS
    SerializedProperty m_MoveSpeedWalkProp;
    SerializedProperty m_MoveSpeedRunProp;
    SerializedProperty m_GroundAccelerationProp;
    SerializedProperty m_GroundDecelerationProp;
    SerializedProperty m_PushingSpeedProportionProp;
    #endregion // MOVEMENT SETTINGS

    #region AIRNORN SETTINGS
    SerializedProperty m_AirborneAccelProportionProp;
    SerializedProperty m_AirborneDecelProportionProp;
    SerializedProperty m_GravityProp;
    SerializedProperty m_LimitYGravityProp;
    SerializedProperty m_JumpSpeedProp;
    SerializedProperty m_JumpAbortSpeedReductionProp;
    #endregion // AIRNORN SETTINGS

    #region DASHING SETTINGS
    SerializedProperty m_DashSpeedProp;
    SerializedProperty m_DashAccelerationProp;
    SerializedProperty m_DashCooldownTimeProp;
    SerializedProperty m_DashOnChangeEventProp;
    #endregion // DASHING SETTINGS

    #region HEALTH
    SerializedProperty m_HealPotionValueProp;
    SerializedProperty m_MaxHealthPotionProp;
    SerializedProperty m_OnHealthEventProp;
    #endregion // HEALTH

    #region AETHER
    SerializedProperty m_AetherPotionValueProp;
    SerializedProperty m_MaxAetherPotionProp;
    SerializedProperty m_OnAetherEventProp;
    #endregion // AETHER

    #region HURT SETTINGS
    SerializedProperty m_HurtJumpAngleProp;
    SerializedProperty m_HurtJumpSpeedProp;
    SerializedProperty m_FlickeringDurationProp;
    #endregion // HURT SETTINGS

    #region AUDIO SETTINGS
    SerializedProperty m_FootstepAudioPlayerProp;
    SerializedProperty m_JumpingAudioPlayerProp;
    SerializedProperty m_LandingAudioPlayerProp;
    SerializedProperty m_HurtAudioPlayerProp;
    SerializedProperty m_FocusedAudioPlayerProp;
    SerializedProperty m_HeavyAttackAudioPlayerProp;
    SerializedProperty m_SuperAttackAudioPlayerProp;
    #endregion // AUDIO SETTINGS

    #region ATTACKING SETTINGS
    SerializedProperty m_LightAttackCadenceProp;
    SerializedProperty m_LightAttackBulletSpeedProp;
    SerializedProperty m_HeavyAttackBulletSpeedProp;
    SerializedProperty m_HeavyAttackTimeBeforeShotProp;
    SerializedProperty m_SuperAttackBulletSpeedProp;
    SerializedProperty m_SuperAttackTimeBeforeShotProp;
    #endregion // ATTACKING SETTINGS

    #region PARTICLE SYSTEMS
    SerializedProperty m_ParticleFocusedProp;
    SerializedProperty m_ParticleLightAttackProp;
    SerializedProperty m_ParticleForceFieldProp;
    SerializedProperty m_JumpDustPositionOffsetProp;
    SerializedProperty m_LandDustPositionOffsetProp;
    #endregion // PARTICLE SYSTEMS

    #endregion // PROPS

    #region CONTENT

    #region REFERENCES
    readonly GUIContent m_SpriteRendererContent = new("Sprite Renderer");
    readonly GUIContent m_DamageControllerContent = new("Damage Controller");
    readonly GUIContent m_AetherControllerContent = new("Aether Controller");
    readonly GUIContent m_FacingLeftBulletSpawnContent = new("Facing Left Bullet Spawn Point");
    readonly GUIContent m_FacingRightBulletSpawnContent = new("Facing Rights Bullet Spawn Point");
    readonly GUIContent m_BulletPoolLightAttackContent = new("Bullet Pool Light Attack");
    readonly GUIContent m_BulletPoolHeavyAttackContent = new("Bullet Pool Heavy Attack");
    readonly GUIContent m_BulletPoolSuperAttackContent = new("Bullet Pool Super Attack");
    #endregion // REFERENCES

    #region MOVEMENT SETTINGS
    readonly GUIContent m_MoveSpeedWalkContent = new("Walk Speed");
    readonly GUIContent m_MoveSpeedRunContent = new("Run Speed");
    readonly GUIContent m_GroundAccelerationContent = new("Ground Acceleration");
    readonly GUIContent m_GroundDecelerationContent = new("Ground Deceleration");
    readonly GUIContent m_PushingSpeedProportionContent = new("Pushing Speed Proportion");
    #endregion // MOVEMENT SETTINGS

    #region AIRNORN SETTINGS
    readonly GUIContent m_AirborneAccelProportionContent = new("Airborne Accel Proportion");
    readonly GUIContent m_AirborneDecelProportionContent = new("Airborne Decel Proportion");
    readonly GUIContent m_GravityContent = new("Gravity");
    readonly GUIContent m_LimitYGravityContent = new("Gravity Y Limit");
    readonly GUIContent m_JumpSpeedContent = new("Jump Speed");
    readonly GUIContent m_JumpAbortSpeedReductionContent = new("Jump Abort Speed Reduction");
    #endregion // AIRNORN SETTINGS

    #region DASHING SETTINGS
    readonly GUIContent m_DashSpeedContent = new("Dash Speed");
    readonly GUIContent m_DashAccelerationContent = new("Dash Acceleration");
    readonly GUIContent m_DashCooldownTimeContent = new("Dash Cooldown Time");
    readonly GUIContent m_DashOnChangeEventContent = new("Dash On Change Event");
    #endregion // DASHING SETTINGS

    #region HEALTH
    readonly GUIContent m_HealPotionValueContent = new("Heal Potion Value");
    readonly GUIContent m_MaxHealthPotionContent = new("Max Health Potion In Inventory");
    readonly GUIContent m_OnHealthEventContent = new("On Health Event");
    #endregion // HEALTH

    #region AETHER
    readonly GUIContent m_AetherPotionValueContent = new("Aether Potion Value");
    readonly GUIContent m_MaxAetherPotionContent = new("Max Aether Potion In Inventory");
    readonly GUIContent m_OnAetherEventContent = new("On Aether Event");
    #endregion // AETHER

    #region HURT SETTINGS
    readonly GUIContent m_HurtJumpAngleContent = new("Hurt Jump Angle");
    readonly GUIContent m_HurtJumpSpeedContent = new("Hurt Jump Speed");
    readonly GUIContent m_FlickeringDurationContent = new("Flicking Duration", "When the player is hurt she becomes invulnerable for a short time and the SpriteRenderer flickers on and off to indicate this.  This field is the duration in seconds the SpriteRenderer stays either on or off whilst flickering.  To adjust the duration of invulnerability see the Damageable component.");
    #endregion // HURT SETTINGS

    #region AUDIO SETTINGS
    readonly GUIContent m_FootstepPlayerContent = new("Footstep Audio Player");
    readonly GUIContent m_JumpingAudioPlayerContent = new("Jumping Audio Player");
    readonly GUIContent m_LandingAudioPlayerContent = new("Landing Audio Player");
    readonly GUIContent m_HurtAudioPlayerContent = new("Hurt Audio Player");
    readonly GUIContent m_FocusedAudioPlayerContent = new("Focused Audio Player");
    readonly GUIContent m_HeavyAttackAudioPlayerContent = new("Heavy Attack Audio Player");
    readonly GUIContent m_SuperAttackAudioPlayerContent = new("Super Attack Audio Player");
    #endregion // AUDIO SETTINGS

    #region ATTACKING SETTINGS
    readonly GUIContent m_LightAttackCadenceContent = new("Light Attack Cadence");
    readonly GUIContent m_LightAttackBulletSpeedContent = new("Light Attack Bullet Speed");
    readonly GUIContent m_HeavyAttackBulletSpeedContent = new("Heavy Attack Bullet Speed");
    readonly GUIContent m_HeavyAttackTimeBeforeShotContent = new("Heavy Attack Time Before Shot");
    readonly GUIContent m_SuperAttackBulletSpeedContent = new("Super Attack Bullet Speed");
    readonly GUIContent m_SuperAttackTimeBeforeShotContent = new("Super Attack Time Before Shot");
    #endregion // ATTACKING SETTINGS

    #region PARTICLE SYSTEMS
    readonly GUIContent m_ParticleFocusedContent = new("Particle Focused");
    readonly GUIContent m_ParticleLightAttackContent = new("Particle Light Attack");
    readonly GUIContent m_ParticleForceFieldContent = new("Force Field Particle");
    readonly GUIContent m_JumpDustPositionOffsetContent = new("Jump Dust Position Offset");
    readonly GUIContent m_LandDustPositionOffsetContent = new("Land Dust Position Offset");
    #endregion // PARTICLE SYSTEMS

    #endregion // CONTENT

    #region TITLES
    readonly GUIContent m_ReferencesContent = new("References");
    readonly GUIContent m_MovementSettingsContent = new("Movement Settings");
    readonly GUIContent m_AirborneSettingsContent = new("Airborne Settings");
    readonly GUIContent m_DashingSettingsContent = new("Dashing Settings");
    readonly GUIContent m_HealingSettingsContent = new("Healing Settings");
    readonly GUIContent m_AetherSettingsContent = new("Aether Settings");
    readonly GUIContent m_HurtSettingsContent = new("Hurt Settings");
    readonly GUIContent m_AttackingSettingsContent = new("Attacking Settings");
    readonly GUIContent m_AudioSettingsContent = new("Audio Settings");
    readonly GUIContent m_ParticleSystemsSettingsContent = new("Particle Systems Settings");
    #endregion // TITLES

    bool m_ReferencesFoldout;
    bool m_MovementSettingsFoldout;
    bool m_AirborneSettingsFoldout;
    bool m_DashingSettingsFoldout;
    bool m_HealingSettingsFoldout;
    bool m_AetherSettingsFoldout;
    bool m_HurtSettingsFoldout;
    bool m_AttackingSettingsFoldout;
    bool m_AudioSettingsFoldout;
    bool m_ParticleSystemsSettingsFoldout;

    void OnEnable()
    {
        #region REFERENCES
        m_SpriteRendererProp = serializedObject.FindProperty("spriteRenderer");
        m_HealthControllerProp = serializedObject.FindProperty("healthController");
        m_AetherControllerProp = serializedObject.FindProperty("aetherController");
        m_FacingLeftBulletSpawnProp = serializedObject.FindProperty("facingLeftBulletSpawnPoint");
        m_FacingRightBulletSpawnProp = serializedObject.FindProperty("facingRightBulletSpawnPoint");
        m_BulletPoolLightAttackProp = serializedObject.FindProperty("bulletPoolLightAttack");
        m_BulletPoolHeavyAttackProp = serializedObject.FindProperty("bulletPoolHeavyAttack");
        m_BulletPoolSuperAttackProp = serializedObject.FindProperty("bulletPoolSuperAttack");
        #endregion // REFERENCES

        #region MOVEMENT SETTINGS
        m_MoveSpeedWalkProp = serializedObject.FindProperty("moveSpeedWalk");
        m_MoveSpeedRunProp = serializedObject.FindProperty("moveSpeedRun");
        m_GroundAccelerationProp = serializedObject.FindProperty("groundAcceleration");
        m_GroundDecelerationProp = serializedObject.FindProperty("groundDeceleration");
        m_PushingSpeedProportionProp = serializedObject.FindProperty("pushingSpeedProportion");
        #endregion // MOVEMENT SETTINGS

        #region AIRNORN SETTINGS
        m_AirborneAccelProportionProp = serializedObject.FindProperty("airborneAccelProportion");
        m_AirborneDecelProportionProp = serializedObject.FindProperty("airborneDecelProportion");
        m_GravityProp = serializedObject.FindProperty("gravity");
        m_LimitYGravityProp = serializedObject.FindProperty("limitYGravity");
        m_JumpSpeedProp = serializedObject.FindProperty("jumpSpeed");
        m_JumpAbortSpeedReductionProp = serializedObject.FindProperty("jumpAbortSpeedReduction");
        #endregion // AIRNORN SETTINGS

        #region DASHING SETTINGS
        m_DashSpeedProp = serializedObject.FindProperty("dashSpeed");
        m_DashAccelerationProp = serializedObject.FindProperty("dashAcceleration");
        m_DashCooldownTimeProp = serializedObject.FindProperty("dashCooldownTime");
        m_DashOnChangeEventProp = serializedObject.FindProperty("OnDashCooldownChanged");
        #endregion // DASHING SETTINGS

        #region HEALTHLING
        m_HealPotionValueProp = serializedObject.FindProperty("healPotionValue");
        m_MaxHealthPotionProp = serializedObject.FindProperty("maxHealthPotionInInventory");
        m_OnHealthEventProp = serializedObject.FindProperty("OnHealthItemCounterChanged");
        #endregion // HEALTHLING

        #region AETHER
        m_AetherPotionValueProp = serializedObject.FindProperty("aetherPotionValue");
        m_MaxAetherPotionProp = serializedObject.FindProperty("maxAetherPotionInInventory");
        m_OnAetherEventProp = serializedObject.FindProperty("OnAetherItemCounterChanged");
        #endregion // AETHER

        #region HURT SETTINGS
        m_HurtJumpAngleProp = serializedObject.FindProperty("hurtJumpAngle");
        m_HurtJumpSpeedProp = serializedObject.FindProperty("hurtJumpSpeed");
        m_FlickeringDurationProp = serializedObject.FindProperty("flickeringDuration");
        #endregion // HURT SETTINGS

        #region AUDIO SETTINGS
        m_FootstepAudioPlayerProp = serializedObject.FindProperty("footstepAudioPlayer");
        m_JumpingAudioPlayerProp = serializedObject.FindProperty("jumpingAudioPlayer");
        m_LandingAudioPlayerProp = serializedObject.FindProperty("landingAudioPlayer");
        m_HurtAudioPlayerProp = serializedObject.FindProperty("hurtAudioPlayer");
        m_FocusedAudioPlayerProp = serializedObject.FindProperty("focusedAudioPlayer");
        m_HeavyAttackAudioPlayerProp = serializedObject.FindProperty("heavyAttackAudioPlayer");
        m_SuperAttackAudioPlayerProp = serializedObject.FindProperty("superAttackAudioPlayer");
        #endregion // AUDIO SETTINGS

        #region ATTACKING SETTINGS
        m_LightAttackCadenceProp = serializedObject.FindProperty("lightAttackCadence");
        m_LightAttackBulletSpeedProp = serializedObject.FindProperty("lightAttackBulletSpeed");
        m_HeavyAttackBulletSpeedProp = serializedObject.FindProperty("heavyAttackBulletSpeed");
        m_HeavyAttackTimeBeforeShotProp = serializedObject.FindProperty("heavyAttackTimeBeforeShot");
        m_SuperAttackBulletSpeedProp = serializedObject.FindProperty("superAttackBulletSpeed");
        m_SuperAttackTimeBeforeShotProp = serializedObject.FindProperty("superAttackTimeBeforeShot");
        #endregion // ATTACKING SETTINGS

        #region PARTICLE SYSTEMS
        m_ParticleFocusedProp = serializedObject.FindProperty("particleFocused");
        m_ParticleLightAttackProp = serializedObject.FindProperty("particleLightAttack");
        m_ParticleForceFieldProp = serializedObject.FindProperty("particleForceField");
        m_JumpDustPositionOffsetProp = serializedObject.FindProperty("jumpDustPositionOffset");
        m_LandDustPositionOffsetProp = serializedObject.FindProperty("landDustPositionOffset");
        #endregion // PARTICLE SYSTEMS
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

        EditorGUILayout.Space();
        m_ReferencesFoldout = EditorGUILayout.Foldout(m_ReferencesFoldout, m_ReferencesContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_ReferencesFoldout)
        {
            EditorGUILayout.PropertyField(m_SpriteRendererProp, m_SpriteRendererContent);
            EditorGUILayout.PropertyField(m_HealthControllerProp, m_DamageControllerContent);
            EditorGUILayout.PropertyField(m_AetherControllerProp, m_AetherControllerContent);
            EditorGUILayout.PropertyField(m_FacingLeftBulletSpawnProp, m_FacingLeftBulletSpawnContent);
            EditorGUILayout.PropertyField(m_FacingRightBulletSpawnProp, m_FacingRightBulletSpawnContent);
            EditorGUILayout.PropertyField(m_BulletPoolLightAttackProp, m_BulletPoolLightAttackContent);
            EditorGUILayout.PropertyField(m_BulletPoolHeavyAttackProp, m_BulletPoolHeavyAttackContent);
            EditorGUILayout.PropertyField(m_BulletPoolSuperAttackProp, m_BulletPoolSuperAttackContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // REFERENCES

        #region MOVEMENT SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_MovementSettingsFoldout = EditorGUILayout.Foldout(m_MovementSettingsFoldout, m_MovementSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_MovementSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_MoveSpeedWalkProp, m_MoveSpeedWalkContent);
            EditorGUILayout.PropertyField(m_MoveSpeedRunProp, m_MoveSpeedRunContent);
            EditorGUILayout.PropertyField(m_GroundAccelerationProp, m_GroundAccelerationContent);
            EditorGUILayout.PropertyField(m_GroundDecelerationProp, m_GroundDecelerationContent);
            EditorGUILayout.PropertyField(m_PushingSpeedProportionProp, m_PushingSpeedProportionContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // MOVEMENT SETTINGS

        #region AIRNORN SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_AirborneSettingsFoldout = EditorGUILayout.Foldout(m_AirborneSettingsFoldout, m_AirborneSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_AirborneSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_AirborneAccelProportionProp, m_AirborneAccelProportionContent);
            EditorGUILayout.PropertyField(m_AirborneDecelProportionProp, m_AirborneDecelProportionContent);
            EditorGUILayout.PropertyField(m_GravityProp, m_GravityContent);
            EditorGUILayout.PropertyField(m_LimitYGravityProp, m_LimitYGravityContent);
            EditorGUILayout.PropertyField(m_JumpSpeedProp, m_JumpSpeedContent);
            EditorGUILayout.PropertyField(m_JumpAbortSpeedReductionProp, m_JumpAbortSpeedReductionContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // AIRNORN SETTINGS

        #region DASHING SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_DashingSettingsFoldout = EditorGUILayout.Foldout(m_DashingSettingsFoldout, m_DashingSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_DashingSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_DashSpeedProp, m_DashSpeedContent);
            EditorGUILayout.PropertyField(m_DashAccelerationProp, m_DashAccelerationContent);
            EditorGUILayout.PropertyField(m_DashCooldownTimeProp, m_DashCooldownTimeContent);
            EditorGUILayout.PropertyField(m_DashOnChangeEventProp, m_DashOnChangeEventContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // DASHING SETTINGS

        #region HEALING SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_HealingSettingsFoldout = EditorGUILayout.Foldout(m_HealingSettingsFoldout, m_HealingSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_HealingSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_HealPotionValueProp, m_HealPotionValueContent);
            EditorGUILayout.PropertyField(m_MaxHealthPotionProp, m_MaxHealthPotionContent);
            EditorGUILayout.PropertyField(m_OnHealthEventProp, m_OnHealthEventContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // HEALING SETTINGS

        #region AETHER SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_AetherSettingsFoldout = EditorGUILayout.Foldout(m_AetherSettingsFoldout, m_AetherSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_AetherSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_AetherPotionValueProp, m_AetherPotionValueContent);
            EditorGUILayout.PropertyField(m_MaxAetherPotionProp, m_MaxAetherPotionContent);
            EditorGUILayout.PropertyField(m_OnAetherEventProp, m_OnAetherEventContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // AETHER SETTINGS

        #region HURT SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_HurtSettingsFoldout = EditorGUILayout.Foldout(m_HurtSettingsFoldout, m_HurtSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_HurtSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_HurtJumpAngleProp, m_HurtJumpAngleContent);
            EditorGUILayout.PropertyField(m_HurtJumpSpeedProp, m_HurtJumpSpeedContent);
            EditorGUILayout.PropertyField(m_FlickeringDurationProp, m_FlickeringDurationContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // HURT SETTINGS

        #region AUDIO SYSTEMS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_AudioSettingsFoldout = EditorGUILayout.Foldout(m_AudioSettingsFoldout, m_AudioSettingsContent, boldFoldoutStyle);

        if (m_AudioSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_FootstepAudioPlayerProp, m_FootstepPlayerContent);
            EditorGUILayout.PropertyField(m_JumpingAudioPlayerProp, m_JumpingAudioPlayerContent);
            EditorGUILayout.PropertyField(m_LandingAudioPlayerProp, m_LandingAudioPlayerContent);
            EditorGUILayout.PropertyField(m_HurtAudioPlayerProp, m_HurtAudioPlayerContent);
            EditorGUILayout.PropertyField(m_FocusedAudioPlayerProp, m_FocusedAudioPlayerContent);
            EditorGUILayout.PropertyField(m_HeavyAttackAudioPlayerProp, m_HeavyAttackAudioPlayerContent);
            EditorGUILayout.PropertyField(m_SuperAttackAudioPlayerProp, m_SuperAttackAudioPlayerContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // AUDIO SYSTEMS

        #region ATTACKING SETTINGS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_AttackingSettingsFoldout = EditorGUILayout.Foldout(m_AttackingSettingsFoldout, m_AttackingSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_AttackingSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_LightAttackCadenceProp, m_LightAttackCadenceContent);
            EditorGUILayout.PropertyField(m_LightAttackBulletSpeedProp, m_LightAttackBulletSpeedContent);
            EditorGUILayout.PropertyField(m_HeavyAttackBulletSpeedProp, m_HeavyAttackBulletSpeedContent);
            EditorGUILayout.PropertyField(m_HeavyAttackTimeBeforeShotProp, m_HeavyAttackTimeBeforeShotContent);
            EditorGUILayout.PropertyField(m_SuperAttackBulletSpeedProp, m_SuperAttackBulletSpeedContent);
            EditorGUILayout.PropertyField(m_SuperAttackTimeBeforeShotProp, m_SuperAttackTimeBeforeShotContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // ATTACKING SETTINGS

        #region PARTICLE SYSTEMS
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;

        EditorGUILayout.Space();
        m_ParticleSystemsSettingsFoldout = EditorGUILayout.Foldout(m_ParticleSystemsSettingsFoldout, m_ParticleSystemsSettingsContent, boldFoldoutStyle);
        EditorGUILayout.Space();

        if (m_ParticleSystemsSettingsFoldout)
        {
            EditorGUILayout.PropertyField(m_ParticleFocusedProp, m_ParticleFocusedContent);
            EditorGUILayout.PropertyField(m_ParticleLightAttackProp, m_ParticleLightAttackContent);
            EditorGUILayout.PropertyField(m_ParticleForceFieldProp, m_ParticleForceFieldContent);
            EditorGUILayout.PropertyField(m_JumpDustPositionOffsetProp, m_JumpDustPositionOffsetContent);
            EditorGUILayout.PropertyField(m_LandDustPositionOffsetProp, m_LandDustPositionOffsetContent);
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        #endregion // PARTICLE SYSTEMS

        serializedObject.ApplyModifiedProperties();
    }
}
