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
        public const string ON_DICE_DONE = "ON_DICE_DONE";
        public const string ON_DICE_RESULT = "ON_DICE_RESULT";
        public const string DICE_ADS = "DICE_ADS";
        public const string ADD_MODIFIER = "ADD_MODIFIER";
    }

    public class DialogueEvents
    {
        public const string ON_QUEST_PROGRESSION = "ON_QUEST_PROGRESSION";
        public const string ON_RECRUIT = "ON_RECRUIT";
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
}