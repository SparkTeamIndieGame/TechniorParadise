using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Items;
using Spark.Gameplay.RefactoredPlayer.RefactoredSystems.Weapons.Melee;
using Spark.Refactored.Gameplay.Entities.Player.MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Spark.Refactored.Gameplay.System.Checkpoint
{
    public class CheckpointManager : MonoBehaviour, ICollectable
    {
        private const string CheckpointDataKey = "CheckpointData";

        public void Activate(View player)
        {
            player.OnCheckpointPickUped.Invoke();
        }

        public void Deactivate()
        {

        }

        public void SaveCheckpoint(Model playerModel)
        {
            CheckpointData checkpointData = new CheckpointData(playerModel);
            string json = JsonUtility.ToJson(checkpointData);
            PlayerPrefs.SetString(CheckpointDataKey, json);
            PlayerPrefs.Save();

            Debug.Log(json);
        }

        public void LoadCheckpoint(Model playerModel)
        {
            if (PlayerPrefs.HasKey(CheckpointDataKey))
            {
                string json = PlayerPrefs.GetString(CheckpointDataKey);
                CheckpointData checkpointData = JsonUtility.FromJson<CheckpointData>(json);
                checkpointData.Restore(playerModel);
            }
        }
    }

    [Serializable]
    public class CheckpointData
    {
        [SerializeField] private float _health;
        [SerializeField] private float _details;

        [SerializeField] private string[] _meleeTypes;
        [SerializeField] private string[] _rangedTypes;

        [SerializeField] private int _currentMeleeIndex;
        [SerializeField] private int _currentRangedIndex;

        public CheckpointData(Model playerModel)
        {
            _health = playerModel.health;
            _details = playerModel.details;

            var meleeTypes = playerModel.meleeTypes.GetAllTypes();
            _meleeTypes = new string[meleeTypes.Count];
            for (int index = 0; index < meleeTypes.Count; ++index)
            {
                _meleeTypes[index] = meleeTypes[index].ToString();
            }
            _currentMeleeIndex = playerModel.meleeTypes.GetCurrentIndex();
            
            var rangedTypes = playerModel.rangedTypes.GetAllTypes();
            _rangedTypes = new string[rangedTypes.Count];
            for (int index = 0; index < rangedTypes.Count; ++index)
            {
                _rangedTypes[index] = rangedTypes[index].ToString();
            }
            _currentMeleeIndex = playerModel.rangedTypes.GetCurrentIndex();
        }

        public void Restore(Model playerModel)
        {
            playerModel.health = _health;
            playerModel.details = _details;

            playerModel.meleeTypes.Clear();
            foreach (var typeName in _meleeTypes)
            {
                if (Enum.TryParse(typeName, out MeleeWeaponType type))
                {
                    playerModel.meleeTypes.TryAddNewType(type);
                }
            }
            playerModel.meleeTypes.SetCurrentIndex(_currentMeleeIndex);
        }
    }
}