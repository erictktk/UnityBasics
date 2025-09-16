// !!!ID: 29c867fe0ef94480b751db05b47208de
using UnityEngine;

/// <summary>
/// Toggles a Behaviour component on after a delay, then off after another delay.
/// - Disables the component at Start.
/// - Enables it after onDelay seconds.
/// - Disables it again after offDelay seconds, then stops running the script.
/// </summary>
namespace Basics
{
    public class ToggleComponentOnOff : MonoBehaviour
    {
        public Behaviour componentToToggle;
        public float onDelay = 1f;
        public float offDelay = 2f;

        private enum State { WaitingOn, WaitingOff, Done }
        private State state = State.WaitingOn;
        private float timer;

        void Start()
        {
            if (componentToToggle != null)
                componentToToggle.enabled = false;
            timer = 0f;
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (state == State.WaitingOn && timer >= onDelay)
            {
                componentToToggle.enabled = true;
                state = State.WaitingOff;
                timer = 0f;
            }
            else if (state == State.WaitingOff && timer >= offDelay)
            {
                componentToToggle.enabled = false;
                state = State.Done;
                enabled = false;
            }
        }
    }
}
