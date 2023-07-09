namespace RMS.Models
{
    public static class StatusModel
    {
        public static string GetStatus(int num)
        {
			return num switch
			{
				1 => "Відкрито",
				2 => "Виконується",
                3 => "Закрито",
                4 => "Відміна",
				_ => ""
			};
		}
    }
}
