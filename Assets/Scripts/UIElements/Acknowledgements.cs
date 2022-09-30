using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIElements
{
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

