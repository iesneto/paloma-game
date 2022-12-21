/* 
 *  Ivo Seitenfus -  Gamob Mobile Games
 *  Shows an example on how to load/wait for something using coroutine
 *  and when the result is complete do an update
 *  
 *  Prints out:
 *  
 *  [00:00:00] Started game.
 *  [00:00:00] Loading Something in coroutine.
 *  [00:00:00] Continue Game... Shows a loading animation
 *  [00:00:05] Coroutine has ended... Update something and continue game
 * 
 */

using System.Collections;
using UnityEngine;

namespace Gamob
{

    public class CoroutineExample : MonoBehaviour
    {

        void Start()
        {
            Debug.Log("Started game.");
            StartCoroutine("LoadSomething");
            ContinueGameWhileWaitForCoroutine();

        }

        IEnumerator LoadSomething()
        {
            Debug.Log("Loading Something in coroutine.");
            yield return new WaitForSeconds(5.0f);
            CoroutineEndedUpdateSomething();

        }


        void ContinueGameWhileWaitForCoroutine()
        {
            Debug.Log("Continue Game... Shows a loading animation");
        }

        void CoroutineEndedUpdateSomething()
        {
            Debug.Log("Coroutine has ended... Update something and continue game");
        }

    }
}