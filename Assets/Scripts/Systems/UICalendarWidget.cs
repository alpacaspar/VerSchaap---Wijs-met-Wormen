using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UICalendarWidget : MonoBehaviour
{
    public RectTransform calendarDayPanel;
    public GameObject calendarCellObject;

    public DateTimeOffset timeStamp;
    int currentYear;
    int currentMonth;
    int currentDay = 1;

    public Button btnPrevMonth;
    public Button btnNextMonth;
    public TextMeshProUGUI txtDate;

    public SheepDataViewer sheepDataReader;

    // Start is called before the first frame update
    private void Awake()
    {
        timeStamp = new DateTimeOffset(DateTime.UtcNow);//.ToUnixTimeSeconds();
        currentYear = timeStamp.Year;
        currentMonth = timeStamp.Month;
        txtDate.SetText(currentMonth + "-" + currentYear);
        btnPrevMonth.onClick.AddListener(delegate { ChangeCurrentDate(-1); });
        btnNextMonth.onClick.AddListener(delegate { ChangeCurrentDate(1); });
        CreateDayCells();
    }

    private int GetDaysInMonth()
    {
        int yr = currentYear - 1972;
        bool bIsLeapYear = yr % 4 == 0;
        int daysInMonth = 31;

        // februari
        if (currentMonth == 2)
        {
            daysInMonth = bIsLeapYear ? 29 : 28;
        }

        // april, juni, september, november
        else if (currentMonth == 4 || currentMonth == 6 || currentMonth == 9 || currentMonth == 11)
        {
            daysInMonth = 30;
        }

        return daysInMonth;
    }

    private void UpdateDayCells()
    {
        int daysInMonth = GetDaysInMonth();

        for (int i = 0; i < calendarDayPanel.childCount; i++)
        {
            calendarDayPanel.GetChild(i).gameObject.SetActive(i < daysInMonth);
        }
    }

    public void ChangeCurrentDate(int divMonth)
    {
        // this will not work if divmonth isnt -1 or 1
        currentMonth += divMonth;
        if (currentMonth > 12)
        {
            currentYear++;
            currentMonth = 1;
        }
        else if (currentMonth < 1)
        {
            currentYear--;
            currentMonth = 12;
        }

        SetDate(currentDay);
        UpdateDayCells();
    }

    private void SetDate(DateTimeOffset time)
    {
        timeStamp = time;
        currentDay = timeStamp.Day;
        currentMonth = timeStamp.Month;
        currentYear = timeStamp.Year;
        txtDate.SetText(currentDay + "-" + currentMonth + "-" + currentYear);
        sheepDataReader.tmpTime = time;
        UpdateDayCells();
    }

    public void SetDate(long time)
    {
        SetDate(DateTimeOffset.FromUnixTimeSeconds(time));
    }

    private void SetDate(int day)
    {
        currentDay = day;
        int daysInMonth = GetDaysInMonth();
        if (currentDay > daysInMonth) currentDay = daysInMonth;
        SetDate(new DateTimeOffset(currentYear, currentMonth, currentDay, 1, 1, 1, TimeSpan.Zero));
    }

    private void CreateDayCells()
    {
        for (int i = 0; i < 31; i++)
        {
            GameObject dayCell = Instantiate(calendarCellObject, calendarDayPanel);
            Button btnDayCell = dayCell.GetComponent<Button>();
            int j = i + 1;
            btnDayCell.onClick.AddListener(delegate
            {
                SetDate(j);
                sheepDataReader.UpdateTSButton(timeStamp);
                gameObject.SetActive(false);
            });
            TextMeshProUGUI txt = dayCell.GetComponentInChildren<TextMeshProUGUI>();
            txt.SetText((i + 1).ToString());
        }

        UpdateDayCells();
    }
}
