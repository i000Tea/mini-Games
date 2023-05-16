using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea.PolygonHit.Enemy
{
   public class Enemy_AddMode : MonoBehaviour
   {
      protected EnemyBase Base
      {
         get
         {
            if (@base == null)
            {
               @base = GetComponent<EnemyBase>();
            }
            return @base;
         }
      }
      private EnemyBase @base;

      /// <summary>
      /// 自身刚体
      /// </summary>
      protected Rigidbody2D m_Rig => GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
      protected Collider2D m_Collider => GetComponent<Collider2D>() ?? gameObject.AddComponent<CircleCollider2D>();

      private void Awake()
      {
         Base.ModeInitialize += Initialize;
         Base.ModeDestroy += BeDestroy;
      }

      protected virtual void Initialize()
      {

      }
      protected virtual void BeDestroy()
      {

      }

      #region OnCollision2D
      private void OnCollisionEnter2D(Collision2D collision)
      {
         if (collision.transform.TryGetComponent(out PlayerBase player))
         {
            if (player == Base.TargetPlayer)
            {
               OnEnterHitTarget(collision);
            }
         }
      }
      private void OnCollisionStay2D(Collision2D collision)
      {
         if (collision.transform.TryGetComponent(out PlayerBase player))
         {
            if (player == Base.TargetPlayer)
            {
               OnStayHitTarget(collision);
            }
         }
      }
      private void OnCollisionExit2D(Collision2D collision)
      {
         if (collision.transform.TryGetComponent(out PlayerBase player))
         {
            if (player == Base.TargetPlayer)
            {
               OnExitHitTarget(collision);
            }
         }
      }

      #endregion

      #region OnHitTirget
      /// <summary>
      /// 撞击到敌人
      /// </summary>
      /// <param name="target"></param>
      protected virtual void OnEnterHitTarget(Collision2D collision) { }
      /// <summary>
      /// 正在接触中
      /// </summary>
      /// <param name="target"></param>
      protected virtual void OnStayHitTarget(Collision2D collision) { }
      /// <summary>
      /// 结束撞击
      /// </summary>
      /// <param name="target"></param>
      protected virtual void OnExitHitTarget(Collision2D collision) { }
      #endregion
   }
}
