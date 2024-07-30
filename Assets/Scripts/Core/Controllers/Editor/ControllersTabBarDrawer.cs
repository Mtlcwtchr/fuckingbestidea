using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor
{
    public class ControllersTabBarDrawer
    {
        private readonly ControllersTreeViewModel _model;

        private List<ControllerTabData> _tabData;
        private int _index = 0;
        private int? _removeIndex;
        private Vector2 _scrollPosition;

        public ControllersTabBarDrawer(ControllersTreeViewModel model, ControllerTabData rootTabData)
        {
            _model = model;
            SetData(rootTabData);
        }

        private void SetData(ControllerTabData rootTabData)
        {
            _tabData = new List<ControllerTabData>();
            _tabData.Add(rootTabData);
            Select(_index);
        }

        public void AddData(ControllerTabData tabData)
        {
            _tabData.Add(tabData);
        }

        private void RemoveData(int index)
        {
            if (_index >= index)
            {
                --_index;
            }

            _removeIndex = index;
        }

        private void Select(int index)
        {
            _index = Mathf.Clamp(index, 0, _tabData?.Count - 1 ?? 0);
        }

        public void Draw()
        {
            if (_tabData == null || _tabData.Count == 0)
            {
                return;
            }

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                var index = _index;

                DrawTabBar();

                if (changeScope.changed)
                {
                    _tabData[index].TabOffCallback?.Invoke();
                    _tabData[_index].TabOnCallback?.Invoke();
                }

                _tabData[_index].TabDrawCallback?.Invoke();
            }

            if (_removeIndex.HasValue)
            {
                _tabData.RemoveAt(_removeIndex.Value);
                _removeIndex = null;
            }
        }

        public void Reload()
        {
            if (_tabData == null || _tabData.Count == 0 || _removeIndex.HasValue)
            {
                return;
            }

            _tabData[_index].TabReloadCallback();
        }

        private void DrawTabBar()
        {
            using var scrollScope = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUIStyle.none, GUIStyle.none, GUILayout.ExpandHeight(false));
            _scrollPosition = scrollScope.scrollPosition;

            using var scope = new EditorGUILayout.HorizontalScope();
            for (int i = 0; i < _tabData.Count; ++i)
            {
                DrawTabButton(i);
            }
        }

        private void DrawTabButton(int index)
        {
            using var scope = new GUILayout.HorizontalScope(GUI.skin.box);
            var color = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;

            if (GUILayout.Button(_tabData[index].TabContent, GUI.skin.box, GUILayout.Height(20), GUILayout.Width(128)))
            {
                _index = index;
            }

            GUI.backgroundColor = color;

            if (_tabData[index].IsClosable)
            {
                using var verticalScope = new GUILayout.VerticalScope(GUILayout.Height(22), GUILayout.Width(20));
                GUILayout.Space(4);
                if (GUILayout.Button("", _model.CloseButtonStyle))
                {
                    RemoveData(index);
                }
            }

            var colorRect = _index == index
                                ? ControllersTreeHelper.SelectedColor
                                : ControllersTreeHelper.UnselectedColor;
            var rect = GUILayoutUtility.GetLastRect();
            var lineRect = rect;
            lineRect.height = 2;
            var center = lineRect.center;
            center.y += rect.height;
            lineRect.center = center;

            if (rect.Contains(Event.current.mousePosition))
            {
                colorRect = ControllersTreeHelper.FocusedColor;
            }

            EditorGUI.DrawRect(lineRect, colorRect);
        }
    }
}
