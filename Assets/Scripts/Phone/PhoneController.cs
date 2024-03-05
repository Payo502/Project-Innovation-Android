using UnityEngine;

public class PhoneController : MonoBehaviour
{
    [SerializeField] private float pickupThreshold = 20f;
    private bool lastPickedupFrame = false;
    private Vector3 initialEulerAngles;
    [SerializeField] private audioManager audioManager;

    bool eventReceived = false;

    private void Start()
    {
        Input.gyro.enabled = true;
        CalibratePhone();

        ClientMessageManager.DoorsStuckEvent += ClientMessageManager_DoorsStuckEvent;
    }

    private void ClientMessageManager_DoorsStuckEvent()
    {
        eventReceived = true;
    }

    private void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Quaternion gyroAttitude = Input.gyro.attitude;
            Quaternion convertedAttitude = new Quaternion(gyroAttitude.x, gyroAttitude.y, -gyroAttitude.z, -gyroAttitude.w);

            // DEBUGGING
            transform.rotation = convertedAttitude;

            bool isPickedup = IsPhonePickedUp(convertedAttitude);

            if (isPickedup != lastPickedupFrame)
            {
                if (eventReceived)
                {
                    // Event has been triggered, send a different bool message
                    eventReceived = false;
                    ClientMessageManager.Singleton.SendBoolMessagesToServer(ClientToServerId.boolMessageDoorOpen, isPickedup);
                }
                else
                {
                    // Event has not been triggered, send the normal bool message
                    ClientMessageManager.Singleton.SendBoolMessagesToServer(ClientToServerId.boolMessagePhonePickedUp, isPickedup);
                }

                Debug.Log($"Phone Picked Up: {isPickedup}");
                lastPickedupFrame = isPickedup;
                audioManager.canPlay = isPickedup;
            }
        }
    }

    private bool IsPhonePickedUp(Quaternion currentAttitude)
    {
        Vector3 currentEulerAngles = currentAttitude.eulerAngles;

        float deltaX = Mathf.Abs(currentEulerAngles.x - initialEulerAngles.x);
        float deltaY = Mathf.Abs(currentEulerAngles.y - initialEulerAngles.y);

        deltaX = deltaX > 180f ? 360f - deltaX : deltaX;
        deltaY = deltaY > 180f ? 360f - deltaY : deltaY;

        return deltaX > pickupThreshold || deltaY > pickupThreshold;
    }

    public void CalibratePhone()
    {
        Quaternion initialAttitude = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        initialEulerAngles = initialAttitude.eulerAngles;
    }
}
