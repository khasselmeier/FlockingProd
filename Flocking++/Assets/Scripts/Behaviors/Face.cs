using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Face : Align
{
    public override float getTargetAngle()
    {
        // Calculate the angle to face the target
        Vector3 direction = target.transform.position - character.transform.position;
        direction.y = 0; // Ignore vertical component
        if (direction.sqrMagnitude > 0.0f)
        {
            return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
        return character.transform.eulerAngles.y;
    }
}*/

public class Face : Align
{
    public override float getTargetAngle()
    {
        Vector3 direction = target.transform.position - character.transform.position;
        direction.y = 0;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        return targetAngle;
    }
}