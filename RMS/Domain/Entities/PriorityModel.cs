namespace RMS.Domain.Entities
{
    public static class PriorityModel
    {
        public static string GetPriority(int num)
        {
            return num switch
            {
                1 => "Звичайний",
                2 => "Критичний",
                _ => ""
            };
        }
    }
}
