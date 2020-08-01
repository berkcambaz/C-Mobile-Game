using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
}
