using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NPCEmotions
{
    [System.Serializable]
    public class EmotionalDisposition
    {
        [Header("Emotional Thresholds")]
        public Vector2 happinessThresholds, stressThresholds, shockThresholds; // minimum threshold for npc to feel this emotion

        public Emotion emotionalDisposition;

        [Range(-1,1)] public float extroversion;
        [Range(5f, 25f)] public float attentionSpan;

        //public Personality(Vector2 happinessMinThresholds, Vector2 stressMinThresholds, Vector2 shockMinThresholds)
        //{
        //    this.happinessMinThresholds.x = happinessMinThresholds.x; // sadness
        //    this.happinessMinThresholds.y = happinessMinThresholds.y; // happiness
        //    this.stressMinThresholds.x = stressMinThresholds.x; // nervous
        //    this.stressMinThresholds.y = stressMinThresholds.y; // anger
        //    this.shockMinThresholds.x = shockMinThresholds.x; // fear
        //    this.shockMinThresholds.y = shockMinThresholds.y; // surprise
        //}

    }
    public EmotionalDisposition disposition;

    [System.Serializable]
    public enum Emotion
    {
        calm, happy, sad, angry, nervous, scared, surprised, alert, excited, delighted,
        brave, grumpy, worried, dismayed, fearful, startled, upset, alarmed, horrified, overwhelmed
    }

    [System.Serializable]
    public struct NPCCurrentEmotion
    {
        public Emotion npcEmotion;


        public float happiness;
        public float stress;
        public float shock;

        public NPCCurrentEmotion(Emotion npcMood, float happiness, float stress, float shock)
        {
            this.npcEmotion = npcMood;

            this.happiness = happiness;// happy & sad emotions
            this.stress = stress; // angry and nervous emotions
            this.shock = shock; // surprised & scared emotions
        }
    }

    public NPCCurrentEmotion emotion;


    public Emotion GetMood(NPCCurrentEmotion emotion, EmotionalDisposition disposition)
    {
        SetMood(emotion, disposition);
        return GetStrongestEmotion(emotion, disposition);
    }

    public void SetMood(NPCCurrentEmotion emotion, EmotionalDisposition disposition)
    {
        emotion.npcEmotion = GetStrongestEmotion(emotion, disposition);
        //Debug.Log("Mood has been set");
    }

    public Emotion SetEmotionalDisposition(Vector2 happinessThresholds, Vector2 stressThresholds, Vector2 shockThresholds)
    {
        NPCCurrentEmotion personalityEmotions = new NPCCurrentEmotion();

        personalityEmotions.happiness = happinessThresholds.x + happinessThresholds.y;
        personalityEmotions.stress = stressThresholds.x + stressThresholds.y;
        personalityEmotions.shock = shockThresholds.x + shockThresholds.y;

        //personalityEmotions.happiness *= Mathf.Abs(personalityEmotions.happiness);
        //personalityEmotions.stress *= Mathf.Abs(personalityEmotions.stress);
        //personalityEmotions.shock *= Mathf.Abs(personalityEmotions.shock);

        if (Mathf.Abs(personalityEmotions.happiness) < Mathf.Abs(personalityEmotions.stress) && Mathf.Abs(personalityEmotions.happiness) < Mathf.Abs(personalityEmotions.shock))
        {
            if (personalityEmotions.happiness > 0)
            {
                personalityEmotions.happiness += 10f;
            }
            else
            {
                personalityEmotions.happiness -= 10f;
            }

            if (Mathf.Abs(personalityEmotions.shock) < Mathf.Abs(personalityEmotions.stress))
            {
                if (personalityEmotions.shock > 0)
                {
                    personalityEmotions.shock += 5f;
                }
                else
                {
                    personalityEmotions.shock -= 5f;
                }
            }
            else
            {
                if (personalityEmotions.stress > 0)
                {
                    personalityEmotions.stress += 5f;
                }
                else
                {
                    personalityEmotions.stress -= 5f;
                }
            }
        }
        else if (Mathf.Abs(personalityEmotions.stress) < Mathf.Abs(personalityEmotions.happiness) && Mathf.Abs(personalityEmotions.stress) < Mathf.Abs(personalityEmotions.shock))
        {
            if (personalityEmotions.stress > 0)
            {
                personalityEmotions.stress += 10f;
            }
            else
            {
                personalityEmotions.stress -= 10f;
            }

            if (Mathf.Abs(personalityEmotions.shock) < Mathf.Abs(personalityEmotions.happiness))
            {
                if (personalityEmotions.shock > 0)
                {
                    personalityEmotions.shock += 5f;
                }
                else
                {
                    personalityEmotions.shock -= 5f;
                }
            }
            else
            {
                if (personalityEmotions.happiness > 0)
                {
                    personalityEmotions.happiness += 5f;
                }
                else
                {
                    personalityEmotions.happiness -= 5f;
                }
            }
        }
        else if (Mathf.Abs(personalityEmotions.shock) < Mathf.Abs(personalityEmotions.happiness) && Mathf.Abs(personalityEmotions.shock) < Mathf.Abs(personalityEmotions.stress))
        {
            if (personalityEmotions.shock > 0)
            {
                personalityEmotions.shock += 10f;
            }
            else
            {
                personalityEmotions.shock -= 10f;
            }

            if (Mathf.Abs(personalityEmotions.stress) < Mathf.Abs(personalityEmotions.happiness))
            {
                if (personalityEmotions.stress > 0)
                {
                    personalityEmotions.stress += 5f;
                }
                else
                {
                    personalityEmotions.stress -= 5f;
                }
            }
            else
            {
                if (personalityEmotions.happiness > 0)
                {
                    personalityEmotions.happiness += 5f;
                }
                else
                {
                    personalityEmotions.happiness -= 5f;
                }
            }
        }

        return GetStrongestEmotion(personalityEmotions, disposition);
    }

    public Emotion GetStrongestEmotion(NPCCurrentEmotion emotion, EmotionalDisposition disposition)
    {
        if (CheckEmotionThreshold(emotion.happiness, disposition.happinessThresholds.x))
        {
            if (CheckEmotionThreshold(emotion.stress, disposition.stressThresholds.x))
            {
                if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
                {
                    //happy & angry & surprised
                    return Emotion.overwhelmed;
                }
                else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
                {
                    //happy & angry & scared
                    return Emotion.overwhelmed;

                }
                // happy & angry
                return Emotion.alert;
            }
            else if (CheckEmotionThreshold(emotion.stress, disposition.stressThresholds.y))
            {
                if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
                {
                    //happy & nervous & surprised
                    return Emotion.overwhelmed;

                }
                else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
                {
                    //happy & nervous & scared
                    return Emotion.overwhelmed;

                }
                // happy & nervous
                return Emotion.excited;

            }
            if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
            {
                // happy & surprised
                return Emotion.delighted;
            }
            else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
            {
                // happy & scared
                return Emotion.brave;
            }

            // just happy
            return Emotion.happy;
        }
        else if (CheckEmotionThreshold(emotion.happiness, disposition.happinessThresholds.y))
        {
            if (CheckEmotionThreshold(emotion.stress, disposition.stressThresholds.x))
            {
                if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
                {
                    //sad & angry & surprised
                    return Emotion.overwhelmed;

                }
                else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
                {
                    //sad & angry & scared
                    return Emotion.overwhelmed;


                }
                // sad & angry
                return Emotion.grumpy;
            }
            else if (CheckEmotionThreshold(emotion.stress, disposition.stressThresholds.y))
            {
                if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
                {
                    //sad & nervous & surprised
                    return Emotion.overwhelmed;


                }
                else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
                {
                    //sad & nervous & scared
                    return Emotion.overwhelmed;


                }
                // sad & nervous
                return Emotion.worried;

            }
            if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
            {
                // sad & surprised
                return Emotion.dismayed;
            }
            else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
            {
                // sad & scared
                return Emotion.fearful;
            }
            // just sad
            return Emotion.sad;
        }
        else if (CheckEmotionThreshold(emotion.stress, disposition.stressThresholds.x))
        {
            if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
            {
                // angry and surprised
                return Emotion.startled;
            }
            else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
            {
                //angry and scared
                return Emotion.upset;
            }
            // just angry
            return Emotion.angry;
        }
        else if (CheckEmotionThreshold(emotion.stress, disposition.stressThresholds.y))
        {
            if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
            {
                // nervous and surprised
                return Emotion.alarmed;
            }
            else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
            {
                //nervous and scared
                return Emotion.horrified;
            }
            //just nervous
            return Emotion.nervous;
        }
        else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.x))
        {
            // just surprised
            return Emotion.surprised;
        }
        else if (CheckEmotionThreshold(emotion.shock, disposition.shockThresholds.y))
        {
            //just scared
            return Emotion.scared;
        }

        //no strong emotions
        return Emotion.calm;
    }

    public bool CheckEmotionThreshold(float emotion, float emotionThreshold)
    {
        if (emotionThreshold > 0)
        {
            if (emotion > emotionThreshold)
            {
                return true;
            }
        }
        else
        {
            if (emotion < emotionThreshold)
            {
                return true;
            }
        }

        return false;
    }
}

