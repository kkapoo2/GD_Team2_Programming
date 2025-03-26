using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public GameObject[] Stages;
    public Player player;
    public Transform spawnPoint;

    public void NextStage()
    {
        DestroyAllCorpse();
        //Change Stage
        if(stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
    }

    //시체 삭제
    void DestroyAllCorpse()
    {
        GameObject[] corpses = GameObject.FindGameObjectsWithTag("Corpse");
        foreach(GameObject corpse in corpses)
        {
            Destroy(corpse);
        }
    }

    void PlayerReposition()
    {
        player.transform.position = spawnPoint.position;
        player.VelocityZero();
    }
}
