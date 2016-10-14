#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Beats
{
    public class SequenceEditor : EditorWindow
    {
        [MenuItem("Beats/Sequence Editor")]
        public static void OpenSquenceEditor()
        {
            EditorWindow ew = EditorWindow.GetWindow<SequenceEditor>();
            if (ew)
            {
                ew.titleContent.text = "BEATS SEQUENCE EDITOR";
                ew.Show();
            }
        }

        private BeatSequence beatSequence;
        private GameObject serializationObj;
        private bool isRecording = false;
        private AudioSource audioSource;
        private BeatSequenceEditor bsEditor;

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                DrawSerializationObjectEditor();
                DrawRecodingGUI();
                DrawBeatSequenceGUI();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSerializationObjectEditor()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("CLOSE", GUILayout.Width(60)))
                {
                    Close();
                }

                if (GUILayout.Button("SAVE", GUILayout.Width(60)))
                {
                    SaveToObject();
                }
                if (GUILayout.Button("LOAD", GUILayout.Width(60)))
                {
                    LoadFromObject();
                }
                EditorGUILayout.LabelField("Sequence Object", GUILayout.Width(120));
                serializationObj = (GameObject)EditorGUILayout.ObjectField("", serializationObj, typeof(GameObject), true);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawBeatSequenceGUI()
        { 
            if (beatSequence == null)
            {
                GameObject bsObj = new GameObject("BeatSequence for Editor");
                bsObj.hideFlags = HideFlags.HideAndDontSave;
                beatSequence = bsObj.AddComponent<BeatSequence>();
            }

            if (beatSequence != null)
            {
                if (bsEditor == null)
                {
                    bsEditor = new BeatSequenceEditor();
                    bsEditor.BeatHandler = BeatHandler;
                }
                bsEditor.DrawGUI(beatSequence);
            }
        }

        private void BeatHandler(BeatSequence.BeatDirection dir, int presetId)
        {
            if (beatSequence != null)
            {
                beatSequence.AddBeat(audioSource.time, dir, presetId);
            }
        }

        private void DrawRecodingGUI()
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Recodring", GUILayout.Width(200));
                if (!isRecording)
                {
                    if (GUILayout.Button("START", GUILayout.Width(60)))
                    {
                        if (audioSource != null)
                        {
                            // Start Recording 
                            audioSource.Play();
                            isRecording = true;
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("STOP", GUILayout.Width(60)))
                    {
                        if (audioSource != null)
                        {
                            // Start Recording 
                            audioSource.Stop();
                            isRecording = false;
                        }
                    }
                }

                EditorGUILayout.LabelField("Audio", GUILayout.Width(50));
                audioSource = (AudioSource)EditorGUILayout.ObjectField("", audioSource, typeof(AudioSource), true);
                if (audioSource != null)
                {
                    EditorGUILayout.LabelField("Time:" + audioSource.time);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SaveToObject()
        {
            if (serializationObj == null || beatSequence == null)
            {
                return;
            }

            var bs = serializationObj.GetComponent<BeatSequence>();
            if (bs == null)
            {
                bs = serializationObj.AddComponent<BeatSequence>();
            }
            
            if (bs != null)
            {
                bs.Presets = new List<BeatSequence.Preset>(beatSequence.Presets);
                bs.Beats = new List<BeatSequence.Beat>(beatSequence.Beats);
            }
        }

        private void LoadFromObject()
        {
            if (serializationObj == null)
            {
                return;
            }
            var bs = serializationObj.GetComponent<BeatSequence>();
            if (bs == null)
            {
                return;
            }

            if (beatSequence == null)
            {
                GameObject bsObj = new GameObject("BeatSequence for Editor");
                bsObj.hideFlags = HideFlags.HideAndDontSave;
                beatSequence = bsObj.AddComponent<BeatSequence>();
            }

            if (beatSequence != null)
            {
                beatSequence.Beats = new List<BeatSequence.Beat>(bs.Beats);
                beatSequence.Presets = new List<BeatSequence.Preset>(bs.Presets);
            }
        }
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(SequenceEditor))]
//public class SequenceEditorEditor : Editor
//{
//    private int editing = -1;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        SequenceEditor se = target as SequenceEditor;
//        if (se == null)
//        {
//            return;
//        }

//        if (se.presets == null)
//        {
//            se.presets = new List<SequenceEditor.Preset>();
//        }

//        EditorGUILayout.BeginVertical();
//        {
//            EditorGUILayout.LabelField("Enemy Preset");
//            if (GUILayout.Button("Add"))
//            {
//                se.presets.Add(new SequenceEditor.Preset());
//            }

//            int remove = -1;
//            for (int i = 0; i < se.presets.Count; i++)
//            {
//                EditorGUILayout.BeginHorizontal();
//                {
//                    if (GUILayout.Button("X", GUILayout.Width(20)))
//                    {
//                        remove = i;
//                    }
//                    if (GUILayout.Button("E", GUILayout.Width(20)))
//                    {
//                        editing = (editing == i) ? -1 : i;
//                    }
//                    if (GUILayout.Button("Left"))
//                    {
//                        se.CreateEnemy(true, se.presets[i]);
//                    }
//                    if (GUILayout.Button("Right"))
//                    {
//                        se.CreateEnemy(false, se.presets[i]);
//                    }
//                }
//                EditorGUILayout.EndHorizontal();

//                if (editing == i)
//                {
//                    EditorGUILayout.BeginVertical();
//                    {
//                        se.presets[editing].Distance = EditorGUILayout.FloatField("Distance", se.presets[editing].Distance);
//                        se.presets[editing].Duration = EditorGUILayout.FloatField("Duration", se.presets[editing].Duration);
//                        se.presets[editing].Speed = EditorGUILayout.FloatField("Speed", se.presets[editing].Speed);
//                    }
//                    EditorGUILayout.EndVertical();
//                }

//            }
//            if (remove >= 0)
//            {
//                se.presets.RemoveAt(remove);
//            }
//        }
//        EditorGUILayout.EndVertical();
//    }
//}
//#endif

#endif

