// !!!ID: eb18803a209c405fa354412717914307
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// Simple vertical menu system for Unity UI.
/// - Maintains a list of menu items and corresponding UnityEvents.
/// - Navigation: up/down keys cycle through items (wrap-around indexing).
/// - Confirm key invokes the UnityEvent for the selected item.
/// - Supports hover highlighting via IHover interface.
/// </summary>
namespace Basics.UI
{
    public interface IHover
    {
        void SetHover(bool state);
    }

    public class VerticalMenu : MonoBehaviour
    {
        public List<GameObject> menuItems = new();
        public List<UnityEvent> onSelect = new();
        public int selectedIndex = 0;
        public KeyCode upKey = KeyCode.UpArrow;
        public KeyCode downKey = KeyCode.DownArrow;
        public KeyCode confirmKey = KeyCode.Return;

        void Start() => UpdateHover();

        void Update()
        {
            if (Input.GetKeyDown(upKey))
            {
                selectedIndex = (selectedIndex - 1 + menuItems.Count) % menuItems.Count;
                UpdateHover();
            }

            if (Input.GetKeyDown(downKey))
            {
                selectedIndex = (selectedIndex + 1) % menuItems.Count;
                UpdateHover();
            }

            if (Input.GetKeyDown(confirmKey) && selectedIndex < onSelect.Count)
            {
                onSelect[selectedIndex]?.Invoke();
            }
        }

        void UpdateHover()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                var hover = menuItems[i].GetComponent<IHover>();
                if (hover != null)
                    hover.SetHover(i == selectedIndex);
            }
        }
    }
}
