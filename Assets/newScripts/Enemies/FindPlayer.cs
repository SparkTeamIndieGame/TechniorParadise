
using UnityEngine;

public class FindPlayer : MonoBehaviour
{
   public Transform _player;
   private void OnTriggerEnter(Collider other)
   {
      _player = other.gameObject.transform;
      enemyTEST.isFindPlayer = true;
   }

   private void OnTriggerExit(Collider other)
   {
      _player = null;
      enemyTEST.isFindPlayer = false;
   }
}
