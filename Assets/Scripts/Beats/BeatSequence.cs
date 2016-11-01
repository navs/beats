using UnityEngine;
using System.Collections.Generic;

namespace Beats
{
    public class BeatSequence : MonoBehaviour
    {
        [System.Flags]
        public enum BeatDirection
        {
            Left = 1,
            Right = 2,
            Both = 3
        }

        [System.Serializable]
        public class Preset
        {
            public float Distance = 10;
            public float Duration = 2;
            public float Speed = 4.5f;
            public float Angle = 0;
        }

        [System.Serializable]
        public class Beat
        {
            public float Second;
            public BeatDirection Dir;
            public int PresetId;
        }

        [SerializeField]
        public List<Preset> Presets;

        [SerializeField]
        public List<Beat> Beats;

        public void AddBeat(float second, BeatDirection dir, int presetId)
        {
            if (Presets == null || presetId < 0 || presetId >= Presets.Count)
            {
                return;
            }
            if (Beats == null)
            {
                Beats = new List<Beat>();
            }
            Beats.Add(new Beat()
            {
                Second = second,
                Dir = dir,
                PresetId = presetId
            });
        }
    }
}
