using System.Collections.ObjectModel;
using System.Globalization;

namespace GoMeowee.Models;

public class EventsDayGroup : ObservableCollection<EventListItemDto>
{
    public DateTime Date { get; }

    public string Header =>
        Date.ToString("dddd, dd. MMM", CultureInfo.InvariantCulture);

    public EventsDayGroup(DateTime date, IEnumerable<EventListItemDto> events)
        : base(events)
    {
        Date = date;
    }
}
