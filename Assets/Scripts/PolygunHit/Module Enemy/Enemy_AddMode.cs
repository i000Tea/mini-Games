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
   }
}
