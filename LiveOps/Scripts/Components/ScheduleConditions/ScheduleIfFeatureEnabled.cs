﻿using Celeste.Components;
using Celeste.Features;
using System;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(ScheduleIfFeatureEnabled), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Scheduling/If Feature Enabled", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class ScheduleIfFeatureEnabled : Celeste.Components.Component, ILiveOpScheduleCondition
    {
        #region Data

        [Serializable]
        public class FeatureRequiredScheduleConditionData : ComponentData
        {
            public int featureGuid;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private FeatureRecord featureRecord;

        #endregion

        public override ComponentData CreateData()
        {
            return new FeatureRequiredScheduleConditionData();
        }

        public bool CanSchedule(Instance instance, InterfaceHandle<ILiveOpAssets> assets)
        {
            FeatureRequiredScheduleConditionData data = instance.data as FeatureRequiredScheduleConditionData;
            return featureRecord.IsFeatureEnabled(data.featureGuid);
        }
    }
}
