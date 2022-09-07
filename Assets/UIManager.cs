using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PM
{
    public class UIManager : MonoBehaviour
    {
        public GameObject selectWindow;

        public void openSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void closeSelectWindow()
        {
            selectWindow.SetActive(false);
        }
    }
}