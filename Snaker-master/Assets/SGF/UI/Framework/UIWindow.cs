﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGF;
using UnityEngine;
using UnityEngine.UI;

namespace SGF.UI.Framework
{
    public class UIWindow:UIPanel
    {
        //=======================================================================

        public delegate void CloseEvent(object arg = null);

        //=======================================================================
        /// <summary>
        /// 关闭按钮，大部分窗口都会有关闭按钮
        /// </summary>
        [SerializeField]
        private Button m_btnClose;

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        public event CloseEvent onClose;

        /// <summary>
        /// 打开UI的参数
        /// </summary>
        protected object m_openArg;

        /// <summary>
        /// 该UI的当前实例是否曾经被打开过
        /// </summary>
        private bool m_isOpenedOnce;

        /// <summary>
        /// 当UI可用时调用
        /// </summary>
        protected void OnEnable()
        {
            this.Log("OnEnable()");
            if (m_btnClose != null)
            {
                m_btnClose.onClick.AddListener(OnBtnClose);
            }
        }

        /// <summary>
        /// 当UI不可用时调用
        /// </summary>
        protected void OnDisable()
        {
            this.Log("OnDisable()");

            if (m_btnClose != null)
            {
                m_btnClose.onClick.RemoveAllListeners();
            }
        }

        /// <summary>
        /// 当点击关闭按钮时调用
        /// 但是并不是每一个Window都有关闭按钮
        /// </summary>
        private void OnBtnClose()
        {
            this.Log("OnBtnClose()");
            Close(0);
        }


        /// <summary>
        /// 调用它打开UIWindow
        /// </summary>
        /// <param name="arg"></param>
        public sealed override void Open(object arg = null)
        {
            this.Log("Open() arg:{0}", arg);
            m_openArg = arg;
            m_isOpenedOnce = false;
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
            }

            OnOpen(arg);
            m_isOpenedOnce = true;
        }

        /// <summary>
        /// 调用它以关闭UIWindow
        /// </summary>
        public sealed override void Close(object arg = null)
        {
            this.Log("Close()");
            if (this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(false);
            }

            OnClose(arg);
            if (onClose != null)
            {
                onClose(arg);
                onClose = null;
            }
        }



    }
}
