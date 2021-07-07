namespace ExpertWaves {
	namespace Data {
			public interface IBaseData {
				public void onLoadFileNotFound();
				public void onBeforeLoad();
				public void onAfterLoad();
				public void onBeforeSave();
				public void onAfterSave();
			}
		
	}
}