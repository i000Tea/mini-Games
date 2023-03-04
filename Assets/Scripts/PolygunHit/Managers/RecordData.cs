using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tea
{
    public class RecordData : MonoBehaviour
    {
        public static RecordData inst;
        private void Awake()
        {
            if (!inst)
            {
                inst = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(gameObject);
        }
    }
}
