using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour {

	bool speedUpTime = false;

	void Update () {
        if (Input.GetKey(KeyCode.LeftControl))
        {

            if (Input.GetKeyDown(KeyCode.F1))
            {

                GameManager.AddZakarium(1000);

            }

			if(Input.GetKeyDown(KeyCode.F2)){

				speedUpTime = !speedUpTime;
				GameManager.SetTimeSpeed(speedUpTime ? 20 : 1);

			}


        }
	}

}