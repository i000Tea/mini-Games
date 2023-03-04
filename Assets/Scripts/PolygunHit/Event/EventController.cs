using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit
{
    /// <summary>
    /// 事件控制器
    /// </summary>
    public class EventController
    {
        /// <summary>
        /// EventCenter 最顶上加的一个 目前没看懂是啥用途
        /// </summary>
        private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

        #region 脚本精简，将重复性脚本提取出来
        /// <summary>
        /// 添加监听
        /// </summary>
        private static void OnListenerAdding(EventType eventType, Delegate callBack)
        {
            //判断事件码是否存在，如果不存在就添加
            if (!m_EventTable.ContainsKey(eventType))
            {
                m_EventTable.Add(eventType, null);
            }
            //拿到m_EventTable所对应的委托
            Delegate d = m_EventTable[eventType];
            //判断要添加的委托与当前事件码对应的委托类型是否一致，一致才可以绑定
            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("尝试为事件码{0}添加不同类型的委托，" +
                    "当前事件所对应的委托是{1}，要添加的委托类型是{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }

        /// <summary>
        /// 移除监听前
        /// </summary>
        private static void OnListenerRemoving(EventType eventType, Delegate callBack)
        {
            //判断事件码是否存在
            if (m_EventTable.ContainsKey(eventType))
            {
                Delegate d = m_EventTable[eventType];
                //当前事件码对应的委托是否为空，空无法移除
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
                }
                else
                //判断要移除的委托与当前事件码对应的委托类型是否一致，一致才可以移除
                if (d.GetType() != callBack.GetType())
                {
                    throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，" +
                        "当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("移除监听错误：没有事件码{0}", eventType));
            }
        }

        /// <summary>
        /// 移除监听后
        /// </summary>
        private static void OnListenerRemoved(EventType eventType)
        {
            //判断当前事件码对应的委托是否为空，为空的话事件码无用，移除事件码
            if (m_EventTable[eventType] == null)
            {
                m_EventTable.Remove(eventType);
            }
        }
        #endregion

        #region 添加监听
        /// <summary>
        /// 无参的添加监听
        /// </summary>
        public static void AddListener(EventType eventType, CallBack callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 单个参数的监听
        /// </summary>
        public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 两个参数的监听
        /// </summary>
        public static void AddListener<T, X>(EventType eventType, CallBack<T, X> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 三个参数的监听
        /// </summary>
        public static void AddListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 四个参数的监听
        /// </summary>
        public static void AddListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 五个参数的监听
        /// </summary>
        public static void AddListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] + callBack;
        }

        #endregion

        #region 移除监听

        /// <summary>
        /// 无参的移除监听
        /// </summary>
        public static void RemoveListener(EventType eventType, CallBack callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 一个参数的移除监听
        /// </summary>
        public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 两个参数的移除监听
        /// </summary>
        public static void RemoveListener<T, X>(EventType eventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 三个参数的移除监听
        /// </summary>
        public static void RemoveListener<T, X, Y>(EventType eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 四个参数的移除监听
        /// </summary>
        public static void RemoveListener<T, X, Y, Z>(EventType eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 无个参数的移除监听
        /// </summary>
        public static void RemoveListener<T, X, Y, Z, W>(EventType eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        #endregion

        #region 广播
        /// <summary>
        /// 无参广播
        /// </summary>
        public static void Broadcast(EventType eventType)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        /// <summary>
        /// 一个参数的广播
        /// </summary>
        public static void Broadcast<T>(EventType eventType, T arg)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        /// <summary>
        /// 两个参数的广播
        /// </summary>
        public static void Broadcast<T, X>(EventType eventType, T arg1, X arg2)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X> callBack = d as CallBack<T, X>;
                if (callBack != null)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        /// <summary>
        /// 三个参数的广播
        /// </summary>
        public static void Broadcast<T, X, Y>(EventType eventType, T arg1, X arg2, Y arg3)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        /// <summary>
        /// 四个参数的广播
        /// </summary>
        public static void Broadcast<T, X, Y, Z>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }

        /// <summary>
        /// 五个参数的广播
        /// </summary>
        public static void Broadcast<T, X, Y, Z, W>(EventType eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
                }
            }
        }
        #endregion
    }
}
