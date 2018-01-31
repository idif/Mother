using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscUtilities {

	private static Camera mainCameraref;

	public static Camera mainCamera{

		get{
			if (mainCameraref == null) {
				mainCameraref = Camera.main;
			}
			return mainCameraref;
		}

	}

	public static int IndexOfBiggest (List <int> list){

		int result = 0;
		int resultIndex = 0;

		for (int i = 0; i < list.Count; i++) {

			if (list [i] > result) {

				result = list [i];
				resultIndex = i;

			}

		}

		return resultIndex;

	}


		
}
