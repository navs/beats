#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Beats
{
    public class BeatSequenceEditor
    {
        public System.Action<BeatSequence.BeatDirection, int> BeatHandler;
        private Vector2 beatsScrollPos;

        public void DrawGUI(BeatSequence bs)
        {
            if (bs == null)
            {
                return;
            }
            if (bs.Presets == null)
            {
                bs.Presets = new List<BeatSequence.Preset>();
            }
            if (bs.Beats == null)
            {
                bs.Beats = new List<BeatSequence.Beat>();
            }
            EditorGUILayout.BeginVertical();
            {
                DrawPresetsGUI(bs);
                DrawBeatsGUI(bs);
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPresetsGUI(BeatSequence bs)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Presets", GUILayout.Width(200));
                    if (GUILayout.Button("Add", GUILayout.Width(50)))
                    {
                        bs.Presets.Add(new BeatSequence.Preset());
                    }
                    if (GUILayout.Button("Clear", GUILayout.Width(50)))
                    {
                        bs.Presets.Clear();
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                int remove = -1;
                for (int i = 0; i < bs.Presets.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            remove = i;
                        }

                        if (BeatHandler != null)
                        {
                            if (GUILayout.Button("Left", GUILayout.Width(60)))
                            {
                                BeatHandler(BeatSequence.BeatDirection.Left, i);
                            }
                            if (GUILayout.Button("Both", GUILayout.Width(60)))
                            {
                                BeatHandler(BeatSequence.BeatDirection.Both, i);
                            }
                            if (GUILayout.Button("Right", GUILayout.Width(60)))
                            {
                                BeatHandler(BeatSequence.BeatDirection.Right, i);
                            }
                        }

                        EditorGUILayout.LabelField("Distance", GUILayout.Width(60));
                        bs.Presets[i].Distance = EditorGUILayout.FloatField("", bs.Presets[i].Distance, GUILayout.Width(60));
                        EditorGUILayout.LabelField("Duration", GUILayout.Width(60));
                        bs.Presets[i].Duration = EditorGUILayout.FloatField("", bs.Presets[i].Duration, GUILayout.Width(60));
                        EditorGUILayout.LabelField("Speed", GUILayout.Width(60));
                        bs.Presets[i].Speed = EditorGUILayout.FloatField("", bs.Presets[i].Speed, GUILayout.Width(60));
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (remove >= 0)
                {
                    bs.Presets.RemoveAt(remove);
                    bs.Beats.ForEach((BeatSequence.Beat beat) =>
                    {
                        if (beat.PresetId >= remove)
                        {
                            beat.PresetId--;
                        }
                    });
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawBeatsGUI(BeatSequence bs)
        {
            beatsScrollPos.x = 0;
            beatsScrollPos = EditorGUILayout.BeginScrollView(beatsScrollPos);
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Beats(" + bs.Beats.Count + ")", GUILayout.Width(200));
                        if (GUILayout.Button("Add", GUILayout.Width(50)))
                        {
                            bs.Beats.Add(new BeatSequence.Beat());
                        }
                        if (GUILayout.Button("Sort", GUILayout.Width(50)))
                        {
                            bs.Beats.Sort((b1, b2) => (b1.Second > b2.Second) ? 1 : (b1.Second < b2.Second) ? -1 : 0);
                        }
                        if (GUILayout.Button("Clear", GUILayout.Width(50)))
                        {
                            bs.Beats.Clear();
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    int remove = -1;
                    for (int i = 0; i < bs.Beats.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("X", GUILayout.Width(20)))
                            {
                                remove = i;
                            }
                            EditorGUILayout.LabelField("Second", GUILayout.Width(60));
                            bs.Beats[i].Second = EditorGUILayout.FloatField("", bs.Beats[i].Second, GUILayout.Width(60));
                            bs.Beats[i].Dir = (BeatSequence.BeatDirection)EditorGUILayout.EnumPopup("", bs.Beats[i].Dir, GUILayout.Width(60));
                            // @TODO INVALID preset Id ?
                            EditorGUILayout.LabelField("PresetId", GUILayout.Width(60));
                            bs.Beats[i].PresetId = EditorGUILayout.IntField("", bs.Beats[i].PresetId, GUILayout.Width(60));
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    if (remove >= 0)
                    {
                        bs.Beats.RemoveAt(remove);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}

#endif