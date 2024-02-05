using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private int nombreEnfants;

    void Start()
    {
        // Appel de la fonction pour compter les enfants
        nombreEnfants = CompterEnfants(transform);
        
        // Affichage du résultat dans la console
        Debug.Log("Nombre d'enfants : " + nombreEnfants);

        // Appel de la fonction pour obtenir le nombre d'objets ramassés par le joueur
        int objetsRamassesParJoueur = GetCollectibleCounter();

        // Affichage du résultat dans la console
        Debug.Log("Nombre d'objets ramassés par le joueur : " + objetsRamassesParJoueur);
    }

    public int GetCollectibleCounter()
    {
        // Vous pouvez également afficher le nombre d'objets ramassés par le joueur ici
        PickUp playerPickupScript = FindObjectOfType<PickUp>();
        if (playerPickupScript != null)
        {
            // Retourner le nombre d'objets ramassés par le joueur
            return playerPickupScript.GetNombreObjetsRamasses();
        }

        // Si le script du joueur n'est pas trouvé, retourner 0 par défaut
        return 0;
    }

    // Fonction récursive pour compter les enfants d'un objet
    int CompterEnfants(Transform parent)
    {
        int nombreEnfants = 0;

        // Parcours de tous les enfants de l'objet parent
        foreach (Transform enfant in parent)
        {
            // Incrémentation du nombre d'enfants
            nombreEnfants++;

            // Appel récursif pour les enfants de cet enfant
            nombreEnfants += CompterEnfants(enfant);
        }

        return nombreEnfants;
    }
}