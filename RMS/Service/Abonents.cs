using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RMS.Domain.Entities;
using System.Diagnostics;
using System.Text;

namespace RMS.Service
{
	public class Abonents
	{
		
		public string apiUrl = "https://demo.abills.net.ua:9443/api.cgi/users/all?password&fio&locationId&phone&pageRows=2810";
		
		public string apiKey = "testAPI_KEY12";

		private readonly IMemoryCache _memoryCache;
		private const string CacheKey = "CachedAbonents";
		private static bool _isCacheInitialized = false;
		public Abonents() { }

		public Abonents(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}
		public async Task<Abonent> SearchAbonentByUID(int uid)
		{
			return await GetUserFromApi(uid);
		}
		public async Task<List<Abonent>> SearchAbonents(string text)
		{
			List<Abonent> data;
			List<Abonent> usersByField = new List<Abonent>();

			if (!_isCacheInitialized)
			{
				data = await GetUsersFromApi();
				_isCacheInitialized = true;
			}
			else
			{
				// Попытка получить данные из кэша
				if (!_memoryCache.TryGetValue(CacheKey, out data))
				{
					data = await GetUsersFromApi();
				}
			}

			foreach (var user in data)
			{
				foreach (var property in typeof(Abonent).GetProperties())
				{
					var fieldValue = property.GetValue(user)?.ToString().ToLower();
					if (fieldValue != null)
					{
						if (user.fio != "  ")
						{
							if (fieldValue.Contains(text))
							{
								if (usersByField.Contains(user))
									continue;
								usersByField.Add(user);
							}
						}
					}
				}
			}

			return usersByField;
		}
		private async Task<Abonent> GetUserFromApi(int uid)
		{
			string apiUserUrl = $"https://demo.abills.net.ua:9443/api.cgi/users/{uid}/pi";
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("KEY", apiKey);

				HttpResponseMessage response = await client.GetAsync(apiUserUrl);

				if (response.IsSuccessStatusCode)
				{
					var byteArray = await response.Content.ReadAsByteArrayAsync();
					var jsonString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

					// Декодируем строку JSON для правильной обработки экранирования обратных слешей
					jsonString = System.Text.RegularExpressions.Regex.Unescape(jsonString);

					// Используем Json.NET для десериализации JSON-строки
					var data = JsonConvert.DeserializeObject<Abonent>(jsonString);

					return data;
				}
				else
				{
					// Обработка ошибки, если не удалось получить данные
					return new Abonent();
				}
			}
		}
		private async Task<List<Abonent>> GetUsersFromApi()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("KEY", apiKey);

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					var byteArray = await response.Content.ReadAsByteArrayAsync();
					var jsonString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

					// Декодируем строку JSON для правильной обработки экранирования обратных слешей
					jsonString = System.Text.RegularExpressions.Regex.Unescape(jsonString);

					// Используем Json.NET для десериализации JSON-строки
					var data = JsonConvert.DeserializeObject<List<Abonent>>(jsonString);

					// Добавляем данные в кэш на 1 минуту
					var cacheEntryOptions = new MemoryCacheEntryOptions
					{
						AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
					};

					_memoryCache.Set(CacheKey, data, cacheEntryOptions);

					return data;
				}
				else
				{
					// Обработка ошибки, если не удалось получить данные
					return new List<Abonent>();
				}
			}
		}
	}
}
