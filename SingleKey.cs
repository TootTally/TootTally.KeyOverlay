using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TootTally.Graphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TootTally.KeyOverlay
{
    public class SingleKey
    {
        private CustomButton _gameObject;
        public bool isPressed;
        private int _pressCount; 

        public SingleKey(CustomButton gameObject)
        {
            _gameObject = gameObject;
        }

        public void OnKeyPress()
        {
            _gameObject.button.OnPointerEnter(new PointerEventData(EventSystem.current));
            _pressCount++;
            _gameObject.textHolder.text = _pressCount.ToString();
            isPressed = true;
        }
        public void OnKeyRelease()
        {
            _gameObject.button.OnPointerExit(new PointerEventData(EventSystem.current));
            isPressed = false;
        }
    }
}
