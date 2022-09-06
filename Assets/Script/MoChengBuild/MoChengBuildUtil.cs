#if UNITY_EDITOR
using System.Data.SqlTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using Object = UnityEngine.Object;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.AddressableAssets;

namespace MoChengBuilder
{

    public static partial class MoChengBuildUtil
    {

        public static AddressableAssetSettings setting
        {
            get => AddressableAssetSettingsDefaultObject.Settings;
        }
        public static AddressableAssetProfileSettings profileSettings
        {
            get => setting.profileSettings;
        }

        public static void AddAsset<T>(string groupName, T asset) where T : Object
        {
            var targetGroup = setting.FindGroup(groupName);
            if (targetGroup == null)
            {
                throw new Exception($"Addressable Group : {targetGroup.Name} not exist");
            }
            var assetPath = AssetDatabase.GetAssetPath(asset);

            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (guid == string.Empty)
            {
                throw new Exception($"asset : {assetPath} not exist");
            }
            setting.CreateOrMoveEntry(guid, targetGroup);


        }


        public static AddressableAssetGroup CreateGroup(string groupName)
        {
            //判重
            var old = setting.groups.Find((p) => (p.Name == groupName));
            if (old != null)
            {
                setting.RemoveGroup(old);
            }


            var newGroup = setting.CreateGroup(groupName, false, false, true, null);

            // BundledAssetGroupSchema contentSchema = ScriptableObject.CreateInstance<BundledAssetGroupSchema>();


            // newGroup.AddSchema(contentSchema);
            // newGroup.SetProfileValue();

            // var updateSchema = ScriptableObject.CreateInstance<ContentUpdateGroupSchema>();
            // updateSchema.StaticContent = true;
            // newGroup.AddSchema(updateSchema);
            return newGroup;
        }
        public static void SetGroupData(AddressableAssetGroup targetGroup,string build,string load)
        {
            BundledAssetGroupSchema contentSchema = ScriptableObject.CreateInstance<BundledAssetGroupSchema>();


            targetGroup.AddSchema(contentSchema);
            targetGroup.SetProfileValue(build,load);

            var updateSchema = ScriptableObject.CreateInstance<ContentUpdateGroupSchema>();
            updateSchema.StaticContent = true;
            targetGroup.AddSchema(updateSchema);

        }
        public static void RemoveAllProfileValue()
        {
            var keys = profileSettings.GetAllVariableIds();
            foreach (var i in keys)
            {
                profileSettings.RemoveValue(i);
            }
        }
        public static HashSet<string> GetAllProfileValue() => profileSettings.GetAllVariableIds();
        public static void InitProfileValue()
        {
            RemoveAllProfileValue();

            profileSettings.CreateValue(BuildText, BuildPath);
            profileSettings.CreateValue(LoadText, LoadPath);


        }
        public static void SetProfileValue(this AddressableAssetGroup targetGroup,string build, string load)
        {

            var schema = targetGroup.GetSchema<BundledAssetGroupSchema>();
            schema.BuildPath.SetVariableByName(setting, build);
            schema.LoadPath.SetVariableByName(setting, load);
        }
        public static string GetText(this string path){
            var allPathName = profileSettings.GetVariableNames();
            foreach(var i in allPathName){
                if(profileSettings.GetValueByName(setting.activeProfileId, i) ==  path){
                    return i;
                }
            }
            return "";
        }

    }
}
#endif
