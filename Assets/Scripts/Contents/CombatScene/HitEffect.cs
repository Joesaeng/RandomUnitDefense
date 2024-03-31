using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    WaitForSeconds wfs = new WaitForSeconds(1);
    public void Init(Vector3 pos, float wideAttackArea = 0)
    {
        transform.position = pos;
        transform.localScale = Vector3.one;
        transform.localScale *= wideAttackArea * 0.5f + 1;

        StartCoroutine("DestroyThisObject");
    }

    IEnumerator DestroyThisObject()
    {
        // yield return YieldCache.WaitForSeconds(1f);
        yield return wfs;

        Managers.Resource.Destroy(gameObject);
    }
}
