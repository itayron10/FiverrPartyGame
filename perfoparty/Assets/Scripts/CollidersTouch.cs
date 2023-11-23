using UnityEngine;

public class DetectarGolpes : MonoBehaviour
{
    public float fuerzaDeEmpuje = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            // Obtiene la dirección desde el jugador actual al otro jugador
            Vector3 direccion = collision.transform.position - transform.position;

            // Establece la componente vertical (y) de la dirección a cero para evitar movimiento vertical
            direccion.y = 0;

            // Normaliza la dirección para obtener una magnitud de 1
            direccion.Normalize();

            // Aplica un desplazamiento en la dirección opuesta
            Rigidbody jugadorRB = collision.gameObject.GetComponent<Rigidbody>();
            jugadorRB.MovePosition(jugadorRB.position + direccion * fuerzaDeEmpuje * Time.fixedDeltaTime);
        }
    }
}
