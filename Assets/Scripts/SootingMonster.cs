using System.Collections;
using UnityEngine;

public class ShootingMonster : Unit
{
    [SerializeField]
    private float shootRate = 2.0F;
    [SerializeField]
    private Color bulletColor = Color.white;

    private Bullet bulletPrefab;

    protected void Start()
    {
        bulletPrefab = Resources.Load<Bullet>("Bullet");
        
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootRate);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null)
        {
            Vector3 position = transform.position; position.y += 0.5F;
            Bullet newBullet = Instantiate(bulletPrefab, position, bulletPrefab.transform.rotation) as Bullet;

            newBullet.Parent = gameObject;
            newBullet.Direction = -newBullet.transform.right;
            newBullet.Color = bulletColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Проверяем, является ли столкнувшийся объект пулей
        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage();
            Destroy(bullet.gameObject); 
            return;
        }

    
        Character character = collider.GetComponent<Character>();
        if (character)
        {
            character.ReceiveDamage();
        }
    }
}
