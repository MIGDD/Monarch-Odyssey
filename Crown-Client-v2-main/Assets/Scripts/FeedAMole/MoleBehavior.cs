using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows.WebCam;

public class MoleBehavior : MonoBehaviour
{
    [Header("FAMManager")]
    [SerializeField] private FAMManager famManager;
    [SerializeField] private Material moleskin;

    //The offset of the mole sprite to hide it
    private Vector3 downPosition = new Vector3(0f, -6.0f, 0f);
    private Vector3 upPosition = Vector3.zero;

    //Time for mole show/hide animation (can still be hit)
    private float ShowHideDuration = 0.5f;
    //Time mole is above surface
    private float visibleDuration = 1.0f;

    //private CapsuleCollider capsuleCollider;
    //private Vector3 capsuleOffset;
    //private Vector3 capsuleSize;
    //private Vector3 capsuleOffsetHidden;
    //private Vector3 capsuleSizeHidden;

    //Mole Parameters
    private bool hittable = true;
    public enum MoleType {Standard, Jester, King};
    private MoleType moleType;
    private float jestRate = 0f;
    private float kingRate = 0.25f;
    //How many times the mole must be clicked
    private int lives;
    //The number of the mole on the grid 0-8
    private int moleIndex = 0;

    //Rendere object to access material of mole
    private Renderer ren;

    //Called before start
    private void Awake()
    {
        //Get the renderer
        ren = GetComponent<Renderer>();
        //Get the capsule collider
        //capsuleCollider = GetComponent<CapsuleCollider>();

    }
    // Start is called before the first frame update
    public void Activate(int level)
    {
        //Set the game level, decide which mole to spawn next, and spawn the choosen mole
        setLevel(level);
        CreateNext();
        StartCoroutine(ShowHide(downPosition, upPosition));
    }

    private IEnumerator ShowHide(Vector3 downPos, Vector3 upPos)
    {
        //Start animation from down position
        transform.localPosition = downPos;

        //Show the mole
        float elapsed = 0f;
        while (elapsed < ShowHideDuration) 
        {
        transform.localPosition = Vector3.Lerp(downPos, upPos, elapsed / ShowHideDuration);
        // Update the elapsed time
        elapsed += Time.deltaTime;
        yield return null;
        }

        //Make sure animation is at up position
        transform.localPosition = upPos;

        //Wait for the visible duration to pass (while the mole is above groud)
        yield return new WaitForSeconds(visibleDuration);

        //Hide the mole
        elapsed = 0f;
        while (elapsed < ShowHideDuration)
        {
        transform.localPosition = Vector3.Lerp(upPos, downPos, elapsed / ShowHideDuration);
        // Update the elapsed time
        elapsed += Time.deltaTime;
        yield return null;
        }

        //End animation at down position
        transform.localPosition = downPos;

        //If it is still hittable after going down then it was missed
        if(hittable)
        {
            hittable = false;
            //If it isn't a jester then we do nothing
            famManager.Missed(moleIndex, moleType != MoleType.Jester);
        }
    }

    private void OnMouseDown(){
        if(hittable)
        {
            switch(moleType)
            {
                case MoleType.Standard:
                    
                    ren.material.SetColor("_Color", Color.green);
                    //Play defeated animation

                    //Add 1 point for hitting standard mole
                    famManager.addScore(moleIndex, 1);
                    //Stop the animation
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    //Turn off hittable so mole can't keep being tapped for score
                    hittable = false;
                    break;

                case MoleType.Jester:
                    ren.material.SetColor("_Color", Color.red);
                    //Play jester clicked animation

                    //Subtract score for hitting jester
                    famManager.subtractScore(moleIndex);
                    //Stop the animation
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    //Turn off hittable so mole can't keep being tapped for score
                    hittable = false;
                    break;
                case MoleType.King:
                    // If lives equal 2, reduce and change color to white
                    if(lives == 2)
                    {
                        lives--;
                        ren.material = moleskin;
                        //play hit animation
                    }
                    else
                    {
                        ren.material.SetColor("_Color", Color.green);
                        //Play defeated animation
                        
                        //Add 2 points for hitting king mole
                        famManager.addScore(moleIndex, 2);
                        //Stop the animation
                        StopAllCoroutines();
                        StartCoroutine(QuickHide());
                        //Turn off hittable so mole can't keep being tapped for score
                        hittable = false;
                    }
                    break;
                default:
                    break;
                
            }
        }
    }

    private IEnumerator QuickHide() {
        yield return new WaitForSeconds(0.25f);
        /*While waiting the mole may have respawned in the same spot,
        so check that hasn't happened before hiding it. This will stop if flickering*/
        if(!hittable)
        {
            Hide();
        }
    }

    //Function to force hide the mole (called after the mole is tapped)
    public void Hide(){
        //Set the appropriate mole parameters to hide it
        transform.localPosition = downPosition;
    }

    //Function to determine which type of mole to spawn next
    private void CreateNext(){
        float random = Random.Range(0f, 1f);
        if (random < jestRate)
        {
            //Make a jester
            moleType = MoleType.Jester;
            ren.material.SetColor("_Color", Color.magenta);
            //Create jester mole model
            lives = 1;
        }
        else
        {
            random = Random.Range(0f, 1f);
            if(random < kingRate)
            {
               moleType = MoleType.King;
               ren.material.SetColor("_Color", Color.yellow);
               //Create king mole model
               lives = 2; 
            }
            else
            {
                //Make a standard mole
                moleType = MoleType.Standard;
                ren.material = moleskin;
                //Create regular mole model
                lives = 1;
            }
        }
        // Set hittable variable to true so that the we can register an onclick event next time the mole appears
        hittable = true;
    }

    //As the game progresses the difficulty increases
    private void setLevel(int level) {
        //As level increases, increase the jest rate to 0.25 at level 10
        jestRate = Mathf.Min(level * 0.025f, 0.25f);

        //Increase the amount of kings until 80% at level 40
        kingRate = Mathf.Min(level * 0.0255f, 0.8f);

        //Duration bounds get quicker as we progress
        float durationMin = Mathf.Clamp(1 - level * 0.1f, 0.10f, 1f);
        float durationMax = Mathf.Clamp(2 - level * 0.1f, 0.10f, 1.5f);
        visibleDuration = Random.Range(durationMin, durationMax);
    }

    //Used by the game manager to uniquely identify moles
    public void setIndex(int index)
    {
        moleIndex = index;
    }

    public void stopGame()
    {
        hittable = false;
        StopAllCoroutines();
    }
}

    
