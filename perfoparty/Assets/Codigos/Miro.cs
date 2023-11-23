using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miro : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float velocidadRotacion = 0.0f; // Velocidad de rotación gradual
    private Animator anim;
    public Rigidbody rb;
    public float fuerzaDeSalto = 8f;
    public bool puedoSaltar;
    private Vector3 direccionMovimiento;

    // Start is called before the first frame update
    void Start()
    {
        puedoSaltar = false;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        transform.Translate(direccionMovimiento * velocidadMovimiento * Time.deltaTime, Space.World);

    }

    // Update is called once per frame
    void Update()
    {

        string[] joystickNames = Input.GetJoystickNames();
for (int i = 0; i < joystickNames.Length; i++) {
    Debug.Log("Joystick " + i + ": " + joystickNames[i]);
}

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Calcula la dirección de movimiento
        direccionMovimiento = new Vector3(x, 0, y);
        direccionMovimiento.Normalize(); // Normaliza para que la velocidad sea la misma en todas las direcciones

        // Si hay movimiento, gira el personaje hacia la dirección de movimiento gradualmente
        if (direccionMovimiento != Vector3.zero)
        {
            anim.SetBool("dance", false);
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime * 8);
        }

        // Mueve al personaje en la dirección calculada
        

        // Actualiza las variables del Animator
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

        
        if (puedoSaltar)
        {

            if (Input.GetButtonDown("jump"))
            {
                anim.SetBool("dance", false);
                anim.SetBool("Salte", true);
                rb.AddForce(new Vector3(0, fuerzaDeSalto, 0), ForceMode.Impulse);
            }
            anim.SetBool("tocarsuelo", true);

        }
            else
            {

                EstoyCayendo();
            }
        
        if (Input.GetButtonDown("dance")){

            anim.SetBool("dance", true);

        }

        
    }

    public void EstoyCayendo(){
            anim.SetBool("tocarsuelo", false);
            anim.SetBool("Salte", false);
        }
}