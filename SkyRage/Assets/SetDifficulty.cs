using UnityEngine.UI;
using UnityEngine;

public class SetDifficulty : MonoBehaviour {

    void Start()
    {
        if(PlayerPrefs.HasKey("Difficulty"))
            diffSelect.value = PlayerPrefs.GetInt("Difficulty")-1;
    }

    public Dropdown diffSelect;
	public void AssignDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", diffSelect.value+1);
    }
}
