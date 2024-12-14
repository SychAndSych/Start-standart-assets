//Зона в которой всё уничтожается или наносится "Смерть"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Arcade / Death zone")]
    [DisallowMultipleComponent]
    public class Death_zone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Health>())
            {
                collision.gameObject.GetComponent<Health>().Death();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.GetComponent<Health>())
            {
                col.gameObject.GetComponent<Health>().Death();
            }
            else
            {
                Destroy(col.gameObject);
            }
        }
    }
}