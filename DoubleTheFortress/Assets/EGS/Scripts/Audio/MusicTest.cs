using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTest : MonoBehaviour
{
   private bool buttonIsPush;
   int songNumber;

   private void Awake()
   {
      songNumber = 0;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.A))
      {
         buttonIsPush = true;
      }
      else
      {
         buttonIsPush = false;
      }
      if (buttonIsPush)
      {
         FindObjectOfType<AudioManager>().Play("Track13");
         ChangeSong();
      }
   }

   public void ChangeSong()
   {
      songNumber++;
      Debug.Log("Cancion cambio al numero" + songNumber);
   }
}
