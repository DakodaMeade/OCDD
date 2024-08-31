namespace OCDD.Models
{
    // Dakoda Meade
    // Time Slot Model Class
    // Model class to represent a time slot 
    // Used to detirmine a slot of time to be avaivable or not
    public class TimeSlotModel
    {
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
