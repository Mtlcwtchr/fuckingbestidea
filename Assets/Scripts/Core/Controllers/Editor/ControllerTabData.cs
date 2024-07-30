using System;
using UnityEngine;

namespace Core.Controllers.Editor
{
    public readonly struct ControllerTabData
    {
        public readonly string TabName;
        public readonly GUIContent TabContent;

        public readonly bool IsClosable;
        public readonly Action TabDrawCallback;
        public readonly Action TabReloadCallback;
        public readonly Action TabOnCallback;
        public readonly Action TabOffCallback;

        public ControllerTabData(
            string tabName,
            string tabTooltip,
            bool isClosable,
            Action tabDrawCallback,
            Action tabReloadCallback,
            Action tabOnCallback = null,
            Action tabOffCallback = null)
        {
            TabName = tabName;
            TabContent = new GUIContent(TabName, tabTooltip);

            IsClosable = isClosable;
            TabDrawCallback = tabDrawCallback;
            TabReloadCallback = tabReloadCallback;
            TabOnCallback = tabOnCallback;
            TabOffCallback = tabOffCallback;
        }
    }
}
