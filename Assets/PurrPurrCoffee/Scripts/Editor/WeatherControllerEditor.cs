using UnityEditor;
using UnityEngine;

namespace PurrPurrCoffee.Editor
{
    [CustomEditor(typeof(WeatherController))]
    public class WeatherControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("PlayLightingStrike"))
            {
                _weatherController.LightningStrike();
            }
            if (GUILayout.Button("PlayRain"))
            {
                _weatherController.Type = Abstractions.WeatherType.Rain;
            }
            if (GUILayout.Button("PlayDry"))
            {
                _weatherController.Type = Abstractions.WeatherType.Dry;
            }
            if (GUILayout.Button("StopWeather"))
            {
                _weatherController.Type = Abstractions.WeatherType.None;
            }
        }
        private WeatherController _weatherController => (WeatherController)target;
    }
}
