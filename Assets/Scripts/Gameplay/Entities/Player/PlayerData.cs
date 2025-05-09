using Spark.Gameplay.Entities.Common.Abilities;
using Spark.Gameplay.Entities.Common.Behaviour;
using Spark.Gameplay.Entities.Common.Data;
using Spark.Gameplay.Weapons.MeleeWeapon;
using Spark.Gameplay.Weapons.RangedWeapon;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Spark.Gameplay.Entities.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player Save Data", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public int Details;
        public FlashCard.FlashCard FlashCard;

        public List<MeleeWeaponData> MeleeWeaponData;
        public List<RangedWeaponData> RangedWeaponData;

        private MedKitAbility _medKits;

        public void SaveWithMedKits(MedKitAbility midKitAbility)
        {
            _medKits = midKitAbility;

            string path = Application.persistentDataPath + "/playerData.json";
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        public void LoadWithMedKits(out MedKitAbility midKitAbility)
        {
            string path = Application.persistentDataPath + "/playerData.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, this);
            }
            midKitAbility = _medKits;
        }

        private void OnValidate()
        {
            Details = 0;
        }
    }
}