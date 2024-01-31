using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorController : MonoBehaviour
{
    void Start()
    {
        // Cacher le curseur au démarrage du jeu
        Cursor.visible = false;
        // Bloquer le curseur au centre de l'écran
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Vérifier si la scène actuelle est la scène du menu
        if (SceneManager.GetActiveScene().name == "MenuGame")
        {
            // Afficher le curseur si sur la scène du menu
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Peut-être que vous voulez débloquer le curseur ici
        }
    
        // Vous pouvez ajouter d'autres conditions ici en fonction de vos besoins
    }
}
