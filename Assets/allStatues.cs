using System.Linq;
using UnityEngine;

public class allStatues : MonoBehaviour
{
    public int totalNumber = 0;
    public int sealedNumber = 0;
    public int unsealedNumber = 0;

   // Start is called before the first frame update
    void Start()
    {
        CalculateStatueNumbers();
    }

    // Function to calculate the total, sealed, and unsealed numbers
    void CalculateStatueNumbers()
    {
        // Count the total number using LINQ
        totalNumber = transform.Cast<Transform>().Count(child => child.GetComponent<Spawner>() != null);

        // Count the number of sealed statues using LINQ
        sealedNumber = transform.Cast<Transform>().Count(child => child.GetComponent<Spawner>() != null && child.GetComponent<Spawner>().isSealed);

        // Calculate the number of unsealed statues
        unsealedNumber = totalNumber - sealedNumber;

    }

    private void FixedUpdate()
    {
        if (sealedNumber == totalNumber)
        {
            Debug.Log("All Statue is Sealed");
        }

        CalculateStatueNumbers();
    }
}

