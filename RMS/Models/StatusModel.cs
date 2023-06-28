namespace RMS.Models
{
    public static class StatusModel
    {
        public static string GetStatus(int num)
        {
            switch (num)
            {
                case 1:
                    return "Відкрито";
                case 2:
                    return "Виконується";
                case 3:
                    return "Закрито";
                case 4:
                    return "Відміна";
                default:
                    return "";
            }
        }
    }
}
