using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTest : MonoBehaviour
{
   private bool buttonIsPush;
   int songNumber;
   private AudioManager a;

   private void Awake()
   {
      songNumber = 0;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.A))
      {
         
      }
      // if (buttonIsPush)
      // {
      //    FindObjectOfType<AudioManager>().Play("Track13");
      //    ChangeSong();
      // }
   }

   private void OnCollisionEnter(Collision collision)
   {
      if (collision.collider.CompareTag("HandTest"))
      {
         FindObjectOfType<AudioManager>().Play("ButtonSound");
         ChangeSong();
      }
   }

   private void OnCollisionExit(Collision other)
   {
      throw new NotImplementedException();
   }

   public void ChangeSong()
   {
      songNumber++;
      Debug.Log("Cancion cambio al numero" + songNumber);
      
      FindObjectOfType<AudioManager>().Play("Track13");
   }
}
