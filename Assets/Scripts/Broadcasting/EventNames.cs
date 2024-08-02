using UnityEngine;
using System.Collections;

/*
 * Holder for event names
 * Created By: NeilDG
 */ 
public class EventNames {
	public class DiceEvents
	{
        public const string ON_DIFFICULTY_CLASS_CHANGE = "ON_DIFFICULTY_CLASS_CHANGE";
        public const string ON_DICE_RESULT = "ON_DICE_RESULT";
        public const string ADD_MODIFIER = "ADD_MODIFIER";
    }

    public class QuestProgression
    {
        public const string ON_MQ1_COMPLETE = "ON_MQ1_COMPLETE";
        public const string ON_MQ2_COMPLETE = "ON_MQ2_COMPLETE";
        public const string ON_MQ3_COMPLETE = "ON_MQ3_COMPLETE";

        public const string ON_SQ1_COMPLETE = "ON_SQ1_COMPLETE";
        public const string ON_SQ2_COMPLETE = "ON_SQ2_COMPLETE";
        public const string ON_SQ3_COMPLETE = "ON_sQ3_COMPLETE";
    }

    public class Combat
    {
        public const string ON_ENEMY_KILLED = "ON_ENEMY_KILLED";
        public const string ON_ALLY_KILLED = "ON_ALLY_KILLED";
    }
}