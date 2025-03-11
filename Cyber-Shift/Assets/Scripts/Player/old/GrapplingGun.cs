using UnityEngine;

public class GrapplingGun : MonoBehaviour 
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;

    [SerializeField] private AudioClip[] sound;
    [SerializeField] private AudioSource audioSource;

    void Awake() 
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            StopGrapple();
        }
    }

    void LateUpdate() 
    {
        DrawRope();
    }

    void StartGrapple()
    {
        PlaySound(0);
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) 
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            PlaySound(1);
        }
    }

    void StopGrapple() 
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;
    
    void DrawRope() 
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() 
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() 
    {
        return grapplePoint;
    }

    public void PlaySound(int index)
    {
        if (index < 0 || index >= sound.Length)
        {
            Debug.LogWarning("“–≈¬Œ√¿ “–≈¬Œ√¿ «¬”  «¡≈∆¿À");
            return;
        }

        audioSource.clip = sound[index];
        audioSource.Play();
    }
}