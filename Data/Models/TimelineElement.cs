using System;
namespace Portfolio.Data.Models
{
	public class TimelineElement
	{
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}

