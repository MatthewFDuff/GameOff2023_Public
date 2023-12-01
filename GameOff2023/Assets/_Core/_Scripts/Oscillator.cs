using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Core._Scripts
{
    public class Oscillator : MonoBehaviour
    {
        public double Frequency = 440;

        private double _increment;
        private double _phase;
        private double _samplingFrequency = 48000.0;

        public float gain;
        public float volume = 0.1f;
    
        public int currentFrequency;

        Dictionary<string, float> frequencies = new Dictionary<string, float>()
        {
            {"A0", 27.5f},
            {"A#0", 29.135f},
            {"B0", 30.868f},
            {"C1", 32.703f},
            {"C#1", 34.648f},
            {"D1", 36.708f},
            {"D#1", 38.891f},
            {"E1", 41.203f},
            {"F1", 43.654f},
            {"F#1", 46.249f},
            {"G1", 48.999f},
            {"G#1", 51.913f},
            {"A1", 55.0f},
            {"A#1", 58.27f},
            {"B1", 61.735f},
            {"C2", 65.406f},
            {"C#2", 69.296f},
            {"D2", 73.416f},
            {"D#2", 77.782f},
            {"E2", 82.407f},
            {"F2", 87.307f},
            {"F#2", 92.499f},
            {"G2", 97.999f},
            {"G#2", 103.826f},
            {"A2", 110.0f},
            {"A#2", 116.541f},
            {"B2", 123.471f},
            {"C3", 130.813f},
            {"C#3", 138.591f},
            {"D3", 146.832f},
            {"D#3", 155.563f},
            {"E3", 164.814f},
            {"F3", 174.614f},
            {"F#3", 184.997f},
            {"G3", 195.998f},
            {"G#3", 207.652f},
            {"A3", 220.0f},
            {"A#3", 233.082f},
            {"B3", 246.942f},
            {"C4", 261.626f},
            {"C#4", 277.183f},
            {"D4", 293.665f},
            {"D#4", 311.127f},
            {"E4", 329.628f},
            {"F4", 349.228f},
            {"F#4", 369.994f},
            {"G4", 391.995f},
            {"G#4", 415.305f},
            {"A4", 440.0f},
            {"A#4", 466.164f},
            {"B4", 493.883f},
            {"C5", 523.251f},
            {"C#5", 554.365f},
            {"D5", 587.33f},
            {"D#5", 622.254f},
            {"E5", 659.255f},
            {"F5", 698.456f},
            {"F#5", 739.989f},
            {"G5", 783.991f},
            {"G#5", 830.609f},
            {"A5", 880.0f},
            {"A#5", 932.328f},
            {"B5", 987.767f},
            {"C6", 1046.502f},
            {"C#6", 1108.731f},
            {"D6", 1174.659f},
            {"D#6", 1244.508f},
            {"E6", 1318.51f},
            {"F6", 1396.913f},
            {"F#6", 1479.978f},
            {"G6", 1567.982f},
            {"G#6", 1661.219f},
            {"A6", 1760.0f},
            {"A#6", 1864.655f},
            {"B6", 1975.533f},
            {"C7", 2093.005f},
            {"C#7", 2217.461f},
            {"D7", 2349.318f},
            {"D#7", 2489.016f},
            {"E7", 2637.021f},
            {"F7", 2793.826f},
            {"F#7", 2959.955f},
            {"G7", 3135.963f},
            {"G#7", 3322.438f},
            {"A7", 3520.0f},
            {"A#7", 3729.31f},
            {"B7", 3951.066f},
            {"C8", 4186.01f}
        };

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(PlaySounds());
            }
        }

        private IEnumerator PlaySounds()
        {
            gain = volume;
                
            foreach (var freak in frequencies)
            {
                Frequency = freak.Value;
                yield return new WaitForSeconds(0.2f);
            }
                
            gain = 0;
            yield return null;
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            _increment = Frequency * 2.0 * Math.PI / _samplingFrequency;

            for (int i = 0; i < data.Length; i += channels)
            {
                _phase += _increment;
                data[i] = (float) (gain * Math.Sin(_phase));
            
                if (channels == 2) data[i + 1] = data[i];
            
                if (_phase > 2 * Math.PI) _phase = 0.0;
            }
        }
    }
}
