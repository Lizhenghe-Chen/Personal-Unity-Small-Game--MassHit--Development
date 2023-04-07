using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIElements
{
    /* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class Acknowledgements : GlobalUIFunctions
    {

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PlayMaskAnimatorLeave();
                Invoke("FinnishAsyncLoad", 1f);
                //  FinnishAsyncLoad();
                //enabled = false;
            }
        }
    }
}

