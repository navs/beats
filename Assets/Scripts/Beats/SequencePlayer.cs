using UnityEngine;
using System.Collections.Generic;
using Actors;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Beats
{
    public class SequencePlayer : MonoBehaviour
    {
        public AudioSource Muzik;
        public BeatSequence BeatSeq;
        public EnemyActor EnemyActorPrefab;
        public bool IsPlaying = false;

        private float elapsedTime;
        private List<BeatSequence.Beat> beats;

        void Update()
        {
            if (IsPlaying)
            {
                elapsedTime += Time.deltaTime;
                PlayBeats();
            }
        }

        public void Play()
        {
            if (Muzik == null || BeatSeq == null)
            {
                return;
            }
            beats = new List<BeatSequence.Beat>(BeatSeq.Beats);

            if (!VerifyBeats())
            {
                Debug.LogError("Beats are not valid.");
                return;
            }
            elapsedTime = 0;
            IsPlaying = true;
            Muzik.Play();
        }

        public void Stop()
        {
            if (Muzik != null)
            {
                Muzik.Stop();
            }
            IsPlaying = false;
        }

        private bool VerifyBeats()
        {
            int presetCounts = (BeatSeq != null && BeatSeq.Presets != null) ? BeatSeq.Presets.Count : 0;
            return !beats.Exists(b => b.PresetId < 0 || b.PresetId >= presetCounts);
        }

        private void PlayBeats()
        {
            foreach (var beat in beats.FindAll(b => b.Second - BeatSeq.Presets[b.PresetId].Duration <= elapsedTime))
            {
                PlayBeat(beat);
            }
            beats.RemoveAll(b => b.Second - BeatSeq.Presets[b.PresetId].Duration <= elapsedTime);
        }
        
        private void PlayBeat(BeatSequence.Beat beat)
        {
            var preset = BeatSeq.Presets[beat.PresetId];
            Vector3 pos = new Vector3(preset.Distance, 0, 0);
            Vector3 lookAtTarget = Vector3.zero;
            
            CreateEnemyActor(preset.Distance, preset.Angle, lookAtTarget, preset.Duration, preset.Speed);
        }

        private void CreateEnemyActor(float distance, float angle, Vector3 lookAtTarget, float lifetime, float speed)
        {
            EnemyActor actor = Instantiate<EnemyActor>(EnemyActorPrefab);
            if (actor != null)
            {
                actor.transform.localPosition = new Vector3(
                    distance * Mathf.Cos(angle * Mathf.PI / 180.0f),
                    0,
                    distance * Mathf.Sin(angle * Mathf.PI / 180.0f));
                //actor.transform.localEulerAngles = angle;
                //actor.transform.rotation = Quaternion.EulerAngles(0, -angle, 0);
                actor.transform.LookAt(lookAtTarget);
                actor.Lifetime = lifetime;
                actor.Speed = speed;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SequencePlayer))]
    public class SequencePlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var sp = target as SequencePlayer;
            if (sp == null)
            {
                base.OnInspectorGUI();
                return;
            }

            sp.Muzik = (AudioSource)EditorGUILayout.ObjectField("Muzik", sp.Muzik, typeof(AudioSource), true);
            sp.BeatSeq = (BeatSequence)EditorGUILayout.ObjectField("Beat Sequence", sp.BeatSeq, typeof(BeatSequence), true);
            sp.EnemyActorPrefab = (EnemyActor)EditorGUILayout.ObjectField("Enemy Actor", sp.EnemyActorPrefab, typeof(EnemyActor), true);

            if (Application.isPlaying)
            {
                if (!sp.IsPlaying && GUILayout.Button("PLAY") && sp.Muzik != null && sp.BeatSeq != null && sp.EnemyActorPrefab)
                {
                    sp.Play();
                }
                if (sp.IsPlaying && GUILayout.Button("STOP"))
                {
                    sp.Stop();
                }
            }
        }
    }
#endif
}