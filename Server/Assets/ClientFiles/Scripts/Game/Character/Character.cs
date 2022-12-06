using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "CharacterData")]
public class Character : ScriptableObject
{
    [System.Serializable]
    public struct CharacterData
    {
        public string characterName;
        public GameObject characterModel;
        public int maxHealth;
        public float moveSpeed;
        public float attackSpeed;
        public int attackDamage;
    }

    public CharacterData characterData;
}
