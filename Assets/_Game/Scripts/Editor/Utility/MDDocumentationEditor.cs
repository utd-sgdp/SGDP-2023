using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Utility.Editor;
using System.Text.RegularExpressions;

namespace GameEditor.Utility
{
    /// <summary>
    /// This class is heavily inspired (and parts taken) from Brandon Coffey's Documentation component
    /// https://github.com/metalac190/GhostHouse/blob/develop/Assets/_Game/Scripts/Utility/Documentation.cs
    /// </summary>
    [CustomEditor(typeof(MDDocumentation))]
    public class MDDocumentationEditor : Editor
    {
        SerializedProperty _descriptionProperty;
        bool _preview = true;

        public void OnEnable()
        {
            _descriptionProperty = serializedObject.FindProperty(nameof(MDDocumentation.description));
        }

        public override void OnInspectorGUI()
        {
            if (!_preview)
            {
                // show description property as a normal string
                serializedObject.Update();
                EditorGUILayout.PropertyField(_descriptionProperty);
                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Render"))
                {
                    // switch to preview mode
                    _preview = true;
                }
            }
            else
            {
                // render the description markdown
                GUILayout.Space(6);

                List<MarkdownLine> sections = ParseMarkdown(_descriptionProperty.stringValue);
                foreach (var section in sections)
                {
                    GUIStyle style = new GUIStyle
                    {
                        wordWrap = true,
                    };
                    style.normal.textColor = Color.white;


                    int space = 6;
                    switch (section.Bold)
                    {
                        case 1:
                            space = 12;
                            style.fontSize = 24;
                            style.fontStyle = FontStyle.Bold;
                            break;
                        case 2:
                            space = 15;
                            style.fontSize = 16;
                            style.fontStyle = FontStyle.Bold;
                            break;
                        case 3:
                            space = 18;
                            style.fontSize = 14;
                            style.fontStyle = FontStyle.Italic;
                            break;
                    }

                    if (!string.IsNullOrEmpty(section.Text))
                    {
                        GUILayout.Label(section.Text, style);
                        GUILayout.Space(space);
                    }
                }

                GUILayout.Space(4);

                if (GUILayout.Button("Edit"))
                {
                    _preview = false;
                }
            }
        }

        static List<MarkdownLine> ParseMarkdown(string text)
        {
            var lines = Regex.Split(text, "\n|\r|\r\n");

            var markdownLines = new List<MarkdownLine>();
            foreach (var l in lines)
            {
                var line = l.Trim();
                int bold = 0;

                if (line.StartsWith("###"))
                {
                    line = line.Remove(0, 3);
                    bold = 3;
                }

                if (line.StartsWith("##"))
                {
                    line = line.Remove(0, 2);
                    bold = 2;
                }

                if (line.StartsWith("#"))
                {
                    line = line.Remove(0, 1);
                    bold = 1;
                }

                var markdownLine = new MarkdownLine
                {
                    Text = line.Trim(),
                    Bold = bold
                };

                markdownLines.Add(markdownLine);
            }

            return markdownLines;
        }

        struct MarkdownLine
        {
            public string Text;
            public int Bold;
        }
    }
}