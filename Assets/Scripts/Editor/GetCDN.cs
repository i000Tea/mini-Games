using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WeChatWASM;
using Tea.PolygonHit;
using Tea;

public class GetCDN : MonoBehaviour
{
   const string editorWXConfigPath = "Assets/WX-WASM-SDK/Editor";// MiniGameConfig.asset
   [MenuItem("TeaAdd/Skill/获取CDN路径")]
   private static void GetCDNFlie()
   {
      string[] assetGuids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { editorWXConfigPath });
      if (assetGuids.Length > 0)
      {
         string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
         ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
         if (scriptableObject != null)
         {
            // 处理获取到的第一个ScriptableObject
            Debug.Log(scriptableObject.name);

            // 从JSON文件加载SkillData
            WXEditorScriptObject wxObj = (WXEditorScriptObject)scriptableObject;

            // 查找当前场景中的 SkillManager 脚本的对象
            JsonDataRead jsDataRead = GameObject.FindObjectOfType<JsonDataRead>();

            if (jsDataRead != null)
            {
               // 找到了 SkillManager 对象
               Debug.Log("Found SkillManager object: " + jsDataRead.gameObject.name);
               jsDataRead.DATA_CDN_File = wxObj.ProjectConf.CDN;
            }
            else
            {
               // 没有找到 SkillManager 对象
               Debug.Log("SkillManager object not found in the scene.");
            }
         }
         else
         {
            Debug.LogWarning("scriptableObject 获取为空");
         }
      }
      else
      {
         Debug.LogWarning($"文件夹{editorWXConfigPath}中没有ScriptableObjects");
      }
   }
}
