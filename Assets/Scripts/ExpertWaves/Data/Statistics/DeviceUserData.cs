using ExpertWaves.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExpertWaves {
	namespace Data {
		namespace Statistics {

			public class DeviceUserData : IBaseData {
				public DeviceUserData(string firstName, string lastName) {
					FirstName = firstName;
					LastName = lastName;
				}

				#region user information
				public string FirstName { get; set; }
				public string LastName { get; set; }
				public string FullName { get => $"{FirstName} {LastName}"; }
				#endregion

				#region App information
				public bool AppRated { get; set; }
				public int AppInstalled { get; set; }
				public int AppLastUsed { get; set; }
				public int AppUseCount { get; set; }
				public string AppVersion { get => Application.version; }
				public string AppDirectory => Application.dataPath;
				public string AppInstallerName => Application.installerName;
				public string AppLocation { get; set; }
				#endregion

				#region device information
				public int DeviceMemorySize { get => SystemInfo.systemMemorySize; }
				public int DeviceProcessorCount => SystemInfo.processorCount;
				public bool DeviceSupportAudio { get => SystemInfo.supportsAudio; }
				public bool DeviceSupportsAccelerometer { get => SystemInfo.supportsAccelerometer; }
				public bool DeviceSupportsGpuRecorder => SystemInfo.supportsGpuRecorder;
				public bool DeviceSupportsGyroscope => SystemInfo.supportsGyroscope;
				public bool DeviceSupportsRayTracing => SystemInfo.supportsRayTracing;
				public bool DeviceSupportsShadows => SystemInfo.supportsShadows;
				public bool DeviceSupportsVibration => SystemInfo.supportsVibration;
				public float DeviceBatteryLevel { get => SystemInfo.batteryLevel; }
				public string DeviceBatteryStatus { get => SystemInfo.batteryStatus.ToString(); }
				public string DeviceID { get => SystemInfo.deviceUniqueIdentifier; }
				public string DeviceModel => SystemInfo.deviceModel;
				public string DeviceName => SystemInfo.deviceName;
				public string DeviceType => SystemInfo.deviceType.ToString();
				public string DevicePersistentDataPath => Application.persistentDataPath;
				public string DeviceProcessorType => SystemInfo.processorType;
				public string DeviceTemporaryCachePath => Application.temporaryCachePath;
				#endregion


				#region graphics information
				public int GraphicsDeviceVendorID => SystemInfo.graphicsDeviceVendorID;
				public string GraphicsDeviceName => SystemInfo.graphicsDeviceName;
				public string GraphicsDeviceType => SystemInfo.graphicsDeviceType.ToString();
				public string GraphicsDeviceVendor => SystemInfo.graphicsDeviceVendor;
				public string GraphicsDeviceVersion => SystemInfo.graphicsDeviceVersion;
				public bool GraphicsMultiThreaded => SystemInfo.graphicsMultiThreaded;
				public int GraphicsMemorySize => SystemInfo.graphicsMemorySize;
				#endregion


				#region Public Functions
				public override void onLoadFileNotFound() {
					AppInstalled =  Constant.EpochTimeNow;
					AppLastUsed =  Constant.EpochTimeNow;
					AppUseCount = 0;
				}

				public override void onAfterLoad() {
					if (AppInstalled == Constant.EpochTimeZero)
						AppInstalled =Constant.EpochTimeNow;
					if (AppLastUsed == Constant.EpochTimeZero)
						AppLastUsed = Constant.EpochTimeNow;
				}
				
				public override void onBeforeSave() {
					AppUseCount += 1;
					AppLastUsed = Constant.EpochTimeNow;
				}
				#endregion
			}
		}
	}
}