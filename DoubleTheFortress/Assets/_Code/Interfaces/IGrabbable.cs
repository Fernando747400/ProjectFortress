using UnityEngine;
using System;
using DebugStuff.Inventory;


public class IGrabbable : MonoBehaviour
{
   [Header("IGrabbable")]
   [SerializeField] private MeshRenderer[] _renderers;
   public MeshRenderer[] Renderers => _renderers;
   [SerializeField] private Material[] myMaterials;
   [SerializeField] private Collider _collider;
   
   [SerializeField] private PlayerSelectedItem _selectedItem;
   [SerializeField] private PlayerSelectedItem _typeOfItem;

   public PlayerSelectedItem Item
   {
      get => _selectedItem;
   }
   public PlayerSelectedItem TypeOfItem
   {
      get => _typeOfItem;
   }

   public void SetMaterials(Material material)
   {
      foreach (var renderer in _renderers)
      {
         renderer.material = material;
      }
   }
   public void ResetMaterials()
   {
      for (int i = 0; i < _renderers.Length; i++)
      {
         _renderers[i].material = myMaterials[i];
      }
   }

   public virtual void HandleSelectedState(bool isSelecting)
   {
      if (_collider!= null)
      {
         _collider.enabled = !isSelecting;
      }
   }
   
}
