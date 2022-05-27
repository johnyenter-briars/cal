//using CAL.Client;
//using CAL.Client.Models.Cal;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Xamarin.Forms;

//namespace CAL.Services
//{
//    public class EventsObservable : ObservableCollection<Event>
//    {
//        IEnumerable<Event> Events = new List<Event>(); 
//        public IDataStore<Event> EventDataStore => DependencyService.Get<IDataStore<Event>>();
//        public EventsObservable() : base()
//        {
//            Thread t = new Thread(async () =>
//            {
//                while (true)
//                {
//                    await RefreshEvents();
//                    await Task.Delay(10000);
//                }
//            })
//            {
//                IsBackground = true
//            };

//            t.Start();
//        }

//        public async Task RefreshEvents()
//        {
//            var events = await EventDataStore.GetItemsAsync(true);
//            Clear();
//            foreach (var e in events)
//            {
//                Add(e);
//            }
//        }

//        protected override void ClearItems()
//        {
//            base.ClearItems();
//        }

//        protected override void InsertItem(int index, Event e)
//        {
//            //if (index > _dt.Rows.Count) throw new ArgumentOutOfRangeException("Argument is out of range");
//            //if (index == _dt.Rows.Count)
//            //    _dt.Rows.Add(item);
//            //else
//            //    _dt.Rows.InsertAt(item, index);
//            base.InsertItem(index, e);
//        }

//        protected override void MoveItem(int oldIndex, int newIndex)
//        {
//            //if (oldIndex >= _dt.Rows.Count || newIndex >= _dt.Rows.Count)
//            //    throw new ArgumentOutOfRangeException("Argument is out of range");
//            //int MyNewIndex = newIndex; //so that I don't override anything that goes to base.MoveItem
//            //if (oldIndex < newIndex)
//            //    MyNewIndex--;
//            //RowType dr = (RowType)_dt.Rows[oldIndex];
//            //_dt.Rows.RemoveAt(oldIndex);
//            //if (MyNewIndex == _dt.Rows.Count)
//            //    _dt.Rows.Add(dr);
//            //else
//            //    _dt.Rows.InsertAt(dr, MyNewIndex);
//            //dr = null;
//            base.MoveItem(oldIndex, newIndex);
//        }

//        protected override void RemoveItem(int index)
//        {
//            //if (index >= _dt.Rows.Count) throw new ArgumentOutOfRangeException("Argument is out of range");
//            //_dt.Rows[index].Delete(); //Or if you do not need the data to persist in your data store, simply _dt.Rows.RemoveAt(index);
//            base.RemoveItem(index);
//        }

//        protected override void SetItem(int index, Event e)
//        {
//            //if (index >= _dt.Rows.Count) throw new ArgumentOutOfRangeException("Argument is out of range");
//            //_dt.Rows.RemoveAt(index);
//            //if (index > _dt.Rows.Count - 1)
//            //    _dt.Rows.Add(item);
//            //else
//            //    _dt.Rows.InsertAt(item, index);
//            base.SetItem(index, e);
//        }
//    }
//}
