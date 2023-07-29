using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace SkillSystem
{

    public class SkillTreeEditor : EditorWindow
    {
        SkillTreeManager currentSkillTreeManager;
        GameObject currentGameObject;
        Library library => currentSkillTreeManager.skillLibrary;

        Skill justAdded;
        EditorSkillNode selectedVertex;

        int numLevels = 3;

        public static void OpenWindow()
        {
            GetWindow<SkillTreeEditor>("Skill Tree Editor");
        }

        void OnGUI()
        {
            if (Selection.activeGameObject is null)
            {
                currentGameObject = null;
                currentSkillTreeManager = null;
                return;
            }

            if (currentSkillTreeManager is null)
            {
                if (!Selection.activeGameObject.TryGetComponent<SkillTreeManager>(out currentSkillTreeManager))
                {
                    return;
                }
            }

            if (library is null)
            {
                return;
            }

            if (justAdded is not null)
            {
                CreateNode(justAdded);
                justAdded = null;
            }

            DrawButtons();
            DrawNodes();
            DrawLines();

        }

        /// <summary>
        /// Draws Nodes to represent the graphs Verticies
        /// </summary>
        private void DrawNodes()
        {
            using (var areaScope = new GUILayout.AreaScope(new Rect(10, 300, 500, 500)))
            {
                for (int level = 0; level < numLevels; level++)
                {
                    using (var horizontalScope = new GUILayout.HorizontalScope(GUILayout.Height(20)))
                    {
                        GUILayout.Box("Level " + level, GUILayout.ExpandWidth(true), GUILayout.Height(20));
                        var rect = GUILayoutUtility.GetLastRect();
                        if (Event.current.type == EventType.MouseUp && selectedVertex is not null && rect.Contains(Event.current.mousePosition))
                        {
                            if (Event.current.button == 0)
                                selectedVertex.levelVertex.levelInTree = level;
                            selectedVertex = null;
                        }
                        justAdded = (Skill)EditorGUILayout.ObjectField(justAdded, typeof(Skill), true);
                    }
                    using (var horizontalScope = new GUILayout.HorizontalScope(GUILayout.Height(100)))
                    {
                        if (currentSkillTreeManager.graph is not null)
                        {
                            foreach (var vertex in currentSkillTreeManager.graph.GetVerticies())
                            {
                                if (vertex.levelInTree == level && vertex is not null)
                                {
                                    DrawNodeFromVertex(vertex);
                                }
                            }

                        }
                    }
                }
                using (var horizontalScope = new GUILayout.HorizontalScope(GUILayout.Height(20)))
                {
                    GUILayout.Box("remove node ", GUILayout.ExpandWidth(true), GUILayout.Height(20));
                    var rect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.MouseUp && selectedVertex is not null && rect.Contains(Event.current.mousePosition))
                    {
                        currentSkillTreeManager.graph.RemoveVertex(selectedVertex.levelVertex);
                        selectedVertex = null;
                    }
                }
            }
        }

        /// <summary>
        /// Draws navigation and UI buttons
        /// </summary>
        private void DrawButtons()
        {
            using (var areaScope = new GUILayout.AreaScope(new Rect(10, 100, 500, 200)))
            {
                if (GUILayout.Button("Create Node"))
                {
                    CreateNode(null);
                }

                if (GUILayout.Button("Clear all nodes"))
                {
                    ClearTree();
                }
                if (GUILayout.Button("Add Level"))
                {
                    numLevels++;
                }
                if (GUILayout.Button("Remove Level"))
                {
                    if (numLevels != 1)
                    {
                        numLevels--;
                    }
                }
                if (GUILayout.Button("Remove Connections"))
                {
                    ClearConnections();
                }
            }
        }

        /// <summary>
        /// Creates a GUI element to display a skill in the tree
        /// </summary>
        /// <param name="levelVertex">The tree node to get the information from</param>
        void DrawNodeFromVertex(LevelVertex<Skill> levelVertex)
        {
            // using ( var verticalScope = new GUILayout.VerticalScope(GUILayout.Width(50)))
            using ( var verticalScope = new GUILayout.VerticalScope(GUILayout.Width(50)))
            {
                GUILayout.Box(levelVertex.content?.skillName, GUILayout.Width(50));
                GUILayout.Box(levelVertex.content?.icon.texture, GUILayout.Width(50), GUILayout.Height(50));
                var last = GUILayoutUtility.GetLastRect();
                levelVertex.content = (Skill)EditorGUILayout.ObjectField(levelVertex.content, typeof(Skill), true);
                levelVertex.positionInEditor = last.center;

                if (Event.current.type == EventType.MouseDown && last.Contains(Event.current.mousePosition))
                {
                    selectedVertex = new EditorSkillNode{levelVertex = levelVertex, position = last.center};
                }

                if (Event.current.type == EventType.MouseUp && last.Contains(Event.current.mousePosition))
                {
                    if(Event.current.button == 1)
                    {
                        // Debug.Log("From: " + selectedVertex.content.skillName + "   To: " + levelVertex.content.skillName);
                        if ( levelVertex == selectedVertex.levelVertex )
                        {
                            return;
                        }
                        // Debug.Log("Adding an edge");
                        // currentSkillTreeManager.graph.AddEdge(ref selectedVertex.levelVertex, ref levelVertex);
                        selectedVertex.levelVertex.connections.Add(levelVertex);
                    }
                }
            }
        }

        /// <summary>
        /// Draws lines to represent the graph's edges
        /// </summary>
        void DrawLines()
        {
            foreach (var vertex in currentSkillTreeManager.graph.GetVerticies())
            {
                foreach (var connection in vertex.connections)
                {
                    Handles.DrawLine(vertex.positionInEditor, connection.positionInEditor);
                }
            }

        }

        void DrawSkillFromLibrary(Skill skill)
        {
            GUILayout.Label(skill.icon.texture, GUILayout.Width(50), GUILayout.Height(50));
            var rect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition)) {
                Debug.Log("mose over " + skill.skillName);
            }
        }

        LevelVertex<Skill> CreateNode(Skill skill)
        {
            if (currentSkillTreeManager.graph is not null)
            {
                LevelVertex<Skill> node = new LevelVertex<Skill>();
                node.levelInTree = 0;
                node.content = skill;
                currentSkillTreeManager.graph.AddVertex(node);
                return node;
            }

            return null;
        }

        /// <summary>
        /// Clears all the edges from every vertex in the tree
        /// </summary>
        void ClearConnections()
        {
            foreach(var vertex in currentSkillTreeManager.graph.GetVerticies())
            {
                vertex.connections.Clear();
            }
        }

        /// <summary>
        /// Empties the tree
        /// </summary>
        void ClearTree()
        {
            currentSkillTreeManager.graph.ClearVerticies();
        }
    }

    [CustomEditor(typeof(SkillTreeManager))]
    public class SkillTreeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Edit Tree"))
            {
                //Debug.Log("opening the skill tree editor window");
                SkillTreeEditor.OpenWindow();
            }
        }
    }

    class EditorSkillNode
    {
        public Vector2 position;
        public LevelVertex<Skill> levelVertex;
    }
}