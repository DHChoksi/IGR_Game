using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public static class Constants
    {
        public enum TrashType
        {
            Normal,
            Radioactive,
            Explosive
        }

        public enum LR_Device
        {
            None = 0,
            L_Device,
            R_Device
        }

        public enum ControlState
        {
            Pressed = 1,
            NotPressed = 0
        }


        public enum DrownState 
        { 
            Move, 
            PlayerDetected, 
            Attack, 
            Death 
        }

        public enum ControlType
        {
            None,
            PrimaryButton,
            SecondaryButton,
            Grip,
            Trigger,
            Joystick
        }

        public enum SceneName
        {
            StoryBoard,
            Phase_1,
            Instruction,
            Phase_2
        }

        public enum VideoName
        {
            None = 0,
            IGR_Intro
        }

        public enum SpeakerName
        {
            Kaara = 0,
            Ranger
        }

         public enum TaskDone
        {
            Done,
            NotDone = 0
        }

        public enum GripAction
        {
            None,
            WebSwinging,
            HyperHook
        }

        public enum SFXType
        {
            ButtonClick,
            GunShoot,
            Jetpack,
            Blast,
            Grab,
            Shoot,
            Hurt,
            CollectObject
        }

        public enum BGMType
        {
            Menu,
            Training,
            Gameplay
        }

        public static float MAX_HIT_DETECT_DISTANCE = 1000000f;
        public static float TRASH_DETECT_DISTANCE = 1000000f;
        public static float ENEMY_DETECT_DISTANCE = 1000000f; 
        public static float PLATFORM_DETECT_DISTANCE = 1000000f;
        public static float CORE_DETECT_DISTANCE = 100000f;
    }
}