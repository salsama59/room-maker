using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private GameMapPosition[,] gameMap = new GameMapPosition[10,10];

    private static int LINE_INDEX = 0;
    private static int COLUMN_INDEX = 1;

    public int objectiveCount = 5;

    public GameObject objectiveModel;

    public GameObject playerOneScoreText;
    public GameObject playerTwoScoreText;

    private int playerOneScore = 0;
    private int playerTwoScore = 0;

    public GameMapPosition[,] GameMap { get => gameMap; set => gameMap = value; }

    private List<ObjectiveCollider> objectiveColliders = new List<ObjectiveCollider>();

    public GameObject resultText;

    public GameObject resultUi;


    public bool isEndGame = false;


    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, bool> objectivesDictionary = new Dictionary<string, bool>();
        for(int i = 0; i < objectiveCount; i++)
        {
            bool isCoordinateOk;
            int lineCoordinate = UnityEngine.Random.Range(0, GameMap.GetLength(LINE_INDEX));
            int columnCoordinate = UnityEngine.Random.Range(0, GameMap.GetLength(COLUMN_INDEX));
            string key = lineCoordinate.ToString() + columnCoordinate.ToString();
            isCoordinateOk = !objectivesDictionary.ContainsKey(key);

            if (isCoordinateOk)
            {
                objectivesDictionary.Add(key, true);
            }
            else
            {
                while (!isCoordinateOk)
                {
                    lineCoordinate = UnityEngine.Random.Range(0, GameMap.GetLength(LINE_INDEX));
                    columnCoordinate = UnityEngine.Random.Range(0, GameMap.GetLength(COLUMN_INDEX));
                    key = lineCoordinate.ToString() + columnCoordinate.ToString();
                    isCoordinateOk = !objectivesDictionary.ContainsKey(key);

                    if(isCoordinateOk)
                    {
                        objectivesDictionary.Add(key, true);
                    }
                }
            }
        }

        for(int line = 0; line < GameMap.GetLength(LINE_INDEX); line++)
        {
            for(int column = 0; column < GameMap.GetLength(COLUMN_INDEX); column++)
            {
                GameMapPosition gameMapPosition = new GameMapPosition();
                gameMapPosition.hasObjective = objectivesDictionary.ContainsKey(line.ToString() + column.ToString());
                GameMap[line, column] = gameMapPosition;
                if (gameMapPosition.hasObjective)
                {
                   GameObject objective = Instantiate(objectiveModel, ConvertMapCoprdinatesToWorldPosition(line, column), Quaternion.identity);
                    objectiveColliders.Add(objective.GetComponent<ObjectiveCollider>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsObjectivesOccupied() && !isEndGame)
        {

            if(this.playerOneScore > playerTwoScore)
            {
                resultText.GetComponent<TextMeshProUGUI>().text = resultText.GetComponent<TextMeshProUGUI>().text.Replace("{playerId}", "1");
            } else
            {
                resultText.GetComponent<TextMeshProUGUI>().text = resultText.GetComponent<TextMeshProUGUI>().text.Replace("{playerId}", "2");
            }

            resultUi.SetActive(true);
            isEndGame = true;
        }

        if(isEndGame)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            } else if (Input.GetKeyDown(KeyCode.E))
            {
                Application.Quit();
            }
        }
    }


    public Vector3 ConvertMapCoprdinatesToWorldPosition(int lineIndex, int columnIndex)
    {
        float lineUnitValue = (float)(5f - lineIndex) - 0.5f;
        float columnUnitValue = ((float)(5f - columnIndex) - 0.5f) * -1f;
        return new Vector3(columnUnitValue, lineUnitValue, 0f);
    }

    public MapCoordinates ConvertWorldPositionToMapCoprdinates(Vector3 position)
    {
        float lineIndexValue = (position.y * -1f) + 5f - 0.5f;
        float columnIndexValue = position.x + 5f - 0.5f;

        MapCoordinates mapCoordinates = new MapCoordinates();

        mapCoordinates.columnCoordinate = (int)columnIndexValue;
        mapCoordinates.lineCoordinate = (int)lineIndexValue;
        return mapCoordinates;
    }

    public void UpdateScore(string score, int playerId)
    {

        if(playerId == 1)
        {
            this.playerOneScoreText.GetComponent<TextMeshProUGUI>().text = score;
            playerOneScore = Int16.Parse(score);
        }
        else
        {
            this.playerTwoScoreText.GetComponent<TextMeshProUGUI>().text = score;
            playerTwoScore = Int16.Parse(score);
        }
        
    }

    public bool IsObjectivesOccupied()
    {
        return this.objectiveColliders.TrueForAll(objectiveCollider => {
            return objectiveCollider.isOccupiedByObstacle;
        });
    }
}
