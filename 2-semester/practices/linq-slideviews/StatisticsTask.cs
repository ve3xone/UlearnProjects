using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

// <summary>
// Класс, рассчитывающий среднее время нахождения пользователя на слайде
// </summary>
public class StatisticsTask
{
   /// <summary>
   /// Расчёт среднеего времени, затраченного пользователем на каждый из типов слайдов
   /// </summary>
   /// <param name="visits">Список с информацией о посещениях слайдов</param>
   /// <param name="slideType">Тип слайда</param>
   /// <returns>Среднее время нахождения пользователя на указанном типе слайдов</returns>
   public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
   {
       var minutes = new List<double>();

       var visitsByUsers = visits.GroupBy(visit => visit.UserId);
       foreach (var singleVisit in visitsByUsers)
       {
           TotalTime(singleVisit.OrderBy(visit => visit.DateTime).Bigrams(), minutes, slideType);
       }

       var filteredMinutes = minutes.Where(time => time >= TimeSpan.FromMinutes(1).TotalMinutes
                                                   && time <= TimeSpan.FromHours(2).TotalMinutes);
       if (!filteredMinutes.Any())
           return 0;

       return filteredMinutes.Median();
   }

   /// <summary>
   /// Расчёт общего времени нахождения пользователя на всех слайдах
   /// </summary>
   /// <param name="bigramsOfVisitsByUser">Биграмма посещений пользователя</param>
   /// <param name="minutes">Время нахождения в минутах</param>
   /// <param name="slideType">Тип слайда</param>
   private static void TotalTime(IEnumerable<(VisitRecord First, VisitRecord Second)> bigramsOfVisitsByUser,
                                 List<double> minutes,
                                 SlideType slideType)
   {
       double minutesToAdd = 0;
       foreach (var bigramOfVisits in bigramsOfVisitsByUser)
       {
           if (bigramOfVisits.First.SlideType == slideType)
           {
               minutesToAdd += (bigramOfVisits.Second.DateTime - bigramOfVisits.First.DateTime).TotalMinutes;
               if (bigramOfVisits.First.SlideId != bigramOfVisits.Second.SlideId)
               {
                   minutes.Add(minutesToAdd);
                   minutesToAdd = 0;
               }
           }
       }
   }
}