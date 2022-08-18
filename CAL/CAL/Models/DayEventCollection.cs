using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Interfaces;

namespace CAL.Models
{
    public class DayEventCollection<T> : List<T>, IPersonalizableDayEvent
    {
        /// <summary>
        /// Empty contructor extends from base()
        /// </summary>
        public DayEventCollection() : base()
        { }

        /// <summary>
        /// Color contructor extends from base()
        /// </summary>
        /// <param name="eventIndicatorColor"></param>
        /// <param name="eventIndicatorSelectedColor"></param>
        public DayEventCollection(Color? eventIndicatorColor, Color? eventIndicatorSelectedColor) : base()
        {
            EventIndicatorColor = eventIndicatorColor;
            EventIndicatorSelectedColor = eventIndicatorSelectedColor;
        }

        /// <summary>
        /// IEnumerable contructor extends from base(IEnumerable collection)
        /// </summary>
        /// <param name="collection"></param>
        public DayEventCollection(IEnumerable<T> collection) : base(collection)
        { }

        /// <summary>
        /// Capacity contructor extends from base(int capacity)
        /// </summary>
        /// <param name="capacity"></param>
        public DayEventCollection(int capacity) : base(capacity)
        { }

        #region PersonalizableProperties
        public Color? EventIndicatorColor { get; set; } = Color.Purple;
        public Color? EventIndicatorSelectedColor { get; set; } = Color.Purple;
        public Color? EventIndicatorTextColor { get; set; } = Color.Purple;
        public Color? EventIndicatorSelectedTextColor { get; set; } = Color.Purple;

        #endregion
    }
}
