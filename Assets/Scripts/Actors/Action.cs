using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Action
{
    static private void EndTurn(Actor actor)
    {
        // Controleer of de actor een Player component heeft
        if (actor.GetComponent<Player>() != null)
        {
            // Voer de functie StartEnemyTurn van de GameManager uit
            GameManager.Get.StartEnemyTurn();
        }
    }

    static public void Move(Actor actor, Vector2 direction)
    {
        // Controleer of iemand op de doelpositie is
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        // Als niemand daar is, kunnen we bewegen
        if (target == null)
        {
            actor.Move(direction);
            actor.UpdateFieldOfView();
        }

        // Eindig de beurt als dit de speler is
        EndTurn(actor);
    }
}

