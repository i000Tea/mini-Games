using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tea.PolygonHit.Enemy
{
   public class Enemy_Attack : Enemy_AddMode
   {
      [SerializeField]
      private int Atk;

      /// <summary>
      /// 当前延迟
      /// </summary>
      private float nowDelay;
      /// <summary>
      /// 需求延迟
      /// </summary>
      [SerializeField]
      private float neetDelay = 0.2f;
      private void OnCollisionStay2D(Collision2D collision)
      {
         // 当碰撞对象是目标且存在玩家脚本时累加
         if (collision.transform == Base.m_Target && collision.transform.TryGetComponent(out PlayerBase player))
         {
            nowDelay += Time.deltaTime;
            // 当时间超过阈值 尝试攻击 成功 则时间归零
            if (nowDelay >= neetDelay)
            {
               Debug.Log(collision.gameObject.name);
               player.UnAtk(Atk);
               nowDelay = 0;
            }
         }
      }
      private void OnCollisionExit2D(Collision2D collision)
      {
         nowDelay = 0;
      }
   }
}