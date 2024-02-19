using System;
namespace Portfolio.Model.Data
{
	public class TimelineElement
	{
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int CreationDate { get; set; }
    }
}

