using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerweidAdvisor : MonoBehaviour
{
    private Queue<Advice> adviceQueue = new Queue<Advice>();

    [Header("H Contortus")]
    [Tooltip("Fecundity (eggs per day per adult)")]
    public float varLabda = 2250.0f;
    [Tooltip("Instantaneous daily mortality rate of adult nematodes")]
    public float varMu = 0.05f;
    [Tooltip("Probability that an egg will develop to L3 and migrate onto pasture")]
    public float varQ = -1;         // calculated with a formula;
    [Tooltip("Instantaneous daily development rate of eggs to L3")]
    public float varDelta = -1;     // calculated with a formula;
    [Tooltip("Instantaneous daily mortality rate of eggs")]
    public float varMue = -1;       // calculated with a formula;
    [Tooltip("Instantaneous daily mortality rate of L3 in faeces")]
    public float varMul3 = -1;      // calculated with a formula;
    [Tooltip("Instantaneous daily L3 migration rate between faeces and pasture")]
    public float varM1 = 0.25f;
    public float varRho = -1;       // calculated with a formula;
    [Tooltip("Proportion of total pasture L3 that are found on herbage")]
    public float varM2 = 0.2f;
    [Tooltip("Probability of establishment of ingested L3")]
    public float varP = 0.4f;

    [Header("Host management")]
    public float varBeta = -1;      // calculated with a formula;
    [Tooltip("Daily herbage dry matter intake per host (kg dry matter per day)")]
    public float varC = 1.4f;
    [Tooltip("Host density or stocking density (sheep per ha)")]
    public float varH = 100.0f;     // Either regionally variable or held constant
    [Tooltip("Standing biomass (kg dry matter per ha)")]
    public float varB = 2000.0f;
    [Tooltip("Grazing area (ha)")]
    public float varA = 1.0f;

    [Header("Climate")]
    [Tooltip("Total daily precipitation (mm)")]
    public float varPP = 50;        // Daily variable
    [Tooltip("Daily potential evapotranspiration (mm per day)")]
    public float varE = -1;         // calculated with a formula;

    //TODO find a formula for this, instead of using an average value
    //https://www.researchgate.net/figure/The-monthly-average-daily-extraterrestrial-radiation-H-0-MJ-m-2-day-The-comparisons-of_fig2_261638000
    [Tooltip("Extra-terrestrial radiation (MJm-2day-1)")]
    public float varRa = 18;        // Daily variable, depends on the day of the year
    [Tooltip("Mean daily temperature (C)")]
    public float varTMean = 10;     // Daily variable
    [Tooltip("Minimum daily temperature (C)")]
    public float varTMin = 5;       // Daily variable
    [Tooltip("Maximum daily temperature (C)")]
    public float varTMax = 18;      // Daily variable

    [Tooltip("This variable was used as T in the paper but never described. Assuming they meant the current temperature (C)")]
    public float varT = 15;

    public float CalculateVarE()
    {
        return (float)(0.0023f * 0.408f * varRa * ((varTMax + varTMin) / 2.0f + 17.8f) * Mathf.Sqrt(varTMax - varTMin));
    }

    public float CalculateVarQ()
    {
        varE = CalculateVarE();
        varDelta = (float)(-0.09746f + 0.01063f * varTMean);
        varMue = Mathf.Exp(-1.3484f - 0.10488f * varT + 0.00230f * Mathf.Pow(varTMean, 2));

        //P/E >= 1 (d * m1) / ((ue + d) * (ul3 + m1))	
        //P/E < 1: 0
        if (varPP / varE >= 1)
        {
            return (float)((varDelta * varM1) / ((varMue + varDelta) * (varMul3 + varM1)));
        }

        return 0;
    }
    
    public float CalculateVarBeta()
    {
        return varC / (varB * varA);
    }

    public float CalculateVarRho()
    {
        varMul3 = Mathf.Exp(-2.62088f - 0.14399f * varT + 0.00462f * Mathf.Pow(varTMean, 2));
        return varMul3 / 3;
    }

    public float CalculateQ0()
    {
        varQ = CalculateVarQ();
        varBeta = CalculateVarBeta();
        varRho = CalculateVarRho();
        float varQ0 = ((varQ * varLabda) / varMu) * ((varBeta * varP) / (varRho + varBeta * varH)) * varH * varM2;

        return varQ0;
    }

    private void Start()
    {
        //TODO should get weather info and use its measurements in the formula

        var Q0 = CalculateQ0();
        Debug.Log("Q0=" + Q0);
    }
}
