using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dante.DungeonCrawler
{
    #region Structs

    public enum AgentType { DestroyableObject, EnemyNPC, PlayersAvatar}

    [System.Serializable]
    public struct HurtBoxValues
    {
        public int maxHealthPoints;
        public float cooldownPerHit;
    }

    #endregion


    [CreateAssetMenu(menuName = "Dungeon Crawler/New HurtBox Values")]
    public class HurtBox_ScriptableObject : ScriptableObject
    {
        [SerializeField] public AgentType agentType;
        [SerializeField] public HurtBoxValues hurtBoxValues;
    }

}
