using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveText : MonoBehaviour
{
    //other scripts
    public GameManager gameManager;
    public SoundManager soundManager;
    public GameObject mainCamera;

    //texts for round values
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI roundAnimText;

    //texts for people values
    public TextMeshProUGUI peopleNumText;
    public TextMeshProUGUI peopleDiedText;
    public TextMeshProUGUI peopleCameText;
    //texts for wheat values
    public TextMeshProUGUI wheatNumText;
    public TextMeshProUGUI wheatDiffText;
    public TextMeshProUGUI wheatCollectedText;
    public TextMeshProUGUI wheatDestroyedText;
    public TextMeshProUGUI percentCultivatedText;
    //texts for city values
    public TextMeshProUGUI acresNumText;
    public TextMeshProUGUI acresDiffText;
    public TextMeshProUGUI acrePriceText;
    //game objects for all texts
    public GameObject peopleDiedCell;
    public GameObject peopleCameCell;
    public GameObject wheatCollectedCell;
    public GameObject wheatDestroyedCell;
    public GameObject percentCultivatedCell;
    public GameObject plagueCell;

    public GameObject roundAnimationYearObj;
    public GameObject roundAnimationPlagueObj;

    public TextMeshProUGUI gameOverText;

    //images
    public Image tilePeopleImg;
    public Image tileWheatImg;
    public Image tileCityImg;
    public Image foregroundRoundAnimationImg;
    public Sprite[] peopleImg;
    public Sprite[] wheatImg;
    public Sprite cityImg;
    //screens
    public GameObject mainScreen;
    public GameObject gameOverScreen;
    public GameObject roundAnimScreen;

    private bool[] animationSequence = new bool[4] { false, false, false, false };
    private bool startAnimation = true;
    private bool fadeOutStart = false;
    private bool playSound = true;

    private float deathPercent;
    private int peopleNum;
    private int wheatNum;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        tilePeopleImg.sprite = peopleImg[0];
        tileWheatImg.sprite = wheatImg[0];
        tileCityImg.sprite = cityImg;
    }

    // Update is called once per frame
    void Update()
    {
        GetResourceRemains();
        CheckValuesIfZero();

        acrePriceText.text = gameManager.acrePrice.ToString();//придумать другой вариант, анимация так не работает
        if (animationSequence[0] && !gameManager.isGameOver)
            StartAnimation(0);
        else if (animationSequence[1] && !gameManager.isGameOver && gameManager.isPlague)
            AnimateCard(1, ref tilePeopleImg, 1);
        else if (animationSequence[1] && !gameManager.isGameOver)
            AnimateCard(1, ref tilePeopleImg, 1);
        else if (animationSequence[2] && !gameManager.isGameOver)
            AnimateCard(2, ref tileWheatImg, 2);
        else if (animationSequence[3] && !gameManager.isGameOver)
            AnimateCard(3, ref tileCityImg, 3);
    }

    public void Animate(float deathPercentArg, int peopleNumArg, int wheatNumArg)
    {
        animationSequence[0] = true;
        deathPercent = deathPercentArg;
        peopleNum = peopleNumArg;
        wheatNum = wheatNumArg;
    }

    private void StartAnimation(int animNum)
    {
        if (startAnimation)
        {//fade in
            roundAnimScreen.SetActive(true);
            roundAnimText.text = gameManager.roundNum.ToString();
            float fadeInColor = foregroundRoundAnimationImg.color.a + Time.deltaTime;
            foregroundRoundAnimationImg.color = new Color(0, 0, 0, fadeInColor);
            if (fadeInColor > 0.98)
            {
                foregroundRoundAnimationImg.color = new Color(0, 0, 0, 0.98f);
                timer += Time.deltaTime;

                if (!gameManager.isPlague)
                {
                    plagueCell.SetActive(false);
                    if (playSound)
                    {
                        soundManager.GetSound(animNum, gameManager.isPlague);
                        playSound = false;
                        roundAnimationYearObj.SetActive(true);
                    }
                }
                else if (gameManager.isPlague)
                {
                    plagueCell.SetActive(true);
                    if (playSound)
                    {
                        soundManager.GetSound(animNum, gameManager.isPlague);
                        playSound = false;
                        roundAnimationYearObj.SetActive(true);
                        roundAnimationPlagueObj.SetActive(true);
                    }
                }
                if(timer > 2)
                {
                    fadeOutStart = true;
                    startAnimation = false;
                }
            }
        }
        if (fadeOutStart)
        {
            
            timer += Time.deltaTime;
            roundAnimationYearObj.SetActive(false);
            roundAnimationPlagueObj.SetActive(false);
            //fade out
            float fadeOutColor = foregroundRoundAnimationImg.color.a - Time.deltaTime;
            Debug.Log(fadeOutColor);
            foregroundRoundAnimationImg.color = new Color(0, 0, 0, fadeOutColor);
            if (fadeOutColor <= 0 && timer > 3)
            {
                timer = 0;
                foregroundRoundAnimationImg.color = new Color(0, 0, 0, 0);
                roundAnimScreen.SetActive(false);
                animationSequence[animNum + 1] = true;
                animationSequence[animNum] = false;
                startAnimation = true;
                fadeOutStart = false;
                playSound = true;
                return;
            }
        }
    }

    private void AnimateCard(float time, ref Image image, int animNum)
    {
        if (startAnimation)
        {//fade in
            float fadeInColor = image.color.r - Time.deltaTime / time;
            image.color = new Color(fadeInColor, fadeInColor, fadeInColor, 1);
            if (fadeInColor <= 0)
            {
                startAnimation = false;
                fadeOutStart = true;
            }
        }
        if (fadeOutStart)
        {
            UpdateImages(deathPercent, peopleNum, wheatNum, animNum); // new images
            UpdateNumbers(animNum); // animate numbers
            //start sound
            if (playSound)
            {
                soundManager.GetSound(animNum, gameManager.isPlague);
                playSound = false;
            }
            timer += Time.deltaTime;
            //fade out
            float fadeOutColor = Time.deltaTime / time + image.color.r;
            image.color = new Color(fadeOutColor, fadeOutColor, fadeOutColor, 1);
            if (fadeOutColor >= 1 && timer > 3)
            {
                timer = 0;
                startAnimation = true;
                fadeOutStart = false;
                playSound = true;
                if (animNum < animationSequence.Length - 1)
                    animationSequence[animNum + 1] = true;
                animationSequence[animNum] = false;
                return;
            }
        }
    }

    IEnumerator AnimateNumbers(int oldNum, int newNum, int step, TextMeshProUGUI resultNum, float time)
    {
        if (oldNum < newNum)
        {
            for(int i = oldNum; i<newNum; i += step)
            {
                resultNum.text = i.ToString();
                yield return new WaitForSeconds(time);
            }
            resultNum.text = newNum.ToString();
            yield break;
        }
        else if(oldNum > newNum)
        {
            for (int i = oldNum; i > newNum; i -= step)
            {
                resultNum.text = i.ToString();
                yield return new WaitForSeconds(time);
            }
            resultNum.text = newNum.ToString();
            yield break;
        }
    }

    public void UpdateImages(float deathPercent, int peopleNum, int wheatNum, int animNum)
    {
        if (animNum == 1)
        {
            if (!gameManager.isPlague)
            {
                if (deathPercent == 0)
                    tilePeopleImg.sprite = peopleImg[0];
                else if (deathPercent > 0 && deathPercent < 0.10)
                    tilePeopleImg.sprite = peopleImg[1];
                else if (deathPercent > 0.10 && deathPercent < 0.30)
                    tilePeopleImg.sprite = peopleImg[2];
                else if (deathPercent > 0.3 && deathPercent < 0.45)
                    tilePeopleImg.sprite = peopleImg[3];
            }

            if (gameManager.isPlague)
                tilePeopleImg.sprite = peopleImg[4];
        }
        else if (animNum == 2)
        {
            int wheatPerPerson = wheatNum / peopleNum;
            if (wheatPerPerson > 50)
                tileWheatImg.sprite = wheatImg[0];
            else if (wheatPerPerson > 40 && wheatPerPerson < 50)
                tileWheatImg.sprite = wheatImg[1];
            else if (wheatPerPerson > 20 && wheatPerPerson < 40)
                tileWheatImg.sprite = wheatImg[2];
            else
                tileWheatImg.sprite = wheatImg[3];
        }
        else if (animNum == 3)
        {
            tileCityImg.sprite = cityImg;
        }
    }

    public void UpdateNumbers(int animNum)
    {
        if(animNum == 1)
        {
            StartCoroutine(AnimateNumbers(int.Parse(peopleNumText.text), gameManager.peopleLiveNum, 1, peopleNumText, 0.1f));
            StartCoroutine(AnimateNumbers(int.Parse(peopleDiedText.text), gameManager.peopleDiedNum, 1, peopleDiedText, 0.1f));
            StartCoroutine(AnimateNumbers(int.Parse(peopleCameText.text), gameManager.peopleCame, 1, peopleCameText, 0.1f));
        }
        else if(animNum == 2)
        {
            StartCoroutine(AnimateNumbers(int.Parse(wheatNumText.text), gameManager.wheatNum, 5, wheatNumText, 0.0001f));
            StartCoroutine(AnimateNumbers(int.Parse(wheatCollectedText.text), gameManager.wheatCollected, 5, wheatCollectedText, 0.0001f));
            StartCoroutine(AnimateNumbers(int.Parse(wheatDestroyedText.text), gameManager.wheatDestoyed, 1, wheatDestroyedText, 0.01f));
            StartCoroutine(AnimateNumbers(int.Parse(percentCultivatedText.text), gameManager.percentCultivated, 1, percentCultivatedText, 0.1f));
        }
        else if(animNum == 3)
        {
            StartCoroutine(AnimateNumbers(int.Parse(acresNumText.text), gameManager.acresNum, 5, acresNumText, 0.05f));
            StartCoroutine(AnimateNumbers(int.Parse(acrePriceText.text), gameManager.acrePrice, 1, acrePriceText, 0.05f));
        }
    }

    private void GetResourceRemains()
    {
        wheatDiffText.enabled = true;
        acresDiffText.enabled = true;
        wheatDiffText.text = (int.Parse(wheatNumText.text) - gameManager.feedInputNum - gameManager.seedInputNum / 2 - gameManager.sellInputNum * gameManager.acrePrice).ToString();
        acresDiffText.text = (int.Parse(acresNumText.text) + gameManager.sellInputNum - gameManager.seedInputNum).ToString();
        if (int.Parse(wheatDiffText.text) == int.Parse(wheatNumText.text))
        {
            wheatDiffText.enabled = false;
        }
        if (int.Parse(acresDiffText.text) == int.Parse(acresNumText.text))
        {
            acresDiffText.enabled = false;
        }
    }

    private void CheckValuesIfZero()
    {
        peopleDiedCell.SetActive(true);
        peopleCameCell.SetActive(true);
        wheatCollectedCell.SetActive(true);
        wheatDestroyedCell.SetActive(true);
        percentCultivatedCell.SetActive(true);

        if (int.Parse(peopleDiedText.text) == 0)
            peopleDiedCell.SetActive(false);
        if (int.Parse(peopleCameText.text) == 0)
            peopleCameCell.SetActive(false);
        if (int.Parse(wheatCollectedText.text) == 0)
            wheatCollectedCell.SetActive(false);
        if (int.Parse(wheatDestroyedText.text) == 0)
            wheatDestroyedCell.SetActive(false);
        if (int.Parse(percentCultivatedText.text) == 0)
            percentCultivatedCell.SetActive(false);
    }

    public void GameOver(float averageDeathPercents, int roundsPlayed, float acresPerCitizen)
    {
        mainScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        var backgroundMusic = mainCamera.GetComponent<AudioSource>();
        backgroundMusic.volume = 0;
        Debug.Log($"Death percent = {averageDeathPercents}, acres per citizen = {acresPerCitizen}");
        if(roundsPlayed < 10)
        {
            gameOverText.text = "Более половины населения умерло от голода. " +
                "Начался бунт. Толпа ворвалась в городскую ратушу и вас убили.";
            soundManager.BadEnding();
        }
        else if(averageDeathPercents > 0.33 && acresPerCitizen < 7)
        {
            gameOverText.text = "Из-за вашей некомпетентности в управлении, " +
                "народ устроил бунт и изгнал вас из города. Теперь вы вынуждены " +
                "влачить жалкое существование в изгнании.";
            soundManager.BadEnding();
        }
        else if (averageDeathPercents > 0.1 && acresPerCitizen < 9)
        {
            gameOverText.text = "Вы правили железной рукой подобно Нерону или Ивану Грозному. " +
                "Народ вздохнул с облегчением, и никто больше не желает видеть вас правителем.";
            soundManager.BadEnding();
        }
        else if (averageDeathPercents > 0.03 && acresPerCitizen < 10)
        {
            gameOverText.text = "Вы справились вполне неплохо, у вас, кончено, есть недоброжелатели, " +
                "но многие хотели бы увидеть вас во главе города снова.";
            soundManager.GoodEnding();
        }
        else
        {
            gameOverText.text = "Фантастика! Карл Великий, Дизраэли и Джефферсон вместе не справились бы лучше!";
            soundManager.GoodEnding();
        }
    }
}
