using System;
using System.Collections;
using System.Collections.Generic;
using Tea.PolygonHit;
using UnityEngine;

namespace Tea
{
	/// <summary>
	/// 监听事件
	/// </summary>
	public delegate void DelegateEvent();
	public delegate void GameStateEvent(GameState gameState);
	/// <summary>
	/// 事件控制
	/// </summary>
	public class EventCenter : MonoBehaviour
	{
		/// <summary>
		/// 任意枚举类的事件
		/// </summary>
		private static Dictionary<Enum, Delegate> m_EveryTable = new Dictionary<Enum, Delegate>();

		#region 基类
		/// <summary>
		/// 添加监听
		/// </summary>
		private static void OnAddSomeList<_Enum>(_Enum eventType, Delegate callBack) where _Enum : Enum
		{
			//判断事件码是否存在，如果不存在就添加
			if (!m_EveryTable.ContainsKey(eventType))
			{
				m_EveryTable.Add(eventType, null);
			}
			//拿到m_EventTable所对应的委托
			Delegate d = m_EveryTable[eventType];
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
		private static void OnRemovingSomeList<_Enum>(_Enum eventType, Delegate callBack) where _Enum : Enum
		{
			//判断事件码是否存在
			if (m_EveryTable.ContainsKey(eventType))
			{
				Delegate d = m_EveryTable[eventType];
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
		private static void OnRemovedSomeList<_Enum>(_Enum eventType) where _Enum : Enum
		{

			//判断当前事件码对应的委托是否为空，为空的话事件码无用，移除事件码
			if (m_EveryTable[eventType] == null)
			{
				m_EveryTable.Remove(eventType);
			}
		}
		#endregion

		#region Button
		/// <summary>
		/// 按钮类的监听
		/// </summary>
		public static void OnAddButtonList(ButtonType eventType, DelegateEvent callBack)
		{
			OnAddSomeList(eventType, callBack);
			m_EveryTable[eventType] = (DelegateEvent)m_EveryTable[eventType] + callBack;
		}
		/// <summary>
		/// 按钮类的移除监听
		/// </summary>
		public static void OnRemoveButtonList(ButtonType eventType, DelegateEvent callBack)
		{
			OnRemovingSomeList(eventType, callBack);
			m_EveryTable[eventType] = (DelegateEvent)m_EveryTable[eventType] - callBack;
			OnRemovedSomeList(eventType);
		}
		/// <summary>
		/// 按钮广播
		/// </summary>
		public static void InvokeButton(ButtonType eventType)
		{
			Delegate baseDele;
			if (m_EveryTable.TryGetValue(eventType, out baseDele))
			{
				DelegateEvent dEvent = baseDele as DelegateEvent;
				if (dEvent != null)
				{
					dEvent();
				}
				else
				{
					throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", eventType));
				}
			}
		}
		#endregion

		#region GameState
		/// <summary>
		/// 游戏状态切换 监听
		/// </summary>
		public static void OnAddGameStateList(GameStateEvent callBack)
		{
			OnAddSomeList(EventType.GameState, callBack);
			m_EveryTable[EventType.GameState] = (GameStateEvent)m_EveryTable[EventType.GameState] + callBack;
		}
		/// <summary>
		/// 游戏状态切换 移除
		/// </summary>
		public static void OnRemoveGameStateList(GameStateEvent callBack)
		{
			OnRemovingSomeList(EventType.GameState, callBack);
			m_EveryTable[EventType.GameState] = (GameStateEvent)m_EveryTable[EventType.GameState] - callBack;
			OnRemovedSomeList(EventType.GameState);
		}
		/// <summary>
		/// 游戏状态切换 使用
		/// </summary>
		/// <param name="stateType"></param>
		/// <exception cref="Exception"></exception>
		public static void SetGameState(GameState stateType)
		{
			Delegate baseDele;
			if (m_EveryTable.TryGetValue(stateType, out baseDele))
			{
				GameStateEvent dEvent = baseDele as GameStateEvent;
				if (dEvent != null)
				{
					dEvent(stateType);
				}
				else
				{
					throw new Exception(string.Format("广播事件错误错误：事件{0}对应的委托具有不同的类型", stateType));
				}
			}
		}
		#endregion
	}
}
