using UnityEngine;

namespace Remorse.AI
{
    public class AIBehaviour : MonoBehaviour
    {
    //    [Header("Character")]
    //    public float health;

    //    [Header("Movement")]
    //    public float moveSpeed;
    //    public float runSpeed;
    //    public float rotateSpeed;

    //    [Header("Combat")]
    //    public float distanceForCombat;
    //    public GameObject bullet;
    //    public Transform spawnBullet;
    //    public float combatCooldown;

    //    [Header("Patrol")]
    //    public State currentState;
    //    public Transform[] patrolArea;
    //    [HideInInspector] public int index;

    //    //[HideInInspector] public FieldOfView fov;
    //    private float tempCooldown;
    //    private void Start()
    //    {
    //        //if (fov == null)
    //        //    fov = GetComponent<FieldOfView>();
    //        if (spawnBullet == null)
    //            spawnBullet = GameObject.FindGameObjectWithTag("EnemyBulletSpawn").transform;
    //    }

    //    private void Update()
    //    {
    //        if (currentState != null)
    //        {
    //            currentState.Tick(this);
    //        }

    //        if (health <= 0)
    //            Destroy(gameObject);
    //    }

    //    public void Patrolling()
    //    {
    //        if (patrolArea != null)
    //        {
    //            if (patrolArea.Length != index)
    //            {
    //                if (Vector3.Distance(transform.position, patrolArea[index].position) > 1f)
    //                {
    //                    Vector3 targetDirection = patrolArea[index].position - transform.position;
    //                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f));
    //                    transform.position = Vector3.MoveTowards(transform.position, patrolArea[index].position, moveSpeed * Time.deltaTime);
    //                }
    //                else
    //                    index++;
    //            }
    //            else
    //                index = 0;
    //        }
    //    }

    //    public void Chase()
    //    {
    //        if (fov.visibleTarget.Count > 0)
    //        {
    //            Vector3 targetDirection = fov.visibleTarget[0].position - transform.position;
    //            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f));
    //            transform.position = Vector3.MoveTowards(transform.position, fov.visibleTarget[0].position, runSpeed * Time.deltaTime);
    //        }
    //    }

    //    public bool IsInRange(float distance)
    //    {
    //        if (fov.visibleTarget.Count > 0)
    //            return distance > Vector3.Distance(transform.position, fov.visibleTarget[0].position);
    //        else
    //            return false;
    //    }

    //    public void Combat()
    //    {
    //        if (fov.visibleTarget.Count > 0)
    //        {
    //            Vector3 targetDirection = fov.visibleTarget[0].position - transform.position;
    //            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f));
    //            tempCooldown -= Time.deltaTime;
    //            if(tempCooldown < 0)
    //            {
    //                Shoot();
    //                tempCooldown = combatCooldown;
    //            }
    //        }
    //    }

    //    private void Shoot()
    //    {
    //        GameObject go = Instantiate(bullet, spawnBullet.position, Quaternion.identity);
    //        go.GetComponent<Combat.BulletBehaviour>().spawnBullet = spawnBullet;
    //    }
    }
}
