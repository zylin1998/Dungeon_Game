using UnityEngine;

public class FlipArea : MonoBehaviour
{
    [SerializeField]
    private bool isFlip;

    private void Awake()
    {
        var player = LayerMask.NameToLayer("PlayerCollider");
        var bounding = LayerMask.NameToLayer("EnemyBounding");

        Physics2D.IgnoreLayerCollision(bounding, player, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            var enemy = collision.GetComponent<RoleSystem.Enemy>();

            enemy.isFlip = !enemy.isFlip;
            enemy.Flip();
        }
    }
}
