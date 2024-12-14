//ѕозвол€ет заблокировать частоту кадров до определЄнного значени€
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sych_scripts
{
	[AddComponentMenu("Sych scripts / Game / Base / Set target frame rate")]
	[DisallowMultipleComponent]
	public class Set_target_frame_rate : MonoBehaviour
	{
		[Tooltip("Ѕлокировка максимального fps на этом значение")]
		[SerializeField]
		public int Target_frame_rate = 30;

		private void Start()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = Target_frame_rate;
		}
	}
}