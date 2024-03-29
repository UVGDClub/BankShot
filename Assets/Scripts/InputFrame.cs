﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BankShot
{
    [System.Serializable]
    public class InputFrame : MonoBehaviour
    {
        public static InputFrame Empty { get { return new InputFrame(); } }

        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool shoot;

        public bool HasInput { get { return up || down || left || right || shoot; } }

        // Equality operator overload - we want to check if input has changed
        // and we don't care about the frameNumber, timestamp or input axis
        public static bool operator ==(InputFrame i1, InputFrame i2)
        {
            // Null & reference check (i1 and i2 are the same if they're at the same location in memory obviously)
            if (ReferenceEquals(i1, i2)) return true;
            else if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return false;

            // Check the inputs
            return i1.right == i2.right &&
                   i1.down == i2.down &&
                   i1.left == i2.left &&
                   i1.up == i2.up &&
                   i1.shoot == i2.shoot;
        }

        // Inequality operator overload - see above, but inverted
        public static bool operator !=(InputFrame i1, InputFrame i2)
        {
            // Null & reference check (i1 and i2 are the same if they're at the same location in memory obviously)
            if (ReferenceEquals(i1, i2)) return false;
            else if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return true;

            // Check the inputs
            return i1.right != i2.right ||
                   i1.down != i2.down ||
                   i1.left != i2.left ||
                   i1.up != i2.up ||
                   i1.shoot != i2.shoot;
        }

        // Equals() should be identical to the equality operator
        public override bool Equals(object obj)
        {
            // Null & reference check (i1 and i2 are the same if they're at the same location in memory obviously)
            if (ReferenceEquals(this, obj)) return true;
            else if (ReferenceEquals(this, null) || ReferenceEquals(obj, null)) return false;

            // Check the inputs
            return right == ((InputFrame)obj).right &&
                   down == ((InputFrame)obj).down &&
                   left == ((InputFrame)obj).left &&
                   up == ((InputFrame)obj).up &&
                   shoot == ((InputFrame)obj).shoot;
        }

        // GetHashCode() should correspond to Equals() and the equality operator
        public override int GetHashCode()
        {
            return right.GetHashCode() ^ down.GetHashCode() ^ left.GetHashCode() ^
                up.GetHashCode() ^ shoot.GetHashCode();
        }
    }
}