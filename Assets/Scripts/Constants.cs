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

        public enum PlayerType
        {
            MotionSickGamer,
            ProGamer
        }

        public enum ControllerGrappleState
        {
            None,
            Press,
            Hold,
            Release
        }

        public enum GripState
        {
            Press,
            Hold,
            Release
        }

        public static string CURRENT_PLAYER_TYPE = "PlayerType";

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
            MainMenu = 0,
            StoryBoard,
            Training,
            Gameplay

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

        public enum CrosshairState
        {
            defaultState,
            TargetState
        }


        public enum GripAction
        {
            None,
            WebSwinging,
            HyperHook
        }

        public enum DrownState
        {
            Move, 
            PlayerDetected, 
            Attack, 
            Death 
        }

        public static float MAX_HIT_DETECT_DISTANCE = 1000000f;
        public static float TRASH_DETECT_DISTANCE = 1000000f;
        public static float ENEMY_DETECT_DISTANCE = 1000000f; 
        public static float PLATFORM_DETECT_DISTANCE = 1000000f;
        public static float CORE_DETECT_DISTANCE = 100000f;
    }
}