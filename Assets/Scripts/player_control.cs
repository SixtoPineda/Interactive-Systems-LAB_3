using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class player_control : MonoBehaviour
{
    public AudioClip pickup; //audio al recoger objeto
    public AudioClip uwin; //audio al acabar el juego
    public Text count_text; //texto de recuento de score
    public Text win_text; //texto de que hemos ganado

    public int count; //contador de SCORE

    public GameObject cam;//cámara desde donde ve el objato
    Vector3 pos_cam;


    void Start()
    {
        count = 0; //puntos en un inicio 0
        SetCount_text();//ploteamos texto
        win_text.text = "";//texto de WIN vacío aun no hemos conseguido el objetivo

    }

    void OnTriggerEnter(Collider other)
    {
        // Tag: para saber el nombre del Objeto
        // SetActive: para activar (True) o no (False) un objeto, que aparezca o no, sin eliminar
        // CompareTag: para comparar un Tag de un objeto con una string

        if (other.gameObject.CompareTag("Pick_Up"))
        {

            pos_cam = cam.transform.position; //obtenemos la posicion de la camara para poner el sonido

            AudioSource.PlayClipAtPoint(pickup, pos_cam); //PLAY audio al recoger un objeto
            other.gameObject.SetActive(false); //hacemos desaparecer al objeto al recogerlo
            count = count + 1;//auemnatamos los puntos +1
            SetCount_text();//ploteamos los puntos actualizados
        }
        if (count >= 6)
        {
            AudioSource.PlayClipAtPoint(uwin, pos_cam); //si se recogen todos los cubos, PLAY música final
            win_text.text = "¡GOOD JOB!";//TEXTo de que hemos ganado
        }

    }

    void SetCount_text()//función para plotear el texto de los puntos SCORE
    {
        count_text.text = "SCORE: " + count.ToString();
    }


}
