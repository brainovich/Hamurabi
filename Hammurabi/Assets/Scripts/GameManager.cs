using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_InputField feedInputField;
    public TMP_InputField seedInputField;
    public TMP_InputField sellInputField;
    public ActiveText activeText;
    public List<float> deathPercents = new List<float>();

    //InputsNumbers
    public int feedInputNum;
    public int seedInputNum;
    public int sellInputNum;

    //People
    public int peopleLiveNum = 100;
    public int peopleDiedNum = 0;
    public int peopleCame;

    //Wheat
    public int wheatNum = 2800;
    public int percentCultivated;
    public int wheatDestoyed;
    public int wheatCollected;

    //City
    public int acresNum = 1000;
    public int acrePrice;

    //Plague
    private const int plagueChance = 10;
    public bool isPlague = false;

    public int roundNum = 1;
    private const int roundsToPlay = 10;

    public bool isGameOver = false;

    void Start()
    {
        acrePrice = Random.Range(17, 26);
    }

    // Update is called once per frame
    void Update()
    {
        ValidateInput();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Calculate();
        }
    }

    public void Calculate()
    {
        if ( !(int.TryParse(feedInputField.text.ToString(), out feedInputNum)) || 
            !(int.TryParse(seedInputField.text.ToString(), out seedInputNum)) || 
            !(int.TryParse(sellInputField.text.ToString(), out sellInputNum)) )
        {
            return;
        }
        isPlague = false;
        if ((feedInputNum + seedInputNum/2 + sellInputNum*acrePrice) <= wheatNum && // check if enough wheat
            (seedInputNum + -sellInputNum) <= acresNum && // check if enough acres
            roundNum <= roundsToPlay)//game is not over
        {
            //random values calculated in each round
            percentCultivated = Random.Range(1, 6);
            acrePrice = Random.Range(17, 26);
            
            CalculatePeople(feedInputNum, percentCultivated);
            if (deathPercents[roundNum - 1] > 0.45 || peopleLiveNum <= 0)
            {
                isGameOver = true;
                activeText.GameOver(1, roundNum, 1);
            }
            CalculateWheat(feedInputNum, seedInputNum, sellInputNum, percentCultivated);
            acresNum += sellInputNum;

            activeText.Animate(deathPercents[roundNum - 1], peopleLiveNum, wheatNum);
            if(roundNum+1 <= roundsToPlay)
            {
                roundNum++;
                activeText.roundText.text = roundNum.ToString();
            }
            else if(roundNum + 1  > roundsToPlay)
            {
                float averageDeathPercent = 0;
                float acresPerCitizen = 0;
                isGameOver = true;
                for (int i = 0; i < 11; i++)
                {
                    averageDeathPercent += deathPercents[i];
                }
                averageDeathPercent = averageDeathPercent / 10;
                acresPerCitizen = acresNum / peopleLiveNum;
                activeText.GameOver(averageDeathPercent, roundNum, acresPerCitizen);
            }
        }

        //reset all inputs
        feedInputNum = 0;
        seedInputNum = 0;
        sellInputNum = 0;

        feedInputField.text = string.Empty;
        seedInputField.text = string.Empty;
        sellInputField.text = string.Empty;
    }

    void CalculatePeople(int feed, int percentCultivated)
    {
        //calculate death of hunger
        int need = 20;
        peopleDiedNum = (int)((peopleLiveNum * need - feed) / need);
        if (peopleDiedNum < 0)
            peopleDiedNum = 0;
        peopleLiveNum -= peopleDiedNum;//result

        //add statistic of deaths
        deathPercents.Add((float)peopleDiedNum / (float)peopleLiveNum);

        //calculate newcomers
        peopleCame = peopleDiedNum / 2 + (5 - percentCultivated) * wheatNum / 600 + 1;
        if (peopleCame > 50)
            peopleCame = 50;
        else if (peopleCame < 0)
            peopleCame = 0;
        peopleLiveNum += peopleCame;//result

        //check for plague
        if (Random.Range(1, 101) < plagueChance)
        {
            isPlague = true;
            int diedofPlague = peopleLiveNum / 2;
            peopleLiveNum -= diedofPlague;
            peopleDiedNum += diedofPlague;
        }
    }

    void CalculateWheat(int feed, int seed, int sell, int percentCultivated)
    {
        //minus input
        wheatNum -= feedInputNum;

        //calculate farmed wheat
        int acresCultivated;
        if (seedInputNum <= peopleLiveNum * 10)
            acresCultivated = seedInputNum;
        else
            acresCultivated = peopleLiveNum * 10;
        wheatCollected = acresCultivated * percentCultivated;
        wheatNum += wheatCollected;//result

        //calculate wheat eaten by rats
        wheatDestoyed = (int)(wheatNum * Random.Range(0, 0.07f));
        wheatNum -= wheatDestoyed;//result

        //minus wheat to seed (.5 wheat per acre)
        wheatNum -= seedInputNum / 2;
        //minus whear to sell/plus wheat to buy
        wheatNum += -sellInputNum * acrePrice;
    }

    private void ValidateInput()
    {
        if (!int.TryParse(feedInputField.text.ToString(), out feedInputNum))
        {
            feedInputField.text = string.Empty;
        }
        if (!int.TryParse(seedInputField.text.ToString(), out seedInputNum))
        {
            seedInputField.text = string.Empty;
        }
        if ((sellInputField.text.Length > 0 && sellInputField.text[0] != '-') || sellInputField.text.Length > 1)
        {
            if ((!int.TryParse(sellInputField.text.ToString(), out sellInputNum)))
            {
                seedInputField.text = string.Empty;
            }
        }
    }
}
