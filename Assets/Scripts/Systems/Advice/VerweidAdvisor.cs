using System;
using UnityEngine;
using System.Linq;

class VerweidAdvisor : MonoBehaviour
{	
	private static WeatherInfo weather;
	public static WeatherInfo Weather
	{
		get => weather;
		set
		{
			weather = value;
			if (weather != null)
			{
				CalcValue();
				//CronScheduler.instance.SetRepeat(this, "FireCalcValue", 3600);
			}
		}
	}
	
	private VerweidAdvisor()
	{
		EventSystem<WeatherInfo>.AddListener(EventType.weatherDataReceived, OnWeatherDataReceived);
	}

	~VerweidAdvisor()
	{
		EventSystem<WeatherInfo>.RemoveListener(EventType.weatherDataReceived, OnWeatherDataReceived);
	}

	private static void OnWeatherDataReceived(WeatherInfo info)
	{
		Weather = info;
	}

	private void FireCalcValue()
	{
		var result = CalcValue();
		EventSystem<double>.InvokeEvent(EventType.onAdviceValueCalculated, result);
	}

	static double calc_Q0(double q, double lambda, double mu, double beta, double p, double rho, double H, double m2)
	{
		double a = q * lambda / mu;
		double b = beta * p / (rho + beta * H);

		return a * b * H * m2;
	}

	static double calc_q(double P, double E, double delta, double m1, double mu_e, double mu_l3)
	{
		if (P / E < 1) return 0;
		return delta * m1 / ((mu_e + delta) * (mu_l3 + m1));
	}

	static double calc_delta(double T_mean)
	{
		return -0.09746 + 0.01063 * T_mean;
	}

	public static double CalcValue(WeatherInfo weatherInfo = null)
	{
		if (weatherInfo == null) weatherInfo = weather;

		double c = 1.4; // dialy herbage dry matter intake per host
		double B = 2000; // standing biomass
		double A = 1; // grazing area
		double q0; // basic reproductive quotient of macroparasites.
		//double A; // adult worm lifetime
		double lambda = 2250; // worm fecundity
		double mu = 0.05; // adult mortality
		double q;
		//double B; // number of adult parasites produced by each L3
		double p = 0.4; // establishment rate of ingested L3
		double beta = c / (B * A); // ingestion rate of L3 by host

		// quote https://mijngelderland.nl/inhoud/canons/ermelo/schaapskooi-ermelo: "De schaapskooi van 400m2 biedt plaats aan zo’n 300 schapen."
		// 400m2 is 0.04 hectare
		double H = 1;// 300 * 0.04; // number of hosts

		double m1 = 0.25; // daily L3 migration rate between faeces and pasture
		double m2 = 0.2; // proportion of total pasture L3 found on herbage

		Debug.Log("verweidadvisor weerbericht:");

		double[] temperatures = new double[24];
		Array.Copy(weatherInfo.hourly.temperature_2m, 0, temperatures, 0, 24);
		double temperature_max = temperatures.Max();
		double temperature_min = temperatures.Min();
		double temperature_average = temperatures.Average();
		Debug.Log("hoogste temperatuur = " + temperature_max);
		Debug.Log("laagste temperatuur = " + temperature_min);
		Debug.Log("gemiddelde temperatuur = " + temperature_average);

		// Neerslag doet wat raars met de formule, maakt Q0 vaak 0 bij lage waardes
		double[] precipitation = new double[24];
		Array.Copy(weatherInfo.hourly.precipitation, 0, precipitation, 0, 24);
		Debug.Log("totale neerslag = " + precipitation.Sum());
 
		//double[] neerslag = { 19, 17, 22, 12, 20, 24, 28, 21, 12, 14, 14, 20 };

		//Eextra terrestial radiation. Iets met zonnestraling, kan niet uit de weerinformatie gehaald worden
		double[] wereld_Ra = { 20.8, 23, 21.6, 19.2, 18.8, 16.1, 14.7, 14.1, 16, 16.8, 20.6, 20.7 };
		int m = DateTime.Now.Month - 1; //6

		double P = precipitation.Sum() * 1000;//neerslag[m]; * 1000 om te testen zodat ie geen 0 blijft
		double Ra = wereld_Ra[m];
		double E = 0.0023 * 0.408 * Ra * ((temperature_max + temperature_min) / 2 + 17.8) * Math.Sqrt(temperature_max - temperature_min);

		double delta = calc_delta(temperature_average);
		double mu_e = Math.Exp(-1.3484 - 0.10488 * temperature_average + 0.00230 * temperature_average * temperature_average);
		double mu_l3 = Math.Exp(-2.62088 - 0.14399 * temperature_average + 0.00462 * temperature_average * temperature_average);

		/*
		Debug.Log("P      = " + P);
		Debug.Log("Ra     = " + Ra);
		Debug.Log("E      = " + E);
		Debug.Log("T_mean = " + temperature_average);
		Debug.Log("delta  = " + delta);
		Debug.Log("mu_e   = " + mu_e);
		Debug.Log("mu_l3  = " + mu_l3);
		*/

		q = calc_q(P, E, delta, m1, mu_e, mu_l3); // probability egg will develop into L3

		//Debug.Log("q = " + q);

		double rho = mu_l3 / 3; // mortality rate of L3 on pasture

		/*
		Debug.Log("lambda = " + lambda);
		Debug.Log("mu     = " + mu);
		Debug.Log("beta   = " + beta);
		Debug.Log("p      = " + p);
		Debug.Log("rho    = " + rho);
		Debug.Log("H      = " +  H);
		Debug.Log("m2     = " + m2);
		Debug.Log(" ");
		*/
		Debug.Log("q0     = " + calc_Q0(q, lambda, mu, beta, p, rho, H, m2));
		return calc_Q0(q, lambda, mu, beta, p, rho, H, m2);
	}
}
