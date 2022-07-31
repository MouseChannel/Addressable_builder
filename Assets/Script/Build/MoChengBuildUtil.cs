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
            var newGroup = setting.CreateGroup(groupName, false, false, true, null);

            BundledAssetGroupSchema contentSchema = ScriptableObject.CreateInstance<BundledAssetGroupSchema>();


            newGroup.AddSchema(contentSchema);
            newGroup.SetProfileValue();

            var updateSchema = ScriptableObject.CreateInstance<ContentUpdateGroupSchema>();
            updateSchema.StaticContent = true;
            newGroup.AddSchema(updateSchema);
            return newGroup;
        }
        public static void RemoveAllProfileValue()
        {
            var keys = profileSettings.GetAllVariableIds();
            foreach (var i in keys)
            {
                profileSettings.RemoveValue(i);
            }
        }
        public static void InitProfileValue()
        {
            RemoveAllProfileValue();
            profileSettings.CreateValue(BuildText, BuildPath);
            profileSettings.CreateValue(LoadText, LoadPath);


        }
        public static void SetProfileValue(this AddressableAssetGroup targetGroup)
        {

            var schema = targetGroup.GetSchema<BundledAssetGroupSchema>();
            schema.BuildPath.SetVariableByName(setting, BuildText);
            schema.LoadPath.SetVariableByName(setting, LoadText);
        }

    }
}
#endif
