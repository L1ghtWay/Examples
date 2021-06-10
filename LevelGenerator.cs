using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Характеристики уровня")]
    [SerializeField] private string word;
    [SerializeField] private int maxTurns;
    [SerializeField] private int maxEnergy;

    [Header("Префабы и игровые объекты")]
    [SerializeField] private Material[] lettersMaterials;
    [SerializeField] private GameObject cubeTemplalte;
    [SerializeField] private Transform cubesContainer;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Вспомогательные поля")]
    public static LevelGenerator Instance;    

    private Dictionary<char, Material> letters = new Dictionary<char, Material>(); //список всех доступных букв
    private Dictionary<char, Material> selectedLetters = new Dictionary<char, Material>(); //список букв, присутствующих в слове

    private List<Vector3> startPositions;
    private List<Quaternion> startRotations;

    private int curTurns;
    private int curEnergy;
    private bool cubeIsMoving = false; 

    public int LastCubeIndex { get; set; }
    public bool CubeInCenter { get; set; } = false;
    public bool CubeIsReturning { get; set; } = false;

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        CheckWord.Instance.SetWordLength(word.Length);
        UIManager.Instance.SetWordText(word);

        startPositions = new List<Vector3>(10);
        startRotations = new List<Quaternion>(10);

        curTurns = maxTurns;
        curEnergy = maxEnergy;

        FillDictionary();
        LettersSelection();
        SpawnCubes();
    }


    private void FillDictionary() //заполняем список всех доступных букв
    {
        letters.Add('A', lettersMaterials[0]);
        letters.Add('B', lettersMaterials[1]);
        letters.Add('C', lettersMaterials[2]);
        letters.Add('D', lettersMaterials[3]);
        letters.Add('E', lettersMaterials[4]);
        letters.Add('F', lettersMaterials[5]);
        letters.Add('G', lettersMaterials[6]);
        letters.Add('H', lettersMaterials[7]);
        letters.Add('I', lettersMaterials[8]);
        letters.Add('J', lettersMaterials[9]);
        letters.Add('K', lettersMaterials[10]);
        letters.Add('L', lettersMaterials[11]);
        letters.Add('M', lettersMaterials[12]);
        letters.Add('N', lettersMaterials[13]);
        letters.Add('O', lettersMaterials[14]);
        letters.Add('P', lettersMaterials[15]);
        letters.Add('Q', lettersMaterials[16]);
        letters.Add('R', lettersMaterials[17]);
        letters.Add('S', lettersMaterials[18]);
        letters.Add('T', lettersMaterials[19]);
        letters.Add('U', lettersMaterials[20]);
        letters.Add('V', lettersMaterials[21]);
        letters.Add('W', lettersMaterials[22]);
        letters.Add('X', lettersMaterials[23]);
        letters.Add('Y', lettersMaterials[24]);
        letters.Add('Z', lettersMaterials[25]);
    }


    public void TakeTurn(int index)
    {
        if (!CubeInCenter)
        {
            curTurns--;
            UIManager.Instance.ChangeTurnsText(curTurns);
            LastCubeIndex = index;
            cubeIsMoving = true;
        }
    }

    public void ReturnCube()
    {
        if(!CubeInCenter && cubeIsMoving)
        {
            curTurns++;
            UIManager.Instance.ChangeTurnsText(curTurns);
            CubeIsReturning = true;
            cubeIsMoving = false;
        }
    }


    public void TakeEnergy() 
    {        
        curEnergy--;
        UIManager.Instance.ChangeEnergyText(curEnergy);        
    }


    public void StopSwipe() 
    {
        foreach (Transform eachChild in cubesContainer)
        {            
            eachChild.GetComponent<Rigidbody>().isKinematic = true;            
        }        
    }


    public void StartSwipe ()
    {
        foreach (Transform eachChild in cubesContainer)
        {
            eachChild.GetComponent<Rigidbody>().isKinematic = false;            
        }
    }


    public void RespawnCubes()
    {
        for (int i = 0; i < word.Length; i++)
        {
            cubesContainer.GetChild(i).position = startPositions[i];
            cubesContainer.GetChild(i).rotation = startRotations[i];
        }
    }


    public string GetWord()
    {      
        return word;
    }


    private void LettersSelection() //заполняем список букв, присутствующих в слове и удаляем их из списка всех доступных букв
    {
        for (int i = 0; i < word.Length; i++) 
        {
            char letter = word[i];

            if (!selectedLetters.ContainsKey(letter)) 
            {
                selectedLetters.Add(letter, letters[letter]);
                letters.Remove(letter);
            }
        }
    }


    private void SpawnCubes() 
    {
        for (int i=0; i < word.Length; i++) 
        {
            Vector3 pos = spawnPoints[i].position;
            int[] degrees = { -90, 0, 90 };
            Quaternion rot = Quaternion.Euler(degrees[Random.Range(0, 3)], degrees[Random.Range(0, 3)], degrees[Random.Range(0, 3)]);

            GameObject spawnedCube = Instantiate(cubeTemplalte, pos, rot, cubesContainer);
            spawnedCube.GetComponent<Swipe>().Index = i; // даем кубу индекс

            startPositions.Add(spawnedCube.transform.position);
            startRotations.Add(spawnedCube.transform.rotation);            

            foreach (Renderer side in spawnedCube.GetComponentsInChildren<Renderer>())
            {
                List<Material> materials = new List<Material>(letters.Values);
                side.material = materials[Random.Range(0, materials.Count)];
            }

            spawnedCube.transform.GetChild(0).GetComponent<Renderer>().material = selectedLetters[word[i]];            
        }
    }  
}
