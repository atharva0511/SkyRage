using UnityEngine.UI;
using UnityEngine;

public class StatDisplayPanel : MonoBehaviour
{
    public Transform[] StatEntries;
    // Start is called before the first frame update
    void Start()
    {

    }


    void OnEnable()
    {
        for (int i = 0; i < StatEntries.Length; i++)
        {
            if(StatEntries[i]!=null)
                StatEntries[i].GetChild(1).GetComponent<Text>().text = Stats.data[i].ToString();
        }
    }
}
