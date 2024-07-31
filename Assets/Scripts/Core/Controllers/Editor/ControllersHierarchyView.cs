using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor
{
    internal class ControllersHierarchyView
    {
        private readonly ControllerTreeView _controllerTreeView;
        private readonly ControllersTreeViewModel _model;

        private string _searchString;
        private Vector3 _scrollPos;

        internal ControllersHierarchyView(ControllerTreeView controllerTreeView, ControllersTreeViewModel model)
        {
            _model = model;
            _controllerTreeView = controllerTreeView;
        }

        internal void Draw()
        {
            using var verticalScope = new GUILayout.VerticalScope();
            using (new GUILayout.HorizontalScope())
            {
                using (var vertScope = new EditorGUILayout.VerticalScope(
                           GUI.skin.box, GUILayout.Width(_model.HierarchyWidth), GUILayout.Height(_model.TopPanelHeight)))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Space(4f);
                        var searchString = GUILayout.TextField(_searchString, _model.SearchStyle, GUILayout.ExpandWidth(true));
                        if (searchString != _searchString)
                        {
                            _searchString = searchString;
                            _controllerTreeView.searchString = _searchString;
                            _controllerTreeView.Reload();
                        }

                        if (GUILayout.Button("", _model.CancelSearchStyle))
                        {
                            _searchString = null;
                            _controllerTreeView.searchString = _searchString;
                            _controllerTreeView.Reload();
                        }

                        GUILayout.Space(2f);
                    }

                    GUILayout.Space(-36);

                    using (var scrollScope = new GUILayout.ScrollViewScope(_scrollPos))
                    {
                        _scrollPos = scrollScope.scrollPosition;
                        var rect = vertScope.rect;
                        rect.width -= 8f;
                        rect.height -= 26f;
                        _controllerTreeView.OnGUI(rect);
                    }
                }

                if (_model.IsRightPanelVisible)
                {
                    var style = new GUIStyle(GUI.skin.textArea);
                    style.padding.left = 2;
                    style.padding.right = 2;
                    using (var vertScope = new EditorGUILayout.VerticalScope(
                               style, GUILayout.Width(ControllersTreeViewModel.SEPARATOR_SIZE), GUILayout.ExpandHeight(true)))
                    {
                        _model.SetVerticalSeparatorRect(vertScope.rect);
                        EditorGUI.DrawRect(vertScope.rect, ControllersTreeHelper.DividingLineColor);
                    }
                }

                _controllerTreeView.DrawRightPanel();
            }

            DrawBottom();
        }

        private void DrawBottom()
        {
            using var verticalScope = new GUILayout.VerticalScope(GUILayout.Height(_model.BottomPanelHeight));
            if (_model.IsBottomPanelVisible)
            {
                var style = new GUIStyle(GUI.skin.textArea);
                style.padding.top = 2;
                style.padding.bottom = 2;
                using var horizontalScope = new EditorGUILayout.HorizontalScope(
                    style, GUILayout.Height(ControllersTreeViewModel.SEPARATOR_SIZE), GUILayout.ExpandWidth(true));
                _model.SetHorizontalSeparatorRect(horizontalScope.rect);
                EditorGUI.DrawRect(horizontalScope.rect, ControllersTreeHelper.DividingLineColor);
            }

            _controllerTreeView.DrawBottomPanel();
        }

        internal void Reload()
        {
            _controllerTreeView.Reload();
        }
    }
}
