using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain
{
	public class DataManager
	{
		public ITextFieldsRepository? TextFields { get; set; }
		public IServiceItemsRepository? ServiceItems { get; set; }

		public DataManager(ITextFieldsRepository textFieldsRepository, IServiceItemsRepository serviceItemsRepository)
		{
			TextFields = textFieldsRepository;
			ServiceItems = serviceItemsRepository;
		}

	}
}
