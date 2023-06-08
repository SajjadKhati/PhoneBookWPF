public class TestInModel
{
	public async Task Test()
	{
		using (IRepositoryAggregate<PersonEntity> repository = new Repository<PersonEntity>(new PhoneBookDbContext()))
		{
			await repository.InitializeDatabaseAsync();

			PersonEntity mainEntity = repository.FindByLazyLoadingMode<PersonEntity>(1);
			// ویرایش پروپرتی های scalar برای موجودیت PersonEntity
			mainEntity.FirstName = "سردار مقاومت احمد";
			mainEntity.LastName = "متوسلیان";

			mainEntity.MobileNumbers = mainEntity.MobileNumbers ?? new List<MobileNumberEntity>();
			// ویرایش ، شامل حذف و اضافه و ویرایش موجودیت MobileNumberEntity
			MobileNumberEntity addMne = new MobileNumberEntity() {MobileNumber = "0112233"};
			mainEntity.MobileNumbers.Add(addMne);

			MobileNumberEntity deleteMne = mainEntity.MobileNumbers.FirstOrDefault(mobileNumberEntity => mobileNumberEntity.Id == 7);
			mainEntity.MobileNumbers.Remove(deleteMne);
			repository.Remove<MobileNumberEntity>(deleteMne);

			mainEntity.MobileNumbers.FirstOrDefault(mobileNumberEntity => mobileNumberEntity.Id == 6).MobileNumber = "987654";

			/////////////////////////////
			// ویرایش ، شامل حذف و اضافه و ویرایش موجودیت AddressEntity
			mainEntity.Addresses = mainEntity.Addresses ?? new List<AddressEntity>();

			AddressEntity addAe = new AddressEntity {Province = "تهران", City = "شهر تهران"};
			mainEntity.Addresses.Add(addAe);

			AddressEntity deleteAe = mainEntity.Addresses.FirstOrDefault(addressEntity => addressEntity.Id == 3);
			mainEntity.Addresses.Remove(deleteAe);
			if (deleteAe.Persons.Count == 1)
				repository.Remove<AddressEntity>(deleteAe);

			var editAe = mainEntity.Addresses?.FirstOrDefault(addressEntity => addressEntity.Id == 1);
			editAe.Province = "فلسطین";
			editAe.City = "شهر غزه";

			// انجام ویرایش و ذخیره
			repository.Edit(mainEntity.Id, mainEntity);
			repository.SaveChanges();

			// بازیابی شناسه های مربوط به موجودیت های اضافه شده
			int addAeId = addAe.Id;
			int addMneId = addMne.Id;
		}

		///////////////

		//using (IRepositoryBase<PersonEntity> repository = new Repository<PersonEntity>(new PhoneBookDbContext()))
		//{
		//    await repository.InitializeDatabaseAsync();

		//    var entity = repository.FindByLazyLoadingMode<PersonEntity>(1);
		//    var mne = repository.FindByLazyLoadingMode<MobileNumberEntity>(2);
		//    entity.MobileNumbers.Remove(mne);

		//    repository.Edit(entity.Id, entity);
		//    repository.SaveChanges();
		//}


		//TestInDataAccess testInDataAccess = new TestInDataAccess();
		//await testInDataAccess.Test();
	}


	public async void Test_2()
	{
		using (IRepositoryAggregate<PersonEntity> repository = new Repository<PersonEntity>(new PhoneBookDbContext()))
		{
			await repository.InitializeDatabaseAsync();

			PersonEntity personEntity = new PersonEntity();
			personEntity.Id = 987;
			personEntity.FirstName = "تست";
			personEntity.LastName = "دو بار تست";
			personEntity.MobileNumbers = new ObservableCollection<MobileNumberEntity>()
			{
				new MobileNumberEntity(){Id = 45, MobileNumber = "78"},
				new MobileNumberEntity(){Id = 54, MobileNumber = "87"}
			};

			var aa = repository.Add(personEntity);
			repository.SaveChanges();
		}
	}


	public async Task Test_3()
	{
		using (IRepositoryAggregate<PersonEntity> repository = new Repository<PersonEntity>(new PhoneBookDbContext()))
		{
			await repository.InitializeDatabaseAsync();

			PersonEntity mainEntity = repository.FindByLazyLoadingMode<PersonEntity>(1);
			// ویرایش پروپرتی های scalar برای موجودیت PersonEntity
			mainEntity.FirstName = "امیر علی";
			mainEntity.LastName = "افشردی";

			List<MobileNumberEntity> removeMobileEntities = new List<MobileNumberEntity>();
			List<AddressEntity> removeAddressEntities = new List<AddressEntity>();

			foreach (MobileNumberEntity mobileEntity in mainEntity.MobileNumbers.ToList())
			{
				mainEntity.MobileNumbers.Remove(mobileEntity);
				removeMobileEntities.Add(mobileEntity);
				//repository.Remove(mobileEntity);
			}

			foreach (AddressEntity addressEntity in mainEntity.Addresses.ToList())
			{
				mainEntity.Addresses.Remove(addressEntity);
				if(addressEntity.Persons != null && addressEntity.Persons.Count == 1)
					removeAddressEntities.Add(addressEntity);
			}

			//////

			ICollection<MobileNumberEntity> mobileNumbers = new List<MobileNumberEntity>()
			{
				new MobileNumberEntity(){Id = 1, MobileNumber = "2222", PersonEntity = mainEntity},
				new MobileNumberEntity(){Id = 2, MobileNumber = "45", PersonEntity=mainEntity},
				new MobileNumberEntity(){Id = 3, MobileNumber = "4444", PersonEntity=mainEntity}
			};

			foreach (MobileNumberEntity mobileNumber in mobileNumbers)
			{
				mainEntity.MobileNumbers.Add(mobileNumber);
			}

			AddressEntity addressEntity1 = new AddressEntity()
				{Id = 1, Province = "خراسان رضوی", City = "مشهد", Persons = new List<PersonEntity>()};
			addressEntity1.Persons.Add(mainEntity);

			ICollection<AddressEntity> addressEntities = new List<AddressEntity>();
			addressEntities.Add(addressEntity1);

			foreach (AddressEntity myAddressEntity in addressEntities)
			{
				mainEntity.Addresses.Add(myAddressEntity);
			}

			////////

			foreach (MobileNumberEntity removeMobileEntity in removeMobileEntities)
			{
				repository.Remove(removeMobileEntity);
			}

			foreach (AddressEntity removeAddressEntity in removeAddressEntities)
			{
				repository.Remove(removeAddressEntity);
			}

			repository.Edit(mainEntity.Id, mainEntity);
			repository.SaveChanges();

			//// بازیابی شناسه های مربوط به موجودیت های اضافه شده
			//int addAeId = addAe.Id;
			//int addMneId = addMne.Id;
		}
	}

}
