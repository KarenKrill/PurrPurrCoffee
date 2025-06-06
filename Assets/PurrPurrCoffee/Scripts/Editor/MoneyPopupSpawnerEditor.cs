using UnityEditor;
using UnityEngine;

namespace PurrPurrCoffee.Editor
{

    [CustomEditor(typeof(MoneyPopupSpawner))]
    public class MoneyPopupSpawnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("SpawnMoney"))
            {
                _moneyPopupSpawner.ShowMoneyPopupDebug(13);
            }
        }
        private MoneyPopupSpawner _moneyPopupSpawner => (MoneyPopupSpawner)target;
    }
}
