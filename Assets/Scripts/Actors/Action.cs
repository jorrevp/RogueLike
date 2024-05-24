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
    static public void MoveOrHit(Actor actor, Vector2 direction)
    {
        // Controleer of iemand op de doelpositie is
        Actor target = GameManager.Get.GetActorAtLocation(actor.transform.position + (Vector3)direction);

        // Als niemand daar is, kunnen we bewegen
        if (target == null)
        {
            Move(actor, direction);
        }
        else
        {
            Hit(actor, target);
        }
    }
    static private void Move(Actor actor, Vector2 direction)
    {
        // Verplaats de actor in de opgegeven richting
        actor.Move(direction);
        // Update het zichtveld van de actor
        actor.UpdateFieldOfView();
        // Eindig de beurt als dit de speler is
        EndTurn();
    }
    static public void Hit(Actor actor, Actor target)
    {
        // Bereken de schade
        int damage = actor.Power - target.Defense;

        // Als de schade positief is, verminder de hitpoints van het target
        if (damage > 0)
        {
            target.DoDamage(damage);
        }

        // Genereer het bericht voor de UIManager
        string message = "";
        if (actor.GetComponent<Player>())
        {
            // Bericht voor speler
            if (damage > 0)
            {
                message = $"{actor.gameObject.name} hits {target.gameObject.name} for {damage} damage!";
            }
            else
            {
                message = $"{actor.gameObject.name} attacks {target.gameObject.name}, but does no damage.";
            }
            UIManager.Instance.AddMessage(message, Color.white);
        }
        else
        {
            // Bericht voor vijand
            if (damage > 0)
            {
                message = $"{actor.gameObject.name} hits {target.gameObject.name} for {damage} damage!";
            }
            else
            {
                message = $"{actor.gameObject.name} attacks {target.gameObject.name}, but does no damage.";
            }
            UIManager.Instance.AddMessage(message, Color.red);
        }
        // Eindig de beurt
        EndTurn();
    }
    static private void EndTurn()
    {
        // Eindig de beurt door de StartEnemyTurn-functie van GameManager uit te voeren
        GameManager.Get.StartEnemyTurn();
    }

}

