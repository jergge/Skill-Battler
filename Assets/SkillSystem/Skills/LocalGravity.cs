using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;

public class LocalGravity : Skill
{
    public float maxHeightFromWorldGravity;
    public float maxDistanceToCheck;
    public LayerMask layerMask;
    public float distanceFromNewSurface;
    public float durationChangeTime = .5f;
    float changeTime = 0;

    bool usingLocalGravityAlready = false;


    public override void Cast(Transform spawnLoaction, TargetInfo targetInfo)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ChangeGravity(Vector3 start, Vector3 end, Quaternion startRotation, Vector3 newUp) {
        Rigidbody rb = source.GetComponent<Rigidbody>();
        rb.useGravity = false;
        changeTime = 0;

        while (changeTime < durationChangeTime)
        {
            Quaternion targetQ = new Quaternion();
            targetQ.SetLookRotation(Vector3.forward,newUp);

            rb.MovePosition(Vector3.Lerp(start, end, changeTime/durationChangeTime));

            rb.MoveRotation(Quaternion.Slerp(startRotation, targetQ, changeTime/durationChangeTime));

            changeTime += Time.deltaTime;
            Debug.Log(changeTime);
        
        yield return null;
        }
        UniqueGravity grav = source.gameObject.AddComponent<UniqueGravity>();
        grav.gravityDirectionInWorld = newUp*-1;
        
    }

    IEnumerator RotateToWorldNormalGravity(Quaternion oldUp)
    {
        Quaternion targetQ = new Quaternion();
        targetQ.SetLookRotation(Vector3.forward, Vector3.up);

        Rigidbody rb = source.GetComponent<Rigidbody>();

        float duration = 1;
        float t = 0;

        while (t<duration){
            
            rb.MoveRotation(Quaternion.Slerp(oldUp, targetQ, t/duration));

            t+= Time.deltaTime;
            yield return null;
        }
    }

    void CastOnDouble(Vector3 LocalInputDirecton)
    {
        if (!usingLocalGravityAlready){
            Debug.Log("Local Gravity casting from a degegate..." + LocalInputDirecton);
            RaycastHit hit;
            Debug.DrawRay(source.transform.position, LocalInputDirecton * maxDistanceToCheck, Color.red, 5f);
            if (Physics.Raycast(source.transform.position, LocalInputDirecton, out hit, maxDistanceToCheck, layerMask))
            {
                Debug.DrawLine(source.transform.position, hit.point, Color.green, 3f);

                StartCoroutine(ChangeGravity(source.transform.position, hit.point + hit.normal * distanceFromNewSurface ,source.transform.rotation , hit.normal));
                usingLocalGravityAlready = !usingLocalGravityAlready;
            }
        } else {
            UniqueGravity ug;
            if (source.TryGetComponent<UniqueGravity>(out ug))
            {
                Destroy(ug);
                source.GetComponent<Rigidbody>().useGravity = true;
            }
            StartCoroutine(RotateToWorldNormalGravity(source.transform.rotation));
            usingLocalGravityAlready = !usingLocalGravityAlready;
        }
    }

    public override void OnStartInSpellbook()
    {
        PlayerController pc;
        if ( source.gameObject.TryGetComponent<PlayerController>(out pc) ) {
            pc.OnMultiJump += CastOnDouble;
        } else {
            Destroy(gameObject);
        }
    }

    public override void OnStartInWorld()
    {
        base.OnStartInWorld();
    }

    public override void UpdateInSpellBook()
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateInWorld()
    {
        //throw new System.NotImplementedException();
    }
}
