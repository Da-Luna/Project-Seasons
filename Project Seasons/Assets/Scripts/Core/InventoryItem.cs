using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InventoryItem is responsible for handling items that can be collected and added to an inventory. 
/// It uses a CircleCollider2D to detect when an item is collected.
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class InventoryItem : MonoBehaviour, IDataPersister
{
    [Tooltip("The key representing this inventory item.")]
    public string inventoryKey = "";

    [Tooltip("Layers that can interact with this item.")]
    public LayerMask layers;

    [Tooltip("Disable the item upon collection.")]
    public bool disableOnEnter = false;

    [HideInInspector]
    new public CircleCollider2D collider; // The CircleCollider2D component

    [Tooltip("Settings for data persistence.")]
    public DataSettings dataSettings;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void OnEnable()
    {
        collider = GetComponent<CircleCollider2D>();
        PersistentDataManager.RegisterPersister(this);
    }

    /// <summary>
    /// Called when the behavior becomes disabled.
    /// </summary>
    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    /// <summary>
    /// Resets the item to its default state.
    /// </summary>
    void Reset()
    {
        layers = LayerMask.NameToLayer("Everything");
        collider = GetComponent<CircleCollider2D>();
        collider.radius = 5;
        collider.isTrigger = true;
        dataSettings = new DataSettings();
    }

    /// <summary>
    /// Called when another object enters the trigger collider attached to this object.
    /// </summary>
    /// <param name="other">The Collider2D object that entered the trigger.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & layers) != 0)
        {
            var ic = other.GetComponent<InventoryController>();
            if (ic != null)
            {
                ic.AddItem(inventoryKey); // Add the item to the inventory
                if (disableOnEnter)
                {
                    gameObject.SetActive(false); // Disable the item if required
                    Save(); // Save the state
                }
            }
        }
    }

    /// <summary>
    /// Marks this item as dirty for saving.
    /// </summary>
    public void Save()
    {
        PersistentDataManager.SetDirty(this);
    }

    /// <summary>
    /// Draws a gizmo in the editor for visual debugging.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "InventoryItem", false);
    }

    /// <summary>
    /// Gets the data settings for persistence.
    /// </summary>
    /// <returns>The data settings.</returns>
    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    /// <summary>
    /// Sets the data settings for persistence.
    /// </summary>
    /// <param name="dataTag">The data tag.</param>
    /// <param name="persistenceType">The type of persistence.</param>
    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    /// <summary>
    /// Saves the current state of the item (whether it is active or not).
    /// </summary>
    /// <returns>The saved data.</returns>
    public Data SaveData()
    {
        return new Data<bool>(gameObject.activeSelf);
    }

    /// <summary>
    /// Loads the state of the item.
    /// </summary>
    /// <param name="data">The data to load.</param>
    public void LoadData(Data data)
    {
        Data<bool> inventoryItemData = (Data<bool>)data;
        gameObject.SetActive(inventoryItemData.value);
    }
}
