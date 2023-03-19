using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea
{
	/// <summary>
	/// 泛型单例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>   //where T : new()为泛型约束，通俗来说就是确保T类型是可以被new的
	{
		/// <summary>
		/// 私有的T类型的静态变量
		/// </summary>
		private static T _instance;
		/// <summary>
		/// 获取实例的函数
		/// </summary>
		/// <returns></returns>
		public static T I
		{
			//返回实例
			get { return _instance; }
		}
		protected virtual void Awake()
		{
			//判断实例是否已存在
			if (_instance == null)
			{
				//不存在则创建新的实例
				_instance = (T)this;
			}
		}
	}
}
